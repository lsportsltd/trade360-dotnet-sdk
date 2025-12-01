using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class ChangeManualSuspensionRequestTests
    {
        [Fact]
        public void CanInstantiateChangeManualSuspensionRequest()
        {
            var request = new ChangeManualSuspensionRequest();
            Assert.NotNull(request);
        }
    }
} 