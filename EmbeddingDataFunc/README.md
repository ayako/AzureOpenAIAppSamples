# Azure OpenAI Embeddings Azure Function Sample Code (as of 2023/04/03)

This is Azure Function HTTP Request (API) sample code using Azure OpenAI Embedding, reading Azure Blob Storage CSV files as embedding data.

Azure Blob Storage に置いた CSV ファイルを Embedding データとして読み込んで、Azure Open Embedding を HTTP Request (API) として使うサンプルコードです。

## 利用方法

English follows -> See [How to use](#how-to-use)

### 準備

- お持ちの Azure Subscription で Azure OpenAI Service の利用を申請＆許諾を得たのち、Azure OpenAI Service を作成し、API Key と Endpoint を取得しておきます。
- Azure Storage を作成 (または既存の Storage でも可)、'embedding-data' という名前で Blob Container を作成 (アクセスレベルは private のままで OK) します。Blob の Connection String を取得しておきます。
- Embedding に利用するデータを CSV ファイルで用意し、作成した Blob にアップロードしておきます。今回は 'tweetId', 'tweetText', 'userId' という3つの列を持つデータを利用しています。

### Azure OpenAI Service および Azure Storage 情報の設定

- Azure OpenAI Service および Azure Storage Blob 作成時に取得した情報を _init.py_ に設定します (18-20行目)。
- 本番利用では ソースコード内にこのような情報を **直接記載しないでください** 。こちらのコードは検証|ハンズオン用です。

### Response Body の設定

- HTTP request に対する response body 設定箇所 (88-91行目) を、embedding しているデータに合わせて修正してください。


## How to use

### Prep

- Apply and get approved Azure OpenAI Service acccess with your Azure Subscription, and create your Azure OpenAI Service, and get API Key and Endpoint.
- Create (or use existing) Azure Storage, and create Blob Container named 'embedding-data', with access level: private. Get Connection String of Blob.
- Be ready CSV file(s) for embedding data, and upload to Blob created. In this sample, csv file expected to have 3 collumns named 'tweetId','tweetText','userId'.

### Settings Azure OpenAI Service, Azure Storage info in code | Azure OpenAI Service

- Embed your Azure OpenAI Service and Storage Blob info into _init.py_ (line 18-20).
- **DO NOT** embed those info direct into source code for production use. This is only testing|hands-on purpose.

### Settings for response Body

- Change response body to HTTP request in _init_py_ (line 88-91), align with your embedding data.