using System;
using AutoMapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.SnapshotApi.Http;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi
{
    public class SnapshotPrematchApiClient : BaseHttpClient, ISnapshotPrematchApiClient
    {
        private readonly IMapper _mapper;
        public SnapshotPrematchApiClient(IHttpClientFactory httpClientFactory, IOptions<Trade360Settings> settings, IMapper mapper)
            : base(httpClientFactory, settings.Value, settings.Value.PrematchPackageCredentials)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(settings.Value.SnapshotApiBaseUrl ?? throw new InvalidOperationException());
            _mapper = mapper;
        }

        public async Task<IEnumerable<FixtureEvent>> GetFixtures(GetFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<FixtureEvent>>(
                "Prematch/GetFixtures",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<LivescoreEvent>> GetLivescore(GetLivescoreRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<LivescoreEvent>>(
                "Prematch/GetScores",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<MarketEvent>> GetFixtureMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<MarketEvent>>(
                "Prematch/GetFixtureMarkets",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetEventsResponse>> GetEvents(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetEventsResponse>>(
                "Prematch/GetEvents",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetOutrightFixtureResponse>> GetOutrightFixture(GetOutrightFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseOutrightRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightFixtureResponse>>(
                "Prematch/GetOutrightFixture",
                request,
                cancellationToken);
            return response;
        }
        public async Task<IEnumerable<GetOutrightLivescoreResponse>> GetOutrightScores(GetOutrightLivescoreRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseOutrightRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLivescoreResponse>>(
                "Prematch/GetOutrightScores",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetOutrightMarketsResponse>> GetOutrightFixtureMarkets(GetOutrightMarketsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseOutrightRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightMarketsResponse>>(
                "Prematch/GetOutrightFixtureMarkets",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetOutrightEventsResponse>> GetOutrightEvents(GetOutrightMarketsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseOutrightRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightEventsResponse>>(
                "Prematch/GetOutrightEvents",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetOutrightLeaguesFixturesResponse>> GetOutrightLeagues(GetFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLeaguesFixturesResponse>>(
                "Prematch/GetOutrightLeagues",
                request,
                cancellationToken);
            return response;
        }


        public async Task<IEnumerable<GetOutrightLeaguesMarketsResponse>> GetOutrightLeaguesMarkets(GetMarketRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseStandardRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLeaguesMarketsResponse>>(
                "Prematch/GetOutrightLeagueMarkets",
                request,
                cancellationToken);
            return response;
        }

        public async Task<IEnumerable<GetOutrightLeagueEventsResponse>> GetOutrightLeagueEvents(
            GetOutrightFixturesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<BaseOutrightRequest>(requestDto);

            var response = await PostEntityAsync<IEnumerable<GetOutrightLeagueEventsResponse>>(
                "Prematch/GetOutrightLeagueEvents",
                request,
                cancellationToken);
            return response;
        }
    }
}
