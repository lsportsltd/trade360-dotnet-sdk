using System.Collections.Generic;
using Trade360SDK.Common.Entities.Outright;

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
        public OutrightScore? OutrightScore { get; set; }
        public IEnumerable<OutrightMarketResponse>? Markets { get; set; }
    }
}
