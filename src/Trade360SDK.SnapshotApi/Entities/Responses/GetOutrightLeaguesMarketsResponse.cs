using System.Collections.Generic;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightLeaguesMarketsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<SnapshotOutrightEventsResponse>? Competitions { get; set; }
    }

    public class SnapshotOutrightEventsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<OutrightMarketsResponse>? Events { get; set; }
    }
}
