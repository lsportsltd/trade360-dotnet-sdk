using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests
{
    public class LeagueSubscription
    {
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int LeagueId { get; set; }
    }

    public class LeagueSubscriptionRequest : BaseRequest
    {
        public List<LeagueSubscription>? Subscriptions { get; set; }
    }
}
