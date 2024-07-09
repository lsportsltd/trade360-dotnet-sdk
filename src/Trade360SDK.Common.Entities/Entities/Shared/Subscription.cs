using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Shared
{
    public class Subscription
    {
        public int Type { get; set; }
        public SubscriptionStatus Status { get; set; }
    }
}
