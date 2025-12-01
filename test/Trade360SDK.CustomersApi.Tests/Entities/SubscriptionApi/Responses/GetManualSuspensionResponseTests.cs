using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class GetManualSuspensionResponseTests
    {
        [Fact]
        public void CanInstantiateGetManualSuspensionResponse()
        {
            var response = new GetManualSuspensionResponse();
            Assert.NotNull(response);
        }
    }
} 