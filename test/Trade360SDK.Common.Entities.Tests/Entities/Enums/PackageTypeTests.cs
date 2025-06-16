using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class PackageTypeTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(PackageType), PackageType.InPlay));
            Assert.True(System.Enum.IsDefined(typeof(PackageType), PackageType.PreMatch));
        }
    }
} 