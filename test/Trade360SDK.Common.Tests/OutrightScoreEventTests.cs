using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightScoreEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var score = new OutrightLivescoreScore();
            var evt = new OutrightScoreEvent
            {
                FixtureId = 42,
                OutrightScore = score
            };
            Assert.Equal(42, evt.FixtureId);
            Assert.Equal(score, evt.OutrightScore);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new OutrightScoreEvent();
            Assert.Equal(0, evt.FixtureId); // default int
            Assert.Null(evt.OutrightScore);
        }
    }
} 