using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescores;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.SnapshotApi.Http;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi
{
    public class InplaySnapshotApiClient : BaseHttpClient, ISnapshotInplayApiClient
    {
        private readonly IMapper _mapper;
        public InplaySnapshotApiClient(HttpClient httpClient, SnapshotApiSettings settings, IMapper mapper)
            : base(httpClient, settings)
        {
            httpClient.BaseAddress = new System.Uri(settings.BaseUrl);
            _mapper = mapper;
        }


        public async Task<IEnumerable<FixtureEvent>> GetFixtures(GetFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<FixtureEvent>>(
                "Inplay/GetFixtures",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<FixtureEvent>();
        }

        public async Task<IEnumerable<GetLiveScoreResponse>> GetLivescore(GetLivescoreRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetLiveScoreResponse>>(
                "Inplay/GetScores",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<GetLiveScoreResponse>();
        }   
        
        public async Task<IEnumerable<MarketEvent>> GetFixtureMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<MarketEvent>>(
                "Inplay/GetFixtureMarkets",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<MarketEvent>();
        }

        public async Task<IEnumerable<GetEventsResponse>> GetEvents(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetEventsResponse>>(
                "Inplay/GetEvents",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<GetEventsResponse>();
        }
    }
}
