using System;
using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses
{
    public class GetManualSuspensionResponse
    {
        public bool Succeeded { get; set; }
        public List<Suspension>? Suspensions { get; set; }
        public string? Reason { get; set; }
    }

    public class Suspension
    {
        public bool Succeeded { get; set; }
        public int SportId { get; set; }
        public int? LocationId { get; set; } // Nullable to handle missing values
        public int CompetitionId { get; set; }
        public int? FixtureId { get; set; } // Nullable to handle missing values
        public DateTime CreationDate { get; set; }
        public List<SuspendedMarket>? Markets { get; set; }
        public string? Reason { get; set; }
    }

    public class SuspendedMarket
    {
        public int MarketId { get; set; }
        public string? Line { get; set; } // Nullable to handle missing values
    }
}
