using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.MarketsApi.Responses
{
    public class Market
    {
        public SportLookUp? SportLookUp { get; set; }

        public MarketLookUp? MarketLookUp { get; set; }

        public IEnumerable<MarketLookUp>? DependentMarketLookUps { get; set; }
    }
}
