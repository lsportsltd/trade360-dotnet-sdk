using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class LeagueSubscription
    {
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int LeagueId { get; set; }
    }

    public class LeagueSubscriptionRequest : BaseRequest
    {
        public List<LeagueSubscription> Subscriptions { get; set; }
    }
}
