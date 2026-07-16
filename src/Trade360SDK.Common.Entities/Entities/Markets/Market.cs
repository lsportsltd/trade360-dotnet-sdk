using System.Collections.Generic;
using System.Text.Json.Serialization;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Markets
{
    public class Market
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<Bet>? Bets { get; set; }
        
        public IEnumerable<ProviderMarket>? ProviderMarkets { get; set; }

        public string? MainLine { get; set; }

        [JsonPropertyName("Status")]
        public MarketStatus Status { get; set; }

        public MarketPredictionData? PredictionData { get; set; }
    }
}
