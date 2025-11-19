using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.SnapshotApi.Http;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi
{
    public class SnapshotInplayApiClient : BaseHttpClient, ISnapshotInplayApiClient
    {
        private readonly IMapper _mapper;

        public SnapshotInplayApiClient(IHttpClientFactory httpClientFactory, IOptions<Trade360Settings> settings,
            IMapper mapper)
            : base(httpClientFactory, settings.Value, settings.Value.InplayPackageCredentials)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(settings.Value.SnapshotApiBaseUrl ?? throw new InvalidOperationException());
            _mapper = mapper;
        }


        public async Task<IEnumerable<FixtureEvent>> GetFixtures(GetFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<FixtureEvent>>(
                "Inplay/GetFixtures",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetLiveScoreResponse>> GetLivescore(GetLivescoreRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetLiveScoreResponse>>(
                "Inplay/GetScores",
                request,
                cancellationToken);
            return response;
        }   
        
        public async Task<IEnumerable<MarketEvent>> GetFixtureMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<MarketEvent>>(
                "Inplay/GetFixtureMarkets",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetEventsResponse>> GetEvents(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetEventsResponse>>(
                "Inplay/GetEvents",
                request,
                cancellationToken);
            return response;
        }
        
        public async Task<IEnumerable<GetOutrightLeaguesFixturesResponse>> GetOutrightLeagues(GetFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLeaguesFixturesResponse>>(
                "Inplay/GetOutrightLeagues",
                request,
                cancellationToken);
            return response;
        }


        public async Task<IEnumerable<GetOutrightLeaguesMarketsResponse>> GetOutrightLeaguesMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLeaguesMarketsResponse>>(
                "Inplay/GetOutrightLeagueMarkets",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetOutrightLeagueEventsResponse>> GetOutrightLeagueEvents(
            GetOutrightFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseOutrightRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLeagueEventsResponse>>(
                "Inplay/GetOutrightLeagueEvents",
                request,
                cancellationToken);
            return response;
        }
    }
}
