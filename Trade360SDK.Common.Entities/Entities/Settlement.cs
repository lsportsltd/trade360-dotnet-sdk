using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(35)]
    public class Settlement
    {
        public IEnumerable<MarketEvent>? Events { get; set; }
    }
}
