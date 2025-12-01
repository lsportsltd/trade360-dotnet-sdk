using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class PackageQuotaResponseTests
    {
        [Fact]
        public void CanInstantiatePackageQuotaResponse()
        {
            var response = new PackageQuotaResponse();
            Assert.NotNull(response);
        }
    }
} 