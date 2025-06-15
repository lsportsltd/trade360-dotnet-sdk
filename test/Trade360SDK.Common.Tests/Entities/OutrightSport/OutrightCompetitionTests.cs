using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightCompetitionTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var events = new List<string> { "Event1", "Event2" };
            var comp = new OutrightCompetition<string>
            {
                Id = 10,
                Name = "Competition",
                Type = 2,
                Events = events
            };
            Assert.Equal(10, comp.Id);
            Assert.Equal("Competition", comp.Name);
            Assert.Equal(2, comp.Type);
            Assert.Equal(events, comp.Events);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var comp = new OutrightCompetition<string>();
            Assert.Null(comp.Name);
            Assert.Null(comp.Events);
        }
    }
} 