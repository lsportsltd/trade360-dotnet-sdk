using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Outright;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(38)]
    public class OutrightLeagueUpdate
    {
        public IEnumerable<OutrightCompetition<OutrightLeagueEvent>>? Competitions { get; set; }
    }
}
