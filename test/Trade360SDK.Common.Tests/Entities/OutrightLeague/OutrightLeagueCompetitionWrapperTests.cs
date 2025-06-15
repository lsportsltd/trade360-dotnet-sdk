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
            var wrapper = new OutrightLeagueCompetitionWrapper<string>
            {
                Id = 5,
                Name = "CompWrapper",
                Type = 2,
                Competitions = competitions
            };
            Assert.Equal(5, wrapper.Id);
            Assert.Equal("CompWrapper", wrapper.Name);
            Assert.Equal(2, wrapper.Type);
            Assert.Equal(competitions, wrapper.Competitions);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var wrapper = new OutrightLeagueCompetitionWrapper<string>();
            Assert.Null(wrapper.Name);
            Assert.Null(wrapper.Competitions);
        }
    }
} 