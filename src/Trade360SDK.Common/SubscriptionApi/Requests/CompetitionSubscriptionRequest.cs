using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class CompetitionSubscription
    {
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int CompetitionId { get; set; }
    }

    public class CompetitionSubscriptionRequest : BaseRequest
    {
        public List<CompetitionSubscription> Subscriptions { get; set; }
    }
}
