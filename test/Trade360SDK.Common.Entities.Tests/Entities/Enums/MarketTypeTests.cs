using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class MarketTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(MarketType), MarketType.All));
            Assert.True(System.Enum.IsDefined(typeof(MarketType), MarketType.Standard));
            Assert.True(System.Enum.IsDefined(typeof(MarketType), MarketType.Outright));
        }
    }
} 