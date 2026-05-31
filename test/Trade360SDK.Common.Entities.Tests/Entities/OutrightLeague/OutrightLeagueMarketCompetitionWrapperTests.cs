using System;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueMarketCompetitionWrapperTests
    {
        [Fact]
        public void NextFixtureStartTime_ShouldGetAndSetValue()
        {
            var wrapper = new OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>
            {
                Id = 67,
                Name = "League_67",
                Type = 3,
                NextFixtureStartTime = new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc),
            };

            Assert.Equal(67, wrapper.Id);
            Assert.Equal("League_67", wrapper.Name);
            Assert.Equal(3, wrapper.Type);
            Assert.Equal(new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc), wrapper.NextFixtureStartTime);
        }

        [Fact]
        public void NextFixtureStartTime_ShouldAllowNull()
        {
            var wrapper = new OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>();
            Assert.Null(wrapper.NextFixtureStartTime);
        }
    }
}
