using System.Collections.Generic;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ScoreboardTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var results = new List<Result> { new Result() };
            var scoreboard = new Scoreboard
            {
                Status = FixtureStatus.Finished,
                Description = StatusDescription.None,
                CurrentPeriod = 2,
                Time = "90:00",
                Results = results
            };
            Assert.Equal(FixtureStatus.Finished, scoreboard.Status);
            Assert.Equal(StatusDescription.None, scoreboard.Description);
            Assert.Equal(2, scoreboard.CurrentPeriod);
            Assert.Equal("90:00", scoreboard.Time);
            Assert.Equal(results, scoreboard.Results);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var scoreboard = new Scoreboard();
            Assert.Equal(FixtureStatus.NotSet, scoreboard.Status);
            Assert.Equal(StatusDescription.None, scoreboard.Description);
            Assert.Equal(0, scoreboard.CurrentPeriod);
            Assert.Null(scoreboard.Time);
            Assert.Null(scoreboard.Results);
        }
    }
} 