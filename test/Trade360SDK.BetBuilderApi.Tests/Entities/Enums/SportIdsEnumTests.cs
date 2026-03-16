using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Enums;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Enums
{
    public class SportIdsEnumTests
    {
        [Fact]
        public void SportIdsEnum_Football_ShouldHaveCorrectValue()
        {
            ((int)SportIdsEnum.Football).Should().Be(6046);
        }

        [Fact]
        public void SportIdsEnum_Basketball_ShouldHaveCorrectValue()
        {
            ((int)SportIdsEnum.Basketball).Should().Be(48242);
        }

        [Fact]
        public void SportIdsEnum_AmericanFootball_ShouldHaveCorrectValue()
        {
            ((int)SportIdsEnum.AmericanFootball).Should().Be(131506);
        }
    }
}
