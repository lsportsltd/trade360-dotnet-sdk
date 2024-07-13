using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.CustomersApi.Entities.Base;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;

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
            var response = await PostEntityAsync<GetDistributionStatusResponse>(
               "package/GetDistributionStatus",
               new BaseRequest(),
               cancellationToken);
            return response;
        }

        public async Task<StartDistributionResponse> StartDistributionAsync(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<StartDistributionResponse>(
                "distribution/start",
                new BaseRequest(),
                cancellationToken);
            return response;
        }

        public async Task<StopDistributionResponse> StopDistributionAsync(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<StopDistributionResponse>(
                "distribution/stop",
                new BaseRequest(),
                cancellationToken);
            return response;
        }

    }
}
