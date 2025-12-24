using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class FixturePlayer
    {
        public int? PlayerId { get; set; }
        public string? ShirtNumber { get; set; }
        public bool? IsCaptain { get; set; }
        public bool? IsStartingLineup { get; set; }
        public IdNamePair Position { get; set; }
        public IdNamePair State { get; set; }
        public Player Player { get; set; }
    }
}