using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string Name { get; set; }
        public IdNamePair? Country { get; set; }
        public IdNamePair? State { get; set; }
        public IdNamePair? City { get; set; }
    }
}