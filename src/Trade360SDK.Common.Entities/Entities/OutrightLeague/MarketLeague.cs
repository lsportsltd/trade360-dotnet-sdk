using System.Collections.Generic;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class MarketLeague
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<Bet>? Bets { get; set; }

        public IEnumerable<ProviderMarket>? ProviderMarkets { get; set; }

        public string? MainLine { get; set; }
    }
}
