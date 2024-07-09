using Trade360SDK.Common.Entities.Livescore;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetLiveScoreResponse
    {
        public int FixtureId { get; set; }
        public Livescore? Livescore { get; set; }
    }
}
