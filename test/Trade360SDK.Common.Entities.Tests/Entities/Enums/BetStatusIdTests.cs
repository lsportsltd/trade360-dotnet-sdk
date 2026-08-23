using Trade360SDK.Common.Entities.Enums;
using Xunit;
using FluentAssertions;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class BetStatusIdTests
    {
        [Fact]
        public void BetStatusId_ShouldHaveValuesOneThroughFour()
        {
            ((int)BetStatusId.Open).Should().Be(1);
            ((int)BetStatusId.Suspended).Should().Be(2);
            ((int)BetStatusId.Settled).Should().Be(3);
            ((int)BetStatusId.Closed).Should().Be(4);
            System.Enum.IsDefined(typeof(BetStatusId), 0).Should().BeFalse();
            ((int)BetStatusId.Open).Should().Be((int)BetStatus.Open);
            ((int)BetStatusId.Suspended).Should().Be((int)BetStatus.Suspended);
            ((int)BetStatusId.Settled).Should().Be((int)BetStatus.Settled);
        }

        [Theory]
        [InlineData(1, BetStatusId.Open)]
        [InlineData(2, BetStatusId.Suspended)]
        [InlineData(3, BetStatusId.Settled)]
        [InlineData(4, BetStatusId.Closed)]
        public void BetStatusId_CastFromInt_ShouldReturnCorrectEnum(int intValue, BetStatusId expected)
        {
            ((BetStatusId)intValue).Should().Be(expected);
        }
    }
}
