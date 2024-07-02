using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common;
using Microsoft.Extensions.Options;
using Trade360SDK.Api.Common.Models.Requests.Base;
using Trade360SDK.Metadata;
using AutoMapper;
using Trade360SDK.Common.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.PackageDistributionClient
{
    public class PackageDistributionClient : BaseHttpClient, IPackageDistributionClient
    {
        private readonly IMapper _mapper;
        public PackageDistributionClient(HttpClient httpClient, IOptions<Trade360ApiSettings> options, IMapper mapper)
            : base(httpClient, options.Value)
        {
            httpClient.BaseAddress = new System.Uri(options.Value.BaseUrl);
            _mapper = mapper;
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
