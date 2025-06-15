using System.Collections.Generic;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class LivescoreUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var eventsList = new List<LivescoreEvent> { new LivescoreEvent() };
            var update = new LivescoreUpdate
            {
                Events = eventsList
            };
            Assert.Equal(eventsList, update.Events);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new LivescoreUpdate();
            Assert.Null(update.Events);
        }
    }
} 