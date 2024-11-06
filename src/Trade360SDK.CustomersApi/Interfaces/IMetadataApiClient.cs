using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface IMetadataApiClient
    {
        Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto request, CancellationToken cancellationToken);
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto request, CancellationToken cancellationToken);
        Task<TranslationResponse> GetTranslationsAsync(GetTranslationsRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionCollectionResponse> GetCompetitionsAsync(GetCompetitionsRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetFixtureMetadataCollectionResponse> GetFixtureMetadataAsync(GetFixtureMetadataRequestDto requestDto, CancellationToken cancellationToken);
    }
}