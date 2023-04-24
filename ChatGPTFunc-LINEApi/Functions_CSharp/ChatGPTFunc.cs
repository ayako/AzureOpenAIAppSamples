using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.AI.OpenAI;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ChatGPTFunc_CSharp
{
    public static class ChatGPTFunc
    {
        private static readonly string aoaiUrl = "https://YOUR_AOAI_SERVICE.openai.azure.com/";
        private static readonly string aoaiKey = "YOUR_AOAI_KEY";
        private static readonly string aoaiModelName = "YOUR_gtp-35-turbo_NAME";
        private static readonly string lineApiToken = "YOUR_LINE_API_TOKEN";

        private static readonly HttpClient _client;
        static ChatGPTFunc()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + lineApiToken);
        }


        [FunctionName("ChatGPTFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string responseMessage;

            if (!string.IsNullOrEmpty(requestBody))
            {
                LINEApiRequest lineApiRequest = JsonSerializer.Deserialize<LINEApiRequest>(requestBody);
                if (lineApiRequest != null && lineApiRequest.events.Length > 0 && lineApiRequest.events[0].type == "message")
                {

                    // send question to AOAI and get answer

                    string question = lineApiRequest.events[0].message.text;

                    var aoaiClient = new OpenAIClient(new Uri(aoaiUrl), new AzureKeyCredential(aoaiKey));
                    Response<ChatCompletions> aoaiResponse = await aoaiClient.GetChatCompletionsAsync(
                        aoaiModelName,
                        new ChatCompletionsOptions()
                        {
                            Messages =
                            {
                                new ChatMessage(ChatRole.System, @"あなたは「しま〇ろう」というキャラクターです。0-6歳の子供が分かるように話してください。また、口調は親切で親しみやすくしてください。"),
                                new ChatMessage(ChatRole.User, question),
                                new ChatMessage(ChatRole.Assistant, ""),
                            },
                            Temperature = (float)0.7,
                            MaxTokens = 800,
                            NucleusSamplingFactor = (float)0.95,
                            FrequencyPenalty = 0,
                            PresencePenalty = 0
                        });

                    ChatCompletions aoaiCompletions = aoaiResponse.Value;
                    string aoaiAnswer = aoaiCompletions.Choices[0].Message.Content;


                    // Send respone to user via LINE Messaging API

                    var lineApiPostContent = new LINEApiPostContent()
                    {
                        replyToken = lineApiRequest.events[0].replyToken,
                        messages = new Message[]
                        {
                            new Message()
                            {
                                type = "text",
                                text = aoaiAnswer
                            }
                        }
                    };

                    var stringContent = new StringContent(JsonSerializer.Serialize(lineApiPostContent), encoding: Encoding.UTF8, "application/json");
                    HttpResponseMessage lineApiPostResult = await _client.PostAsync("https://api.line.me/v2/bot/message/reply", stringContent);
                    if (lineApiPostResult.StatusCode == HttpStatusCode.OK)
                    {
                        responseMessage = "sent answer to LINE API successfully.";
                    }
                    else
                    {
                        responseMessage = "got error to Post to LINE API.";
                    }

                }
                else
                {
                    responseMessage = "got request body (not message).";
                }

            }
            else
            {
                responseMessage = "got access.";
            }

            log.LogInformation(responseMessage);
            return new OkObjectResult(responseMessage);

        }
    }

}
