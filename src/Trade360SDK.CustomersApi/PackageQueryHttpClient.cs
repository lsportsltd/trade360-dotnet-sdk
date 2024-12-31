using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi
{
    public class PackageQueryHttpClient: BaseHttpClient, IPackageQueryHttpClient
    {
        private readonly IMapper _mapper;

        public PackageQueryHttpClient(IHttpClientFactory httpClientFactory, string? baseUrl, PackageCredentials? packageCredentials, IMapper mapper)
            : base(httpClientFactory, baseUrl, packageCredentials)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl ?? throw new ArgumentNullException(nameof(baseUrl)));
            _mapper = mapper;
        }
        
        public async Task<GetProviderOddsTypeResponse> GetProviderOddsType(CancellationToken cancellationToken)
        {
            var response = await GetEntityAsync<GetProviderOddsTypeResponse>(
                "Package/GetProviderOddsType",
                cancellationToken);
            return response;
        }
    }
}