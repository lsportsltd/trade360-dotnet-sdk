using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses
{
    public class CompetitionSubscriptionCollectionResponse
    {
        public List<CompetitionSubscriptionResponse>? Subscription { get; set; }
    }

    public class CompetitionSubscriptionResponse
    {
        public int CompetitionId { get; set; }
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public bool Success { get; set; }
    }
}
