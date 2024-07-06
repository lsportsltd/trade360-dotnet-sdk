using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Livescores;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(2)]
    public class LivescoreUpdate
    {
        public IEnumerable<LivescoreEvent>? Events { get; set; }
    }
}
