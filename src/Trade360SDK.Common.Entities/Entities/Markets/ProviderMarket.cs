using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Markets
{
    public class ProviderMarket
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<ProviderBet>? Bets { get; set; }

        public DateTime LastUpdate { get; set; }

        public MarketStatus MarketStatus { get; set; }

        public PredictionData? PredictionData { get; set; }
    }
}