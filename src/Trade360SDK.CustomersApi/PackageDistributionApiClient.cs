using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi
{
    public class PackageDistributionApiClient : BaseHttpClient, IPackageDistributionApiClient
    {
        public PackageDistributionApiClient(IHttpClientFactory httpClientFactory, string? baseUrl, PackageCredentials? packageCredentials)
            : base(httpClientFactory, baseUrl, packageCredentials)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new System.Uri(baseUrl ?? throw new ArgumentNullException(nameof(baseUrl)));
        }

        public async Task<GetDistributionStatusResponse> GetDistributionStatusAsync(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<GetDistributionStatusResponse>(
               "Package/GetDistributionStatus",
               cancellationToken);
            return response;
        }

        public async Task<StartDistributionResponse> StartDistributionAsync(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<StartDistributionResponse>(
                "Distribution/Start",
                cancellationToken);
            return response;
        }

        public async Task<StopDistributionResponse> StopDistributionAsync(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<StopDistributionResponse>(
                "Distribution/Stop",
                cancellationToken);
            return response;
        }

    }
}
