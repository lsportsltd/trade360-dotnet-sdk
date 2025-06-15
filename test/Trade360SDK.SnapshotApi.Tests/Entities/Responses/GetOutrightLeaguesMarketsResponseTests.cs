using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightLeaguesMarketsResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var market = new OutrightMarketsResponse { FixtureId = 111, Markets = null };
            var snapshotEvent = new SnapshotOutrightEventsResponse
            {
                Id = 222,
                Name = "EventName",
                Type = 4,
                Events = new List<OutrightMarketsResponse> { market }
            };
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 333,
                Name = "LeaguesMarkets",
                Type = 8,
                Competitions = new List<SnapshotOutrightEventsResponse> { snapshotEvent }
            };

            Assert.Equal(333, response.Id);
            Assert.Equal("LeaguesMarkets", response.Name);
            Assert.Equal(8, response.Type);
            Assert.NotNull(response.Competitions);
            var evt = Assert.Single(response.Competitions);
            Assert.Equal(222, evt.Id);
            Assert.Equal("EventName", evt.Name);
            Assert.Equal(4, evt.Type);
            var marketEvt = Assert.Single(evt.Events);
            Assert.Equal(111, marketEvt.FixtureId);
        }
    }
} 