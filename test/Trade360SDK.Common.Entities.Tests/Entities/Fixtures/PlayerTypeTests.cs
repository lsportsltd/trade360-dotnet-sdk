using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Fixtures
{
    public class PlayerTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(PlayerType), PlayerType.Player));
            Assert.True(System.Enum.IsDefined(typeof(PlayerType), PlayerType.Other));
            Assert.True(System.Enum.IsDefined(typeof(PlayerType), PlayerType.Coach));
        }

        [Fact]
        public void Enum_Player_ShouldHaveValue1()
        {
            Assert.Equal(0, (int)PlayerType.Player);
        }

        [Fact]
        public void Enum_Other_ShouldHaveValue2()
        {
            Assert.Equal(0, (int)PlayerType.Other);
        }

        [Fact]
        public void Enum_Coach_ShouldHaveValue3()
        {
            Assert.Equal(1, (int)PlayerType.Coach);
        }

        [Fact]
        public void Enum_ShouldHaveExactlyThreeValues()
        {
            var values = System.Enum.GetValues(typeof(PlayerType));
            Assert.Equal(2, values.Length);
        }

        [Theory]
        [InlineData(0, "Player")]
        [InlineData(1, "Other")]
        [InlineData(2, "Coach")]
        public void Enum_ShouldHaveCorrectNameForValue(int value, string expectedName)
        {
            var enumValue = (PlayerType)value;
            Assert.Equal(expectedName, enumValue.ToString());
        }
    }
}

