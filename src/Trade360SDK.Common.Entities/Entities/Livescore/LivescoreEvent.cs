using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class LivescoreEvent
    {
        public int FixtureId { get; set; }
        public Fixture? Fixture { get; set; }
        public Livescore? Livescore { get; set; }
    }
}
