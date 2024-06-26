using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Outright;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(39)]
    public class OutrightScoreUpdate
    {
        public IEnumerable<OutrightCompetition<OutrightScoreEvent>>? Competitions { get; set; }
    }
}
