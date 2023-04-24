const aoai_url = 'https://YOUR_AOAI_SERVICE.openai.azure.com/openai/deployments/YOUR_gtp-35-turbo_NAME/chat/completions?api-version=2023-03-15-preview';
const aoai_key = 'YOUR_AOAI_KEY';
const lineapi_url = 'https://api.line.me/v2/bot/message/reply';
const lineapi_token = 'YOUR_LINE_API_CHANNEL_ACCESS_TOKEN';

const util = require('util');
const request = require('request');
const requestPromise = util.promisify(request);

module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');
    var responseMsg = '';

    if (req.body){
        if (req.body.events.length > 0 && req.body.events[0].type == 'message'){
            const question = req.body.events[0].message.text;
            const aoai_answer = await postToAOAI(question);
            const lineapi_result = await replyToUser(aoai_answer, req.body.events[0].replyToken);

            if (lineapi_result){
              responseMsg = 'sent answer to LINE API successfully.'
            }
            else{
              responseMsg = 'got error to POST to LINE API.'
            }
        }
        else{
            responseMsg = 'got request body (not message).';
        }
    }    
    else{
        responseMsg = 'got access.';
    }

    context.res = {
      status: 200, /* Defaults to 200 */
      body: responseMsg
    }

}

async function postToAOAI(question) {
    var req_body = {
      "messages": [
        {
          "role": "system",
          "content": "あなたは「しま〇ろう」というキャラクターです。0-6歳の子供が分かるように話してください。また、口調は親切で親しみやすくしてください。"
        },
        {
          "role": "user",
          "content": question
        },
        {
          "role": "assistant",
          "content": ""
        }
      ],
      "temperature": 0.7,
      "top_p": 0.95,
      "frequency_penalty": 0,
      "presence_penalty": 0,
      "max_tokens": 800,
      "stop": null
    }

    try{
      var response = await requestPromise(
        {
          method:'post',
          url: aoai_url,
          headers: { 'Content-Type': 'application/json', 'api-key': aoai_key },
          body: JSON.stringify(req_body)
        });
      return JSON.parse(response.body).choices[0].message.content;
    }
    catch(error){
      console.error('AOAI Post Error:', error.code + ': ' + error.message)
      return null;
    }
}

async function replyToUser(answer,reply_token){
    var req_body = {
        "messages": [
          {
            "text": answer,
            "type": "text"
          }
        ],
        "replyToken": reply_token
      }
    
    try{
      var response = await requestPromise(
        {
          method:'post',
          url: lineapi_url,
          headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + lineapi_token },
          body: JSON.stringify(req_body)
        });
      return true;
    }
    catch(error){
      console.error('LINE Api Post Error:', error.code + ': ' + error.message)
      return false;
    }
}