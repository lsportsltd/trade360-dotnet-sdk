using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Outright;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(38)]
    public class OutrightLeagueUpdate
    {
        public IEnumerable<OutrightCompetition<OutrightLeagueEvent>>? Competitions { get; set; }
    }
}
