using System.Collections.Generic;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class LivescoreTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var scoreboard = new Scoreboard();
            var periods = new List<Period> { new Period() };
            var statistics = new List<Statistic> { new Statistic() };
            var extraData = new List<NameValuePair> { new NameValuePair() };
            var currentIncident = new CurrentIncident();
            var dangerTriggers = new List<DangerIndicator> { new DangerIndicator() };
            var livescore = new Livescore
            {
                Scoreboard = scoreboard,
                Periods = periods,
                Statistics = statistics,
                LivescoreExtraData = extraData,
                CurrentIncident = currentIncident,
                DangerTriggers = dangerTriggers
            };
            Assert.Equal(scoreboard, livescore.Scoreboard);
            Assert.Equal(periods, livescore.Periods);
            Assert.Equal(statistics, livescore.Statistics);
            Assert.Equal(extraData, livescore.LivescoreExtraData);
            Assert.Equal(currentIncident, livescore.CurrentIncident);
            Assert.Equal(dangerTriggers, livescore.DangerTriggers);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var livescore = new Livescore();
            Assert.Null(livescore.Scoreboard);
            Assert.Null(livescore.Periods);
            Assert.Null(livescore.Statistics);
            Assert.Null(livescore.LivescoreExtraData);
            Assert.Null(livescore.CurrentIncident);
            Assert.Null(livescore.DangerTriggers);
        }
    }
} 