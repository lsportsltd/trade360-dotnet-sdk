using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class GetSubscriptionRequestDtoTests
    {
        [Fact]
        public void CanInstantiateGetSubscriptionRequestDto()
        {
            var dto = new GetSubscriptionRequestDto();
            Assert.NotNull(dto);
        }
    }
} 