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
    public class PrematchSnapshotApiClient : BaseHttpClient, ISnapshotPrematchApiClient
    {
        private readonly IMapper _mapper;
        public PrematchSnapshotApiClient(HttpClient httpClient, SnapshotApiSettings settings, IMapper mapper)
            : base(httpClient, settings)
        {
            httpClient.BaseAddress = new System.Uri(settings.BaseUrl);
            _mapper = mapper;
        }

        public async Task<IEnumerable<FixtureEvent>> GetFixtures(GetFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<FixtureEvent>>(
                "Prematch/GetFixtures",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<FixtureEvent>();
        }

        public async Task<IEnumerable<LivescoreEvent>> GetLivescore(GetLivescoreRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<LivescoreEvent>>(
                "Prematch/GetScores",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<LivescoreEvent>();
        }

        public async Task<IEnumerable<MarketEvent>> GetFixtureMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<MarketEvent>>(
                "Prematch/GetFixtureMarkets",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<MarketEvent>();
        }

        public async Task<IEnumerable<GetEventsResponse>> GetEvents(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetEventsResponse>>(
                "Prematch/GetEvents",
                request,
                cancellationToken);
            return response ?? Enumerable.Empty<GetEventsResponse>();
        }
    }
}
