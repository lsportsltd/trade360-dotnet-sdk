using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Outright;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(37)]
    public class OutrightFixtureUpdate
    {
        public IEnumerable<OutrightCompetition<OutrightFixtureEvent>>? Competitions { get; set; }
    }
}
