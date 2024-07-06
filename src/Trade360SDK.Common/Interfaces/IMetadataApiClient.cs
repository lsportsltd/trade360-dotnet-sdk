using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Api.Abstraction.MetadataApi.Requests;
using Trade360SDK.Api.Abstraction.MetadataApi.Responses;
using Trade360SDK.Common.Metadata.Requests;
using Trade360SDK.Common.Metadata.Responses;
using Trade360SDK.CustomersApi.MetadataApi.Requests;

namespace Trade360SDK.Metadata
{
    public interface IMetadataApiClient
    {
        Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto request, CancellationToken cancellationToken);
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto request, CancellationToken cancellationToken);
        Task<TransactionResponse> GetTranslationsAsync(GetTranslationsRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionCollectionResponse> GetCompetitionsAync(GetCompetitionsRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetFixtureMetadataCollectionResponse> GetFixtureMetadataAsync(GetFixtureMetadataRequestDto requestDto, CancellationToken cancellationToken);
    }
}