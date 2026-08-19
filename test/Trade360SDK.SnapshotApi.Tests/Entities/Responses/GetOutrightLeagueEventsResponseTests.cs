using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Trade360SDK.Common.Entities.OutrightLeague;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightLeagueEventsResponseTests
    {
        [Fact]
        public void Competition_ShouldGetAndSetValues()
        {
            var eventsWrapper = new OutrightLeagueEventsWrapper<OutrightLeagueEvent>
            {
                Id = 1,
                Name = "Test Events Wrapper",
                Events = new[] { new OutrightLeagueEvent { FixtureId = 1 } }
            };
            
            var competitions = new List<OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>>
            {
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>
                {
                    Id = 100,
                    Name = "Test Competition",
                    Competitions = new[] { eventsWrapper }
                }
            };
            
            var response = new GetOutrightLeagueEventsResponse
            {
                Competition = competitions
            };
            
            Assert.Equal(competitions, response.Competition);
            Assert.Single(response.Competition);
            Assert.Single(response.Competition.First().Competitions);
            Assert.Single(response.Competition.First().Competitions.First().Events);
            Assert.Equal(1, response.Competition.First().Competitions.First().Events.First().FixtureId);
        }

        [Fact]
        public void Competition_ShouldAllowNullAndDefaults()
        {
            var response = new GetOutrightLeagueEventsResponse();
            
            Assert.Null(response.Competition);
        }

        [Fact]
        public void Competition_ShouldAcceptEmptyCollection()
        {
            var emptyCompetitions = new List<OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>>();
            var response = new GetOutrightLeagueEventsResponse
            {
                Competition = emptyCompetitions
            };
            
            Assert.NotNull(response.Competition);
            Assert.Empty(response.Competition);
        }

        [Fact]
        public void Competition_ShouldHandleMultipleCompetitions()
        {
            var eventsWrapper1 = new OutrightLeagueEventsWrapper<OutrightLeagueEvent>
            {
                Events = new[] { new OutrightLeagueEvent { FixtureId = 1 } }
            };
            var eventsWrapper2 = new OutrightLeagueEventsWrapper<OutrightLeagueEvent>
            {
                Events = new[] { new OutrightLeagueEvent { FixtureId = 2 } }
            };
            var eventsWrapper3 = new OutrightLeagueEventsWrapper<OutrightLeagueEvent>
            {
                Events = new[] { new OutrightLeagueEvent { FixtureId = 3 } }
            };
            
            var competitions = new List<OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>>
            {
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>
                {
                    Competitions = new[] { eventsWrapper1 }
                },
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>
                {
                    Competitions = new[] { eventsWrapper2 }
                },
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>
                {
                    Competitions = new[] { eventsWrapper3 }
                }
            };
            
            var response = new GetOutrightLeagueEventsResponse
            {
                Competition = competitions
            };
            
            Assert.Equal(3, response.Competition.Count());
            Assert.Equal(1, response.Competition.First().Competitions.First().Events.First().FixtureId);
            Assert.Equal(3, response.Competition.Last().Competitions.First().Events.First().FixtureId);
        }

        [Fact]
        public void Competition_ShouldHandleComplexNestedStructure()
        {
            var event1 = new OutrightLeagueEvent { FixtureId = 1 };
            var event2 = new OutrightLeagueEvent { FixtureId = 2 };
            
            var eventsWrapper = new OutrightLeagueEventsWrapper<OutrightLeagueEvent>
            {
                Id = 1,
                Name = "Test Events Wrapper",
                Events = new[] { event1, event2 }
            };
            
            var competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>
            {
                Id = 100,
                Name = "Test Competition",
                Competitions = new[] { eventsWrapper }
            };
            
            var response = new GetOutrightLeagueEventsResponse
            {
                Competition = new[] { competition }
            };
            
            Assert.NotNull(response.Competition);
            Assert.Single(response.Competition);
            Assert.Single(response.Competition.First().Competitions);
            Assert.Equal(2, response.Competition.First().Competitions.First().Events.Count());
            Assert.Equal(1, response.Competition.First().Competitions.First().Events.First().FixtureId);
            Assert.Equal(2, response.Competition.First().Competitions.First().Events.Last().FixtureId);
        }

        [Fact]
        public void Competition_ShouldBeReassignable()
        {
            var response = new GetOutrightLeagueEventsResponse();
            
            var firstCompetitions = new List<OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>>
            {
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>()
            };
            
            response.Competition = firstCompetitions;
            Assert.Equal(firstCompetitions, response.Competition);
            
            var secondCompetitions = new List<OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>>
            {
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>(),
                new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>()
            };
            
            response.Competition = secondCompetitions;
            Assert.Equal(secondCompetitions, response.Competition);
            Assert.Equal(2, response.Competition.Count());
        }

        [Fact]
        public void JsonDeserialization_WithNextFixtureStartTime_ShouldPopulateCompetition()
        {
            const string json = """
                [
                  {
                    "Competition": [
                      {
                        "Id": 67,
                        "Name": "League_67",
                        "Type": 3,
                        "NextFixtureStartTime": "2026-05-29T14:44:55Z",
                        "Competitions": [
                          {
                            "Id": 2029,
                            "Name": "Season_2029",
                            "Type": 4,
                            "Events": [
                              { "FixtureId": 26721036 }
                            ]
                          }
                        ]
                      }
                    ]
                  }
                ]
                """;

            var result = JsonSerializer.Deserialize<GetOutrightLeagueEventsResponse[]>(json);

            Assert.NotNull(result);
            Assert.Single(result);
            var competition = Assert.Single(result![0].Competition);
            Assert.Equal(67, competition.Id);
            Assert.Equal("League_67", competition.Name);
            Assert.Equal(3, competition.Type);
            Assert.Equal(DateTime.Parse("2026-05-29T14:44:55Z").ToUniversalTime(), competition.NextFixtureStartTime?.ToUniversalTime());
            Assert.Equal(26721036, competition.Competitions!.Single().Events!.Single().FixtureId);
        }
    }
}
