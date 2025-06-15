using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class DangerIndicatorTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(DangerIndicatorType), DangerIndicatorType.General));
            Assert.True(System.Enum.IsDefined(typeof(DangerIndicatorType), DangerIndicatorType.Cards));
            Assert.True(System.Enum.IsDefined(typeof(DangerIndicatorType), DangerIndicatorType.Corners));
        }
    }
} 