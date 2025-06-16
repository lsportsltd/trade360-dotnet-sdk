using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class BetStatusTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.NotSet));
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.Open));
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.Suspended));
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.Settled));
        }
    }
} 