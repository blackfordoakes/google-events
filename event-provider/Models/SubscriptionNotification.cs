using System;

namespace Events.Provider.Models
{
    public class SubscriptionNotification
    {
        public string Version { get; set; }
        public int NotificationType { get; set; }
        public string PurchaseToken { get; set; }
        public string SubscriptionId { get; set; }
    }
}
