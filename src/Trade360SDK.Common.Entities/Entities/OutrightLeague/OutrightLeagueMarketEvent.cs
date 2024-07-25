using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueMarketEvent
    {
        public int FixtureId { get; set; }
        public IEnumerable<MarketLeague>? Markets { get; set; }
    }
}
