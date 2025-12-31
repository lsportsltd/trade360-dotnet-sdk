using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class League
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        
        public IdNamePair Tour { get; set; }
        
        public int? AgeCategory { get; set; }
        
        public int? Gender { get; set; }
        
        public int? Type { get; set; }
        
        public int? NumberOfPeriods { get; set; }
        
        public IdNamePair SportCategory { get; set; }
    }
}

