using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class ChangeManualSuspensionRequestDtoTests
    {
        [Fact]
        public void CanInstantiateChangeManualSuspensionRequestDto()
        {
            var dto = new ChangeManualSuspensionRequestDto();
            Assert.NotNull(dto);
        }
    }
} 