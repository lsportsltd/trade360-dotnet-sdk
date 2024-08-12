using System.Collections.Generic;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.OutrightSport
{
    public class OutrightFixtureParticipant
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public int? RotationId { get; set; }

        public int IsActive { get; set; } = -1;
        public IEnumerable<NameValuePair>? ExtraData { get; set; }
    }
}
