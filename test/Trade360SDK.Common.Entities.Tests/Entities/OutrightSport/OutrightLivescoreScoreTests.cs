using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;
using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLivescoreScoreTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var results = new List<OutrightParticipantResult> { new OutrightParticipantResult { ParticipantId = 1 } };
            var score = new OutrightLivescoreScore
            {
                ParticipantResults = results,
                Status = FixtureStatus.Finished
            };
            Assert.Equal(results, score.ParticipantResults);
            Assert.Equal(FixtureStatus.Finished, score.Status);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var score = new OutrightLivescoreScore();
            Assert.Null(score.ParticipantResults);
            Assert.Equal(FixtureStatus.NotSet, score.Status); // default enum value
        }
    }
} 