using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetEventsResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var fixture = new Fixture { Sport = new Sport { Id = 1, Name = "Football" }, Status = Trade360SDK.Common.Entities.Enums.FixtureStatus.Finished };
            var livescore = new Livescore();
            var market = new Market { Id = 2, Name = "MarketName" };
            var response = new GetEventsResponse
            {
                Fixture = fixture,
                Livescore = livescore,
                Markets = new List<Market> { market },
                FixtureId = 99
            };

            Assert.Equal(fixture, response.Fixture);
            Assert.Equal(livescore, response.Livescore);
            Assert.NotNull(response.Markets);
            Assert.Equal(99, response.FixtureId);
            var marketItem = Assert.Single(response.Markets);
            Assert.Equal(2, marketItem.Id);
            Assert.Equal("MarketName", marketItem.Name);
        }
    }
} 