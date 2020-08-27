using System;
using System.Text;

namespace GoogleSubscription
{
    public class GoogleMessage
    {
        public MessageDetails Message { get; set; }

        public string Subscription { get; set; }

        public string GetMessage()
        {
            if (Message == null)
                return "Null message";

            byte[] data = Convert.FromBase64String(Message.Data);
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }
    }

    public class MessageDetails
    {
        public string MessageId { get; set; }

        public string Message_Id { get; set; }

        public string Data { get; set; }

        public string PublishTime { get; set; }

        public string Publish_Time { get; set; }
    }
}
