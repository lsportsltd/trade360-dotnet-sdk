using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class FixtureStatusTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.NotSet));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.NSY));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.InProgress));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.Finished));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.Cancelled));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.Postponed));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.Interrupted));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.Abandoned));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.LostCoverage));
            Assert.True(System.Enum.IsDefined(typeof(FixtureStatus), FixtureStatus.AboutToStart));
        }
    }
} 