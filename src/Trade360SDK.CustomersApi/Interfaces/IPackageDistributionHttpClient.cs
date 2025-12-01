using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface IPackageDistributionHttpClient
    {
        Task<StartDistributionResponse> StartDistributionAsync(CancellationToken cancellationToken);
        Task<StopDistributionResponse> StopDistributionAsync(CancellationToken cancellationToken);
        Task<GetDistributionStatusResponse> GetDistributionStatusAsync(CancellationToken cancellationToken);
    }
}