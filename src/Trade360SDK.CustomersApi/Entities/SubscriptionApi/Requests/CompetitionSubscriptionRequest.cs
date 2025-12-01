using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests
{
    public class CompetitionSubscription
    {
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int LeagueId { get; set; }
    }

    public class CompetitionSubscriptionRequest : BaseRequest
    {
        public List<CompetitionSubscription>? Subscriptions { get; set; }
    }
}
