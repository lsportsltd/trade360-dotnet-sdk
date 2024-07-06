using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class ChangeManualSuspensionRequest : BaseRequest
    {
        public List<Suspension> Suspensions { get; set; }
    }

    public class Suspension
    {
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int CompetitionId { get; set; }
        public int FixtureId { get; set; }
        public List<Market> Markets { get; set; }
    }

    public class Market
    {
        public int MarketId { get; set; }
        public string Line { get; set; }
    }
}
