using System;

namespace GoogleSubscription
{
    public class SubscriptionNotification
    {
        public string Version { get; set; }
        public int NotificationType { get; set; }
        public string PurchaseToken { get; set; }
        public string SubscriptionId { get; set; }
    }
}