using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Api.Common.Models.Requests.Base;
using Trade360SDK.Common;
using Trade360SDK.Common.Metadata.Responses;

namespace Trade360SDK.Api.Subscription
{
    public class SubscriptionClient : BaseHttpClient
    {
        public SubscriptionClient(HttpClient httpClient, Trade360ApiSettings settings)
            : base(httpClient, settings)
        {
        }

        public Task<PackageQuotaResponse> GetPackageQuotaAsync(CancellationToken cancelationToken)
            => GetEntityAsync<PackageQuotaResponse>("/package/GetPackageQuota", new BaseRequest(), cancelationToken);
    }
}
