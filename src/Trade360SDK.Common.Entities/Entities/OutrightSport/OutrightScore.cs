using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.OutrightSport
{
    public class OutrightScore
    {
        public IEnumerable<OutrightParticipant>? ParticipantResults { get; set; }

        public FixtureStatus Status { get; set; }
    }
}
