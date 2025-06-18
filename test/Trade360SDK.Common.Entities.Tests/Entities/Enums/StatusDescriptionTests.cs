using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class StatusDescriptionTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.None));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.HT));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.OTHT));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.HomeRetired));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.AwayRetired));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.LostCoverage));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.MedicalTimeout));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TimeoutHomeTeam));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TimeoutAwayTeam));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.Timeout));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.HomeWalkover));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.AwayWalkover));
        }
    }
} 