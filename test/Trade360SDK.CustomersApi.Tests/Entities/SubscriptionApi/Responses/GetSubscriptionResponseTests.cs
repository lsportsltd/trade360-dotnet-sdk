using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class GetSubscriptionResponseTests
    {
        [Fact]
        public void CanInstantiateGetSubscriptionResponse()
        {
            var response = new GetSubscriptionResponse();
            Assert.NotNull(response);
        }
    }
} 