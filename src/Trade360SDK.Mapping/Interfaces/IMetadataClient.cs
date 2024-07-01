using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Metadata.Responses;
using Trade360SDK.CustomersApi.MetadataApi.Requests;

namespace Trade360SDK.Metadata
{
    public interface IMetadataClient
    {
        Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto request, CancellationToken cancellationToken);
        Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto request, CancellationToken cancellationToken);
    }
}