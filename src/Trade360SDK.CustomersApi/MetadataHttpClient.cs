using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Incidents;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Validators;
using City = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.City;
using League = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.League;
using Location = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Location;
using ParticipantInfo = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.ParticipantInfo;
using Sport = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Sport;
using State = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.State;
using Venue = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Venue;

namespace Trade360SDK.CustomersApi
{
    public class MetadataHttpClient : BaseHttpClient, IMetadataHttpClient
    {
        private readonly IMapper _mapper;
        public MetadataHttpClient(IHttpClientFactory httpClientFactory, string? baseUrl, PackageCredentials? packageCredentials, IMapper mapper)
            : base(httpClientFactory, baseUrl, packageCredentials)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken)
        {
            var sportsCollection = await PostEntityAsync<SportsCollectionResponse>(
                "Sports/Get",
                cancellationToken);
            return sportsCollection.Sports ?? Enumerable.Empty<Sport>();
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken)
        {
            var locationsCollection = await PostEntityAsync<LocationsCollectionResponse>(
                "Locations/Get",
                cancellationToken);
            return locationsCollection.Locations ?? Enumerable.Empty<Location>();
        }

        public async Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetLeaguesRequest>(requestDto);
            var leaguesCollection = await PostEntityAsync<LeaguesCollectionResponse>(
                "Leagues/get",
                request,
                cancellationToken);
            return leaguesCollection.Leagues ?? Enumerable.Empty<League>();
        }

        public async Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetMarketsRequest>(requestDto);
            var leaguesCollection = await PostEntityAsync<MarketsCollectionResponse>(
                "Markets/get",
                request,
                cancellationToken);
            return leaguesCollection.Markets ?? Enumerable.Empty<Market>();
        }

        public async Task<TranslationResponse> GetTranslationsAsync(GetTranslationsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetTranslationsRequest>(requestDto);

            GetTranslationsRequestValidator.Validate(request);
            var response = await PostEntityAsync<TranslationResponse>(
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

        public async Task<IEnumerable<Incident>> GetIncidentsAsync(GetIncidentsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetIncidentsRequest>(requestDto);
            var response = await PostEntityAsync<GetIncidentsResponse>("Incidents/Get", request, cancellationToken);
            return response.Data ?? Enumerable.Empty<Incident>();
        }

        public async Task<IEnumerable<Venue>> GetVenuesAsync(GetVenuesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetVenuesRequest>(requestDto);
            var response = await PostEntityAsync<GetVenuesResponse>("Venues/Get", request, cancellationToken);
            return response.Data ?? Enumerable.Empty<Venue>();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(GetCitiesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetCitiesRequest>(requestDto);
            var response = await PostEntityAsync<GetCitiesResponse>("Cities/Get", request, cancellationToken);
            return response.Data ?? Enumerable.Empty<City>();
        }

        public async Task<IEnumerable<State>> GetStatesAsync(GetStatesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetStatesRequest>(requestDto);
            var response = await PostEntityAsync<GetStatesResponse>("States/Get", request, cancellationToken);
            return response.Data ?? Enumerable.Empty<State>();
        }

        public async Task<IEnumerable<ParticipantInfo>> GetParticipantsAsync(GetParticipantsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetParticipantsRequest>(requestDto);
            var response = await PostEntityAsync<GetParticipantsResponse>("Participants/Get", request, cancellationToken);
            return response.Data ?? Enumerable.Empty<ParticipantInfo>();
        }
    }
}
