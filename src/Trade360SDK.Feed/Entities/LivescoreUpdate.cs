using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Livescores;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(2)]
    public class LivescoreUpdate
    {
        public IEnumerable<LivescoreEvent>? Events { get; set; }
    }
}
