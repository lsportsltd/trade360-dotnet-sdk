using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class CompetitionTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(CompetitionType), CompetitionType.NotSet));
            Assert.True(System.Enum.IsDefined(typeof(CompetitionType), CompetitionType.Track));
            Assert.True(System.Enum.IsDefined(typeof(CompetitionType), CompetitionType.League));
            Assert.True(System.Enum.IsDefined(typeof(CompetitionType), CompetitionType.Season));
        }
    }
} 