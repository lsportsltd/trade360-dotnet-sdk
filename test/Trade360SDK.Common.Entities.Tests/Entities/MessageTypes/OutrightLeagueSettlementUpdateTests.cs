using System.Linq;
using System.Text.Json;
using FluentAssertions;
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

        [Fact]
        public void JsonDeserialization_WithCompleteRealWorldPayload_ShouldCreateCorrectEntity()
        {
            // Arrange - Real JSON payload from production (simplified to match current entity structure)
            var json = @"{
                ""Competition"": {
                    ""Id"": 67,
                    ""Name"": ""Premier League"",
                    ""Type"": 3,
                    ""Competitions"": [
                        {
                            ""Id"": 2029,
                            ""Name"": ""2023/2024"",
                            ""Type"": 4,
                            ""Events"": [
                                {
                                    ""FixtureId"": 24603148,
                                    ""Markets"": [
                                        {
                                            ""Id"": 274,
                                            ""Name"": ""Outright Winner"",
                                            ""MainLine"": ""1.5"",
                                            ""Bets"": [
                                                {
                                                    ""Id"": 126691427424603148,
                                                    ""Name"": ""Reliable Miss"",
                                                    ""Status"": 3,
                                                    ""StartPrice"": ""1.0"",
                                                    ""Price"": ""1.44"",
                                                    ""Settlement"": 2,
                                                    ""ProviderBetId"": ""8"",
                                                    ""CalculatedMargin"": null,
                                                    ""LastUpdate"": ""2025-07-20T09:02:59.754553Z"",
                                                    ""ParticipantId"": 52280038
                                                },
                                                {
                                                    ""Id"": 15926367224603148,
                                                    ""Name"": ""El Senor"",
                                                    ""Status"": 3,
                                                    ""StartPrice"": ""1.0"",
                                                    ""Price"": ""12.1"",
                                                    ""Settlement"": 1,
                                                    ""ProviderBetId"": ""8"",
                                                    ""LastUpdate"": ""2025-07-20T09:02:59.753568Z"",
                                                    ""ParticipantId"": 52280039
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            }";

            // Act
            var result = JsonSerializer.Deserialize<OutrightLeagueSettlementUpdate>(json);

            // Assert
            result.Should().NotBeNull();
            result.Competition.Should().NotBeNull();
            
            // Competition level assertions
            result.Competition!.Id.Should().Be(67);
            result.Competition.Name.Should().Be("Premier League");
            result.Competition.Type.Should().Be(3);
            result.Competition.Competitions.Should().NotBeNull();
            result.Competition.Competitions.Should().HaveCount(1);
            
            // Events wrapper level assertions
            var eventsWrapper = result.Competition.Competitions!.First();
            eventsWrapper.Id.Should().Be(2029);
            eventsWrapper.Name.Should().Be("2023/2024");
            eventsWrapper.Type.Should().Be(4);
            eventsWrapper.Events.Should().NotBeNull();
            eventsWrapper.Events.Should().HaveCount(1);
            
            // Market event level assertions
            var marketEvent = eventsWrapper.Events!.First();
            marketEvent.FixtureId.Should().Be(24603148);
            marketEvent.Markets.Should().NotBeNull();
            marketEvent.Markets.Should().HaveCount(1);
            
            // Market level assertions
            var market = marketEvent.Markets!.First();
            market.Id.Should().Be(274);
            market.Name.Should().Be("Outright Winner");
            market.MainLine.Should().Be("1.5");
            market.Bets.Should().NotBeNull();
            market.Bets.Should().HaveCount(2);
            
            // Bet level assertions
            var firstBet = market.Bets!.First();
            firstBet.Id.Should().Be(126691427424603148);
            firstBet.Name.Should().Be("Reliable Miss");
            firstBet.Status.Should().Be(Trade360SDK.Common.Entities.Enums.BetStatus.Settled);
            firstBet.StartPrice.Should().Be("1.0");
            firstBet.Price.Should().Be("1.44");
            firstBet.Settlement.Should().Be(Trade360SDK.Common.Entities.Enums.SettlementType.Winner);
            firstBet.ProviderBetId.Should().Be("8");
            firstBet.ParticipantId.Should().Be(52280038);
            
            var secondBet = market.Bets!.Skip(1).First();
            secondBet.Id.Should().Be(15926367224603148);
            secondBet.Name.Should().Be("El Senor");
            secondBet.Price.Should().Be("12.1");
            secondBet.Settlement.Should().Be(Trade360SDK.Common.Entities.Enums.SettlementType.Loser);
        }
    }
}
