using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueEventsWrapperTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var events = new List<string> { "Event1", "Event2" };
            var wrapper = new OutrightLeagueEventsWrapper<string>
            {
                Id = 7,
                Name = "LeagueWrapper",
                Type = 3,
                Events = events
            };
            Assert.Equal(7, wrapper.Id);
            Assert.Equal("LeagueWrapper", wrapper.Name);
            Assert.Equal(3, wrapper.Type);
            Assert.Equal(events, wrapper.Events);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var wrapper = new OutrightLeagueEventsWrapper<string>();
            Assert.Null(wrapper.Name);
            Assert.Null(wrapper.Events);
        }
    }
} 