using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class FixtureSubscriptionCollectionResponseTests
    {
        [Fact]
        public void CanInstantiateFixtureSubscriptionCollectionResponse()
        {
            var response = new FixtureSubscriptionCollectionResponse();
            Assert.NotNull(response);
        }
    }
} 