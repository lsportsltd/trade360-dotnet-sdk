using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class FlowTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(FlowType), FlowType.InPlay));
            Assert.True(System.Enum.IsDefined(typeof(FlowType), FlowType.PreMatch));
        }
    }
} 