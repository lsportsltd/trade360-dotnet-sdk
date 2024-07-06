using System;
using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class GetManualSuspensionResponse
    {
        public bool Succeeded { get; set; }
        public List<Suspension> Suspensions { get; set; }
    }

    public class Suspension
    {
        public bool Succeeded { get; set; }
        public int SportId { get; set; }
        public int? LocationId { get; set; } // Nullable to handle missing values
        public int CompetitionId { get; set; }
        public int? FixtureId { get; set; } // Nullable to handle missing values
        public DateTime CreationDate { get; set; }
        public List<Market> Markets { get; set; }
    }

    public class Market
    {
        public int MarketId { get; set; }
        public string Line { get; set; } // Nullable to handle missing values
    }
}
