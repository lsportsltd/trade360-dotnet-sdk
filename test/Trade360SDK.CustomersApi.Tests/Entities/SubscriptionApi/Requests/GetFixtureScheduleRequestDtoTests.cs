using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class GetFixtureScheduleRequestDtoTests
    {
        [Fact]
        public void CanInstantiateGetFixtureScheduleRequestDto()
        {
            var dto = new GetFixtureScheduleRequestDto();
            Assert.NotNull(dto);
        }
    }
} 