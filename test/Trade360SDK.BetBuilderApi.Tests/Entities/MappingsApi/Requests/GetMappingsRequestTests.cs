using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.MappingsApi.Requests;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.MappingsApi.Requests
{
    public class GetMappingsRequestTests
    {
        [Fact]
        public void GetMappingsRequest_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var request = new GetMappingsRequest();

            request.Should().NotBeNull();
            request.SportId.Should().Be(0);
            request.MarketId.Should().BeNull();
            request.Version.Should().BeNull();
        }

        [Fact]
        public void GetMappingsRequest_SetProperties_ShouldReturnCorrectValues()
        {
            var request = new GetMappingsRequest
            {
                SportId = 6046,
                MarketId = 1,
                Version = "1.0"
            };

            request.SportId.Should().Be(6046);
            request.MarketId.Should().Be(1);
            request.Version.Should().Be("1.0");
        }
    }
}
