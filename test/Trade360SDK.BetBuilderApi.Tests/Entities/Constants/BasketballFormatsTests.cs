using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Constants;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Constants
{
    public class BasketballFormatsTests
    {
        [Fact]
        public void NBA_ShouldHaveCorrectValue()
        {
            BasketballFormats.NBA.Should().Be("basketball/RT:12{4}+OT:5*");
        }

        [Fact]
        public void NCAA_ShouldHaveCorrectValue()
        {
            BasketballFormats.NCAA.Should().Be("basketball/RT:20{2}+OT:5*");
        }

        [Fact]
        public void WNBA_ShouldHaveCorrectValue()
        {
            BasketballFormats.WNBA.Should().Be("basketball/RT:10{4}+OT:5*");
        }

        [Fact]
        public void Euroleague_ShouldHaveCorrectValue()
        {
            BasketballFormats.Euroleague.Should().Be("basketball/RT:10{4}+OT:5*");
        }
    }
}
