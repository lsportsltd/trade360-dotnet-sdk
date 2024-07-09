using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;

namespace Trade360SDK.SnapshotApi.Interfaces
{
    public interface ISnapshotInplayApiClient
    {
        Task<IEnumerable<FixtureEvent>> GetFixtures(GetFixturesRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetLiveScoreResponse>> GetLivescore(GetLivescoreRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<MarketEvent>> GetFixtureMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetEventsResponse>> GetEvents(GetMarketRequestDto requestDto, CancellationToken cancellationToken);
    }
}