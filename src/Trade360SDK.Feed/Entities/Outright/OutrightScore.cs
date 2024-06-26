using System.Collections.Generic;
using Trade360SDK.Feed.Entities.Enums;

namespace Trade360SDK.Feed.Entities.Outright
{
    public class OutrightScore
    {
        public IEnumerable<OutrightParticipant>? ParticipantResults { get; set; }

        public FixtureStatus Status { get; set; }
    }
}
