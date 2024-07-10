using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Outright;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(39)]
    public class OutrightScoreUpdate
    {
        public IEnumerable<OutrightCompetition<OutrightScoreEvent>>? Competitions { get; set; }
    }
}
