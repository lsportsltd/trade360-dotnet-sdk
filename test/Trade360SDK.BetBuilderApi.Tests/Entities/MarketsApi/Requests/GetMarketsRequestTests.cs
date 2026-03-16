using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.MarketsApi.Requests;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.MarketsApi.Requests
{
    public class GetMarketsRequestTests
    {
        [Fact]
        public void GetMarketsRequest_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var request = new GetMarketsRequest();

            request.Should().NotBeNull();
            request.CustomerId.Should().Be(0);
            request.UserId.Should().BeNull();
            request.SportId.Should().Be(0);
        }

        [Fact]
        public void GetMarketsRequest_SetProperties_ShouldReturnCorrectValues()
        {
            var request = new GetMarketsRequest
            {
                CustomerId = 1,
                UserId = "user",
                SportId = 6046
            };

            request.CustomerId.Should().Be(1);
            request.UserId.Should().Be("user");
            request.SportId.Should().Be(6046);
        }
    }
}
