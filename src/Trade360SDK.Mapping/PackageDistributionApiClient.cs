using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Api.Abstraction;
using Trade360SDK.Api.Abstraction.Interfaces;
using Trade360SDK.Api.Abstraction.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi
{
    public class PackageDistributionApiClient : BaseHttpClient, IPackageDistributionApiClient
    {
        public PackageDistributionApiClient(HttpClient httpClient, CustomersApiSettings options)
            : base(httpClient, options)
        {
            httpClient.BaseAddress = new System.Uri(options.BaseUrl);
        }

        public async Task<GetDistributionStatusResponse> GetDistributionStatusAsync(CancellationToken cancellationToken)
        {
            var response = await GetEntityAsync<GetDistributionStatusResponse>(
               "package/GetDistributionStatus",
               new BaseRequest(),
               cancellationToken);
            return response;
        }

        public async Task<StartDistributionResponse> StartDistributionAsync(CancellationToken cancellationToken)
        {
            var response = await GetEntityAsync<StartDistributionResponse>(
                "distribution/start",
                new BaseRequest(),
                cancellationToken);
            return response;
        }

        public async Task<StopDistributionResponse> StopDistributionAsync(CancellationToken cancellationToken)
        {
            var response = await GetEntityAsync<StopDistributionResponse>(
                "distribution/stop",
                new BaseRequest(),
                cancellationToken);
            return response;
        }

    }
}
