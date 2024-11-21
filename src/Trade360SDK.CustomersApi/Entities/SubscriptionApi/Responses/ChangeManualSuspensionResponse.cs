using System;
using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses
{
    public class ChangeManualSuspensionResponse
    {
        public List<SuspensionChangeResponse>? Suspensions { get; set; }
    }

    public class SuspensionChangeResponse
    {
        public bool Succeeded { get; set; }
        public string? Reason { get; set; }
        public int? FixtureId { get; set; }
        public List<SuspendedMarket>? Markets { get; set; }
        public DateTime? CreationDate { get; set; }
    }

}
