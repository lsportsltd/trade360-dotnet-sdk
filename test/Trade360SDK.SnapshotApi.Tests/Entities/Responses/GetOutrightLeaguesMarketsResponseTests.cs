using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Xunit;
using Trade360SDK.Common.Entities.OutrightLeague;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightLeaguesMarketsResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var marketLeague = new MarketLeague { Id = 111, Name = "Market", Bets = null };
            var snapshotEvent = new OutrightLeagueMarketEvent
            {
                FixtureId = 222,
                Markets = new List<MarketLeague> { marketLeague }
            };
            var competitionWrapper = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>
            {
                Id = 333,
                Name = "Competition",
                Type = 5,
                Competitions = new List<OutrightLeagueEventsWrapper<OutrightLeagueMarketEvent>>
                {
                    new OutrightLeagueEventsWrapper<OutrightLeagueMarketEvent>
                    {
                        Id = 444,
                        Name = "Event Wrapper",
                        Type = 6,
                        Events = new List<OutrightLeagueMarketEvent> { snapshotEvent }
                    }
                }
            };
            
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Competition = new List<OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>> { competitionWrapper }
            };
            
            Assert.NotNull(response.Competition);
            var competition = Assert.Single(response.Competition);
            Assert.Equal(333, competition.Id);
            Assert.Equal("Competition", competition.Name);
            Assert.Equal(5, competition.Type);
            
            Assert.NotNull(competition.Competitions);
            var eventWrapper = Assert.Single(competition.Competitions);
            Assert.Equal(444, eventWrapper.Id);
            Assert.Equal("Event Wrapper", eventWrapper.Name);
            Assert.Equal(6, eventWrapper.Type);
            
            Assert.NotNull(eventWrapper.Events);
            var evt = Assert.Single(eventWrapper.Events);
            Assert.Equal(222, evt.FixtureId);
            Assert.NotNull(evt.Markets);
            var marketEvt = Assert.Single(evt.Markets);
            Assert.Equal(111, marketEvt.Id);
            Assert.Equal("Market", marketEvt.Name);
        }
    }
} 