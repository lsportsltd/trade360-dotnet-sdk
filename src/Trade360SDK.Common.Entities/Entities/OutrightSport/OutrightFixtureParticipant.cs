using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;
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
        
        public string Form { get; set; }
        
        public string Formation { get; set; }
        
        public List<FixturePlayer> FixturePlayers { get; set; }
        
        public int? Gender { get; set; }
        
        public int? AgeCategory { get; set; }
        
        public int? Type { get; set; }
        
        public IEnumerable<NameValuePair>? ExtraData { get; set; }
    }
}
