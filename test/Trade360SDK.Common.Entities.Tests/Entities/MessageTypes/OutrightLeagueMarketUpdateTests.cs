using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightLeagueMarketUpdateTests
    {
        private static readonly JsonSerializerOptions FeedJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>();
            var update = new OutrightLeagueMarketUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightLeagueMarketUpdate();
            Assert.Null(update.Competition);
        }

        [Fact]
        public void JsonDeserialization_WithCompleteRealWorldPayload_ShouldCreateCorrectEntity()
        {
            var json = File.ReadAllText(Path.Combine("Fixtures", "outright-league-market-update-type40.json"));

            var result = JsonSerializer.Deserialize<OutrightLeagueMarketUpdate>(json, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.Competition.Should().NotBeNull();
            result.Competition.Should().BeOfType<OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>>();
            result.Competition!.Id.Should().Be(67);
            result.Competition.Name.Should().Be("League_67");
            result.Competition.Type.Should().Be(3);
            result.GetNextFixtureStartTime().Should().Be(DateTime.Parse("2026-05-29T14:44:55Z").ToUniversalTime());

            var season = result.Competition.Competitions!.Single();
            season.Id.Should().Be(2029);
            season.Name.Should().Be("Season_2029");
            season.Type.Should().Be(4);

            var marketEvent = season.Events!.Single();
            marketEvent.FixtureId.Should().Be(26721036);
            marketEvent.FixtureName.Should().Be("Premier League 2023/2024 Outright Winner");
            marketEvent.Markets.Should().HaveCount(1);

            var market = marketEvent.Markets!.Single();
            market.Id.Should().Be(274);
            market.Name.Should().Be("Outright Winner");
            market.Bets.Should().HaveCount(2);
            market.Bets!.First().Name.Should().Be("Not Simply Simon");
            market.Bets.First().ParticipantId.Should().Be(51523593);
            market.ProviderMarkets.Should().HaveCount(1);
            market.ProviderMarkets!.First().Name.Should().Be("Bet365");
        }

        [Fact]
        public void GetNextFixtureStartTime_WhenCompetitionIsBaseWrapper_ShouldReturnNextFixtureStartTime()
        {
            var nextFixtureStartTime = DateTime.Parse("2026-05-29T14:44:55Z").ToUniversalTime();
            var update = new OutrightLeagueMarketUpdate
            {
                Competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>
                {
                    Id = 67,
                    Name = "League_67",
                    Type = 3,
                    NextFixtureStartTime = nextFixtureStartTime,
                }
            };

            update.GetNextFixtureStartTime().Should().Be(nextFixtureStartTime);
        }

        [Fact]
        public void GetNextFixtureStartTime_WhenCompetitionIsNull_ShouldReturnNull()
        {
            var update = new OutrightLeagueMarketUpdate();

            update.GetNextFixtureStartTime().Should().BeNull();
        }

        [Fact]
        public void JsonSerialization_ShouldRoundTripWithNextFixtureStartTime()
        {
            var json = File.ReadAllText(Path.Combine("Fixtures", "outright-league-market-update-type40.json"));

            var original = JsonSerializer.Deserialize<OutrightLeagueMarketUpdate>(json, FeedJsonOptions);
            var serialized = JsonSerializer.Serialize(original, FeedJsonOptions);
            var roundTripped = JsonSerializer.Deserialize<OutrightLeagueMarketUpdate>(serialized, FeedJsonOptions);

            roundTripped.Should().NotBeNull();
            roundTripped!.GetNextFixtureStartTime().Should().Be(DateTime.Parse("2026-05-29T14:44:55Z").ToUniversalTime());
            roundTripped.Competition.Should().BeOfType<OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>>();
        }
    }
}