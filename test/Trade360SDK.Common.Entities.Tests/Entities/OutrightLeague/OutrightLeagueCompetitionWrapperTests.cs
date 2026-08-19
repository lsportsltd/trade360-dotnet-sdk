using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueCompetitionWrapperTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competitions = new List<OutrightLeagueEventsWrapper<string>>
            {
                new OutrightLeagueEventsWrapper<string> { Id = 1 }
            };
            var nextFixtureStartTime = new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc);
            var wrapper = new OutrightLeagueCompetitionWrapper<string>
            {
                Id = 5,
                Name = "CompWrapper",
                Type = 2,
                NextFixtureStartTime = nextFixtureStartTime,
                Competitions = competitions
            };
            Assert.Equal(5, wrapper.Id);
            Assert.Equal("CompWrapper", wrapper.Name);
            Assert.Equal(2, wrapper.Type);
            Assert.Equal(nextFixtureStartTime, wrapper.NextFixtureStartTime);
            Assert.Equal(competitions, wrapper.Competitions);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var wrapper = new OutrightLeagueCompetitionWrapper<string>();
            Assert.Null(wrapper.Name);
            Assert.Null(wrapper.NextFixtureStartTime);
            Assert.Null(wrapper.Competitions);
        }
    }
} 