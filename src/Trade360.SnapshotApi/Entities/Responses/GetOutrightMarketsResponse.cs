using System.Collections.Generic;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightMarketsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<OutrightMarketsResponse>? Events { get; set; }
    }

    public class OutrightMarketsResponse
    {
        public int FixtureId { get; set; }
        public IEnumerable<OutrightMarketResponse>? Markets { get; set; }
    }

    public class OutrightMarketResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Bet>? Bets { get; set; }
    }

}
