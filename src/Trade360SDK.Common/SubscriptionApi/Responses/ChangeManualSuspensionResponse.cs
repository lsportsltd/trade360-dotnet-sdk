using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class ChangeManualSuspensionResponse
    {
        public List<SuspensionChangeResponse> Suspensions { get; set; }
    }

    public class SuspensionChangeResponse
    {
        public bool Succeeded { get; set; }
        public string Reason { get; set; }
        public int? FixtureId { get; set; }
        public List<SuspendedMarket> Markets { get; set; }
    }

    public class SuspendedMarket
    {
        public int MarketId { get; set; }
        public string Line { get; set; }
    }
}
