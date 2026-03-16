using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Constants;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Constants
{
    public class AmericanFootballFormatsTests
    {
        [Fact]
        public void NFLRegularSeason_ShouldHaveCorrectValue()
        {
            AmericanFootballFormats.NFLRegularSeason.Should().Be("american-football/RT:15{4}+OT:15*");
        }

        [Fact]
        public void NFLPlayoffs_ShouldHaveCorrectValue()
        {
            AmericanFootballFormats.NFLPlayoffs.Should().Be("american-football/RT:15{4}+OT:15*");
        }

        [Fact]
        public void NCAA_ShouldHaveCorrectValue()
        {
            AmericanFootballFormats.NCAA.Should().Be("american-football/ncaa/playoffs");
        }
    }
}
