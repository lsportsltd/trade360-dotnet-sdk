using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class LeagueTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var league = new League
            {
                Id = 7,
                Name = "Premier League"
            };
            Assert.Equal(7, league.Id);
            Assert.Equal("Premier League", league.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var league = new League();
            Assert.Equal(0, league.Id);
            Assert.Null(league.Name);
        }
    }
} 