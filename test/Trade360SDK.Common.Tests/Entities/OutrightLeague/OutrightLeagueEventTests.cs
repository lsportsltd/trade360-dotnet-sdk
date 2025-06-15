using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var league = new OutrightLeagueFixture();
            var evt = new OutrightLeagueEvent
            {
                FixtureId = 77,
                OutrightLeague = league
            };
            Assert.Equal(77, evt.FixtureId);
            Assert.Equal(league, evt.OutrightLeague);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new OutrightLeagueEvent();
            Assert.Equal(0, evt.FixtureId); // default int
            Assert.Null(evt.OutrightLeague);
        }
    }
} 