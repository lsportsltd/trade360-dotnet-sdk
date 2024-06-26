using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Outright;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(37)]
    public class OutrightFixtureUpdate
    {
        public IEnumerable<OutrightCompetition<OutrightFixtureEvent>>? Competitions { get; set; }
    }
}
