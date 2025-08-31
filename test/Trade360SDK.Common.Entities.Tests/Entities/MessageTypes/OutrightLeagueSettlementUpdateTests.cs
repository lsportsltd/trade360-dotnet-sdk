using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightLeagueSettlementUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>();
            var update = new OutrightLeagueSettlementUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightLeagueSettlementUpdate();
            Assert.Null(update.Competition);
        }

        [Fact]
        public void InheritsFromMessageUpdate()
        {
            var update = new OutrightLeagueSettlementUpdate();
            Assert.IsAssignableFrom<MessageUpdate>(update);
        }

        [Fact]
        public void Competition_ShouldAcceptDifferentGenericTypes()
        {
            var competitionWithEvent = new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>();
            var competitionWithMarketEvent = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>();
            
            var update1 = new OutrightLeagueSettlementUpdate
            {
                Competition = competitionWithMarketEvent
            };
            
            Assert.Equal(competitionWithMarketEvent, update1.Competition);
        }

        [Fact]
        public void Competition_ShouldHandleComplexNestedStructure()
        {
            var marketEvent = new OutrightLeagueMarketEvent
            {
                FixtureId = 123,
                Markets = new[] { new MarketLeague { Id = 1, Name = "Test Market" } }
            };
            
            var eventsWrapper = new OutrightLeagueEventsWrapper<OutrightLeagueMarketEvent>
            {
                Id = 1,
                Name = "Test Events Wrapper",
                Events = new[] { marketEvent }
            };
            
            var competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>
            {
                Id = 100,
                Name = "Test Competition",
                Competitions = new[] { eventsWrapper }
            };
            
            var update = new OutrightLeagueSettlementUpdate
            {
                Competition = competition
            };
            
            Assert.NotNull(update.Competition);
            Assert.Equal(competition, update.Competition);
            Assert.Equal(100, update.Competition.Id);
            Assert.Equal("Test Competition", update.Competition.Name);
            Assert.Single(update.Competition.Competitions);
            Assert.Single(update.Competition.Competitions.First().Events);
            Assert.Equal(123, update.Competition.Competitions.First().Events.First().FixtureId);
        }
    }
}
