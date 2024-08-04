using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetEventsResponse
    {
        public Fixture? Fixture { get; set; }
        public Livescore? Livescore { get; set; }
        public IEnumerable<Market>? Markets { get; set; }
        public int FixtureId { get; set; }
    }
}
