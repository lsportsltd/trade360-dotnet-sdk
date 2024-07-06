using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Outright;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(40)]
    public class OutrightLeagueMarketUpdate
    {
        public IEnumerable<OutrightCompetition<MarketEvent>>? Competitions { get; set; }
    }
}
