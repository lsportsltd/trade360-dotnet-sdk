using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(35)]
    public class SettlementMessageUpdate : MessageUpdate
    {
        public IEnumerable<MarketEvent>? Events { get; set; }
    }
}
