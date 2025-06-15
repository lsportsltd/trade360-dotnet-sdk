using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class FixtureEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var fixture = new Fixture();
            var evt = new FixtureEvent
            {
                FixtureId = 42,
                Fixture = fixture
            };
            Assert.Equal(42, evt.FixtureId);
            Assert.Equal(fixture, evt.Fixture);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new FixtureEvent();
            Assert.Equal(0, evt.FixtureId);
            Assert.Null(evt.Fixture);
        }
    }
} 