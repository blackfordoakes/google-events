using System;
using Newtonsoft.Json;

namespace GoogleSubscription
{
    public class SubscriptionChangeEvent
    {
        public string Version { get; set; }
        public string PackageName { get; set; }
        public string EventTimeMillis { get; set; }
        public SubscriptionNotification SubscriptionNotification { get; set; }

        public static SubscriptionChangeEvent Parse(string json)
        {
            return JsonConvert.DeserializeObject<SubscriptionChangeEvent>(json);
        }
    }
}
