using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Trade360SDK.Api.Abstraction;
using Trade360SDK.Api.Abstraction.Interfaces;
using Trade360SDK.Api.Abstraction.MetadataApi.Responses;
using Trade360SDK.Api.Abstraction.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi
{
    public class MetadataApiClient : BaseHttpClient, IMetadataApiClient
    {
        private readonly IMapper _mapper;
        public MetadataApiClient(HttpClient httpClient, CustomersApiSettings options, IMapper mapper)
            : base(httpClient, options)
        {
            httpClient.BaseAddress = new System.Uri(options.BaseUrl);
            _mapper = mapper;
        }

        public async Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken)
        {
            var sportsCollection = await PostEntityAsync<SportsCollectionResponse>(
                "sports/get",
                new BaseRequest(),
                cancellationToken);
            return sportsCollection.Sports ?? Enumerable.Empty<Sport>();
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken)
        {
            var locationsCollection = await PostEntityAsync<LocationsCollection>(
                "locations/get",
                new BaseRequest(),
                cancellationToken);
            return locationsCollection.Locations ?? Enumerable.Empty<Location>();
        }

        public async Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetLeaguesRequest>(requestDto);
            var leaguesCollection = await PostEntityAsync<LeaguesCollectionResponse>(
                "leagues/get",
                request,
                cancellationToken);
            return leaguesCollection.Leagues ?? Enumerable.Empty<League>();
        }

        public async Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetMarketsRequest>(requestDto);
            var leaguesCollection = await PostEntityAsync<MarketsCollectionResponse>(
                "markets/get",
                request,
                cancellationToken);
            return leaguesCollection.Markets ?? Enumerable.Empty<Market>();
        }

        public async Task<TransactionResponse> GetTranslationsAsync(GetTranslationsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetTranslationsRequest>(requestDto);

            GetTranslationsRequestValidator.Validate(request);
            var response = await PostEntityAsync<TransactionResponse>(
                "Translation/Get",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionCollectionResponse> GetCompetitionsAsync(GetCompetitionsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetCompetitionsRequest>(requestDto);

            var response = await PostEntityAsync<CompetitionCollectionResponse>(
                "Outright/GetCompetitions",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetFixtureMetadataCollectionResponse> GetFixtureMetadataAsync(GetFixtureMetadataRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetFixtureMetadataRequest>(requestDto);

            var response = await GetEntityAsync<GetFixtureMetadataCollectionResponse>(
                "Fixtures/GetSubscribedMetaData",
                request,
                cancellationToken);
            return response;
        }
    }
}
