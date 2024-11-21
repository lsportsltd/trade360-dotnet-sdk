using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses
{
    public class LeagueSubscriptionCollectionResponse
    {
        public List<LeagueSubscriptionResponse>? Subscription { get; set; }
    }

    public class LeagueSubscriptionResponse
    {
        public int LeagueId { get; set; }
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
