using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Incidents;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using City = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.City;
using League = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.League;
using Location = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Location;
using Sport = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Sport;
using State = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.State;
using Venue = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Venue;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface IMetadataHttpClient
    {
        Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto request, CancellationToken cancellationToken);
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto request, CancellationToken cancellationToken);
        Task<TranslationResponse> GetTranslationsAsync(GetTranslationsRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionCollectionResponse> GetCompetitionsAsync(GetCompetitionsRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<Incident>> GetIncidentsAsync(GetIncidentsRequestDto requestDto, CancellationToken cancellationToken);

        Task<IEnumerable<Venue>> GetVenuesAsync(GetVenuesRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<City>> GetCitiesAsync(GetCitiesRequestDto requestDto, CancellationToken cancellationToken);
        Task<IEnumerable<State>> GetStatesAsync(GetStatesRequestDto requestDto, CancellationToken cancellationToken);
    }
}