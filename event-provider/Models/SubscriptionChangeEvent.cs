using System;

namespace Events.Provider.Models
{
    public class SubscriptionChangeEvent
    {
        public string Version { get; set; }
        public string PackageName { get; set; }
        public string EventTimeMillis { get; set; }
        public SubscriptionNotification SubscriptionNotification { get; set; }
    }
}
