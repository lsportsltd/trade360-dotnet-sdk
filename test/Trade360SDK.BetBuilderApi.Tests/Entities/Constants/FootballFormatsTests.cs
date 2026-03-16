using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Constants;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Constants
{
    public class FootballFormatsTests
    {
        [Fact]
        public void RegularTime90_ShouldHaveCorrectValue()
        {
            FootballFormats.RegularTime90.Should().Be("football/RT:90");
        }

        [Fact]
        public void RegularTime90ExtraTime30Penalties_ShouldHaveCorrectValue()
        {
            FootballFormats.RegularTime90ExtraTime30Penalties.Should().Be("football/RT:90+ET:30+PS:ABAB");
        }
    }
}
