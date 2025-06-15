using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class GetSubscriptionRequestTests
    {
        [Fact]
        public void CanInstantiateGetSubscriptionRequest()
        {
            var request = new GetSubscriptionRequest();
            Assert.NotNull(request);
        }
    }
} 