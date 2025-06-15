using System.Collections.Generic;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class SettlementUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var eventsList = new List<MarketEvent> { new MarketEvent() };
            var update = new SettlementUpdate
            {
                Events = eventsList
            };
            Assert.Equal(eventsList, update.Events);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new SettlementUpdate();
            Assert.Null(update.Events);
        }
    }
} 