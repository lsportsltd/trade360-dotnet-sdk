using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(3)]
    public class MarketMessageUpdate : MessageUpdate
    {
        public IEnumerable<MarketEvent>? Events { get; set; }
    }
}
