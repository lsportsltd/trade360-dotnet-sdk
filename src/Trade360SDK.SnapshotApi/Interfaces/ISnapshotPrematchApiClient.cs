using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;

namespace Trade360SDK.SnapshotApi.Interfaces
{
    public interface ISnapshotPrematchApiClient
    {
        Task<IEnumerable<FixtureEvent>> GetFixtures(GetFixturesRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<LivescoreEvent>> GetLivescore(GetLivescoreRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<MarketEvent>> GetFixtureMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetEventsResponse>> GetEvents(GetMarketRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightFixtureResponse>> GetOutrightFixture(GetOutrightFixturesRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightLivescoreResponse>> GetOutrightScores(GetOutrightLivescoreRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightMarketsResponse>> GetOutrightFixtureMarkets(GetOutrightMarketsRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightEventsResponse>> GetOutrightEvents(GetOutrightMarketsRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightLeaguesFixturesResponse>> GetOutrightLeagues(GetFixturesRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightLeaguesMarketsResponse>> GetOutrightLeaguesMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<GetOutrightLeagueEventsResponse>> GetOutrightLeagueEvents(GetOutrightFixturesRequestDto requestDto, CancellationToken cancellationToken);
    }
}