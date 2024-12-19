using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.Base;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests
{
    public class ChangeManualSuspensionRequest : BaseRequest
    {
        public List<Suspension>? Suspensions { get; set; }
    }

    public class Suspension
    {
        public int? SportId { get; set; }
        public int? LocationId { get; set; }
        public int? CompetitionId { get; set; }
        public int? FixtureId { get; set; }
        public List<SuspendedMarket>? Markets { get; set; }
    }
}
