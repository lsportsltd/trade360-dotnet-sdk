using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightFixtureEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var fixture = new OutrightFixture();
            var evt = new OutrightFixtureEvent
            {
                FixtureId = 99,
                OutrightFixture = fixture
            };
            Assert.Equal(99, evt.FixtureId);
            Assert.Equal(fixture, evt.OutrightFixture);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new OutrightFixtureEvent();
            Assert.Equal(0, evt.FixtureId); // default int
            Assert.Null(evt.OutrightFixture);
        }
    }
} 