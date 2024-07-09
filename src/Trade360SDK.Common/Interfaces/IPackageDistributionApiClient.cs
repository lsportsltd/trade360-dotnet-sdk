using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Api.Abstraction.MetadataApi.Responses;

namespace Trade360SDK.Api.Abstraction.Interfaces
{
    public interface IPackageDistributionApiClient
    {
        Task<StartDistributionResponse> StartDistributionAsync(CancellationToken cancellationToken);
        Task<StopDistributionResponse> StopDistributionAsync(CancellationToken cancellationToken);
        Task<GetDistributionStatusResponse> GetDistributionStatusAsync(CancellationToken cancellationToken);
    }
}