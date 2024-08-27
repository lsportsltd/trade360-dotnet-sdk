using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Livescore;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(2)]
    public class LivescoreMessageUpdate : MessageUpdate
    {
        public IEnumerable<LivescoreEvent>? Events { get; set; }
    }
}
