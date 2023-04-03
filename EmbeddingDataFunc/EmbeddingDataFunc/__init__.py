import logging

import azure.functions as func
import openai
import re
from num2words import num2words
import pandas as pd
from openai.embeddings_utils import get_embedding, cosine_similarity
from transformers import GPT2TokenizerFast
import json

from azure.storage.blob import BlobServiceClient

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('EmbeddingDataFunc HTTP trigger function processed a request.')

    # Set Azure OpenAI Service & Blob Storage Info
    API_KEY = "YOUR_API_KEY" 
    RESOURCE_ENDPOINT = "https://YOUR_SERVICE_NAME.openai.azure.com/" 
    BLOB_CONNECTION_STRING = "YOUR_CONNECTION_STRING"

    openai.api_type = "azure"
    openai.api_key = API_KEY
    openai.api_base = RESOURCE_ENDPOINT
    openai.api_version = "2022-12-01"
    url = openai.api_base + "/openai/deployments?api-version=2022-12-01"

    # Set Azure Storage Info
    blob_service_client = BlobServiceClient.from_connection_string(BLOB_CONNECTION_STRING)

    # Get and Read Embedding Data from Azure Blob Storage
    containerClient = blob_service_client.get_container_client(container="embedding-data")
    blobList = containerClient.list_blobs()
    for blob in blobList:
        file = containerClient.download_blob(blob.name)
        df = pd.read_csv(file)

    # Clean Up Data
    def normalize_text(s, sep_token = " \n "):
        s = re.sub(r'\s+',  ' ', s).strip()
        s = re.sub(r". ,","",s)
        # remove all instances of multiple spaces
        s = s.replace("..",".")
        s = s.replace(". .",".")
        s = s.replace("\n", "")
        s = s.strip()
        return s

    df['text'] = df["text"].apply(lambda x : normalize_text(x))

    # Tokenize Data
    tokenizer = GPT2TokenizerFast.from_pretrained("gpt2")
    df['n_tokens'] = df["text"].apply(lambda x: len(tokenizer.encode(x)))
    df = df[df.n_tokens<2000]

    # Enbedding Data
    df['curie_search'] = df["text"].apply(lambda x : get_embedding(x, engine = 'text-search-curie-doc-001'))

    # Create function to search
    def search_docs(df, user_query, top_n=3, to_print=True):
        embedding = get_embedding(
            user_query,
            engine="text-search-curie-query-001"
        )
        df["similarities"] = df.curie_search.apply(lambda x: cosine_similarity(x, embedding))

        res = (
            df.sort_values("similarities", ascending=False)
            .head(top_n)
        )
        return res

    # Get User Input for Search Data
    q = req.params.get('q')
    if not q:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            q = req_body.get('q')

    if q:
        # Search Data
        res = search_docs(df, q, top_n=4)
        jsondata = []
        for i in range(4):
            data = { "tweetText": res.iloc[i]['text'], 
                    "tweetId": str(res.iloc[i]['tweetId']), 
                    "userId": str(res.iloc[i]['userId']), 
                    "similarity": str(res.iloc[i]['similarities']) }
            jsondata.append(data)
        
        answer = json.dumps(jsondata, ensure_ascii=False)
        return func.HttpResponse(answer)
    
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Add q in the query string or in the request body for searched response.",
             status_code=200
        )
