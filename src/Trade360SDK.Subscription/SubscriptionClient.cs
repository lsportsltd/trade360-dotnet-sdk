using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common;
using Trade360SDK.Common.Models;
using Trade360SDK.Subscription.Entities;

namespace Trade360SDK.Subscription
{
    public class SubscriptionClient : BaseHttpClient
    {
        public SubscriptionClient(string customerApi, int packageId, string username, string password)
            : base(customerApi, packageId, username, password)
        {
        }

        public SubscriptionClient(HttpClient httpClient, int packageId, string username, string password)
            : base(httpClient, packageId, username, password)
        {
        }

        public Task<PackageQuota> GetPackageQuotaAsync(CancellationToken cancelationToken)
            => GetEntityAsync<PackageQuota>("/package/GetPackageQuota", new Request(), cancelationToken);
    }
}
