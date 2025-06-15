using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class ChangeManualSuspensionResponseTests
    {
        [Fact]
        public void CanInstantiateChangeManualSuspensionResponse()
        {
            var response = new ChangeManualSuspensionResponse();
            Assert.NotNull(response);
        }
    }
} 