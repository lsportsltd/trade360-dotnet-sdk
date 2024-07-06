using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class CompetitionSubcsription
    {
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int CompetitionId { get; set; }
    }

    public class CompetitionSubscriptionRequest : BaseRequest
    {
        public List<CompetitionSubcsription> Subscriptions { get; set; }
    }
}
