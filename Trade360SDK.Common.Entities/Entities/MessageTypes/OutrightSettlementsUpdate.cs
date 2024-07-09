using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Outright;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(42)]
    public class OutrightSettlementsUpdate
    {
        public IEnumerable<OutrightCompetition<MarketEvent>>? Competitions { get; set; }
    }
}
