using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Markets;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(3)]
    public class MarketUpdate
    {
        public IEnumerable<MarketEvent>? Events { get; set; }
    }
}
