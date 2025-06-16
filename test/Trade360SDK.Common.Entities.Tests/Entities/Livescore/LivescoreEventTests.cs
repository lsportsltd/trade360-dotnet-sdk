using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class LivescoreEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var fixture = new Fixture();
            var livescore = new Livescore();
            var evt = new LivescoreEvent
            {
                FixtureId = 123,
                Fixture = fixture,
                Livescore = livescore
            };
            Assert.Equal(123, evt.FixtureId);
            Assert.Equal(fixture, evt.Fixture);
            Assert.Equal(livescore, evt.Livescore);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new LivescoreEvent();
            Assert.Equal(0, evt.FixtureId);
            Assert.Null(evt.Fixture);
            Assert.Null(evt.Livescore);
        }
    }
} 