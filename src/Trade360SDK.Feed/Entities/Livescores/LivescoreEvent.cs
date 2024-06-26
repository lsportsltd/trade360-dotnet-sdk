using Trade360SDK.Feed.Entities.Fixtures;

namespace Trade360SDK.Feed.Entities.Livescores
{
    public class LivescoreEvent
    {
        public int FixtureId { get; set; }
        public Fixture? Fixture { get; set; }
        public Livescore? Livescore { get; set; }
    }
}
