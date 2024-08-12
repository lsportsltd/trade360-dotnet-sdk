using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightLivescoreResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<OutrightScoreEventResponse>? Events { get; set; }
    }

    public class OutrightScoreEventResponse
    {
        public int FixtureId { get; set; }
        public OutrightLivescoreScore? OutrightScore { get; set; }
    }
}
