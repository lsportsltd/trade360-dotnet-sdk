using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class SportTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var sport = new Sport
            {
                Id = 2,
                Name = "Football"
            };
            Assert.Equal(2, sport.Id);
            Assert.Equal("Football", sport.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var sport = new Sport();
            Assert.Equal(0, sport.Id);
            Assert.Null(sport.Name);
        }
    }
} 