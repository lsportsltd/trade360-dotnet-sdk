using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface IPackageQueryHttpClient
    {
        Task<GetProviderOddsTypeResponse> GetProviderOddsType(CancellationToken cancellationToken);
    }
}