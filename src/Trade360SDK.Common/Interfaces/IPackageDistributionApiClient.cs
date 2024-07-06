using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.MetadataApi.Responses;

namespace Trade360SDK.Metadata
{
    public interface IPackageDistributionApiClient
    {
        Task<StartDistributionResponse> StartDistributionAsync(CancellationToken cancellationToken);
        Task<StopDistributionResponse> StopDistributionAsync(CancellationToken cancellationToken);
        Task<GetDistributionStatusResponse> GetDistributionStatusAsync(CancellationToken cancellationToken);
    }
}