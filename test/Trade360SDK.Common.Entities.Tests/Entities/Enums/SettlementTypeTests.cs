using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class SettlementTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.Cancelled));
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.NotSettled));
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.Loser));
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.Winner));
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.Refund));
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.HalfLost));
            Assert.True(System.Enum.IsDefined(typeof(SettlementType), SettlementType.HalfWon));
        }
    }
} 