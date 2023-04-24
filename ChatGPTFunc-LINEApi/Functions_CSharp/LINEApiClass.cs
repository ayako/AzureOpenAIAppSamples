namespace ChatGPTFunc_CSharp
{
    #pragma warning disable IDE1006
    public class LINEApiRequest
    {
        public string destination { get; set; }
        public Event[] events { get; set; }
    }

    public class LINEApiPostContent
    {
        public string replyToken { get; set; }
        public Message[] messages { get; set; }
    }

    public class Event
    {
        public string type { get; set; }
        public Message message { get; set; }
        public long timestamp { get; set; }
        public Source source { get; set; }
        public string replyToken { get; set; }
        public string mode { get; set; }
        public string webhookEventId { get; set; }
        public Deliverycontext deliveryContext { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Source
    {
        public string type { get; set; }
        public string userId { get; set; }
    }

    public class Deliverycontext
    {
        public bool isRedelivery { get; set; }
    }
    #pragma warning restore
}
