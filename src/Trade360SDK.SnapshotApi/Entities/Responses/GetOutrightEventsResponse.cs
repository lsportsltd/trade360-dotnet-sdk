using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightEventsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<OutrightEventResponse>? Events { get; set; }
    }

    public class OutrightEventResponse
    {
        public int FixtureId { get; set; }
        public OutrightFixtureSnapshotResponse? OutrightFixture { get; set; }
        public OutrightLivescoreScore? OutrightScore { get; set; }
        public IEnumerable<OutrightMarketResponse>? Markets { get; set; }
    }
}
