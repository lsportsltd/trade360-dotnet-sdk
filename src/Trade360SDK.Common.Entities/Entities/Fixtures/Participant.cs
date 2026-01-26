using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class Participant
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public int? RotationId { get; set; }

        public int IsActive { get; set; } = -1;
        
        public string? Form { get; set; }
        
        public string? Formation { get; set; }
        
        public List<FixturePlayer>? FixturePlayers { get; set; }
        
        public int? Gender { get; set; }
        
        public int? AgeCategory { get; set; }
        
        public int? Type { get; set; }
    }
}
