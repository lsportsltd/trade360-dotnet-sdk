using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class LocationTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var location = new Location
            {
                Id = 3,
                Name = "London"
            };
            Assert.Equal(3, location.Id);
            Assert.Equal("London", location.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var location = new Location();
            Assert.Equal(0, location.Id);
            Assert.Null(location.Name);
        }
    }
} 