using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class FixtureSubscriptionRequestDtoTests
    {
        [Fact]
        public void CanInstantiateFixtureSubscriptionRequestDto()
        {
            var dto = new FixtureSubscriptionRequestDto();
            Assert.NotNull(dto);
        }
    }
} 