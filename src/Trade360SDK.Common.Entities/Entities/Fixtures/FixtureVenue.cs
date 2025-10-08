using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class FixtureVenue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public int? Attendance { get; set; }
        public CourtSurface? CourtSurfaceType { get; set; }
        public VenueEnvironment? Environment { get; set; }
        public VenueAssignment? Assignment { get; set; }
        public IdNamePair? Country { get; set; }
        public IdNamePair? State { get; set; }
        public IdNamePair? City { get; set; }
    }
}