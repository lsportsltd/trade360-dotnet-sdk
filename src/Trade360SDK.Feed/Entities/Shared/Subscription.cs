using Trade360SDK.Feed.Entities.Enums;

namespace Trade360SDK.Feed.Entities.Shared
{
    public class Subscription
    {
        public int Type { get; set; }
        public SubscriptionStatus Status { get; set; }
    }
}
