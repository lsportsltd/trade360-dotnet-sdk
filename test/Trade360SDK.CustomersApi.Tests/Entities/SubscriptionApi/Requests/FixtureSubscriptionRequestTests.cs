using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class FixtureSubscriptionRequestTests
    {
        [Fact]
        public void CanInstantiateFixtureSubscriptionRequest()
        {
            var request = new FixtureSubscriptionRequest();
            Assert.NotNull(request);
        }
    }
} 