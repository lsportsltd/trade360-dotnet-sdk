using System;
using System.Collections.Generic;
using System.Text.Json;
using Trade360SDK.Common.Entities.OutrightLeague;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueFixtureTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var subscription = new Subscription();
            var sport = new Sport();
            var location = new Location();
            var startDate = DateTime.UtcNow.AddHours(1);
            var lastUpdate = DateTime.UtcNow;
            var extraData = new List<NameValuePair> { new NameValuePair { Name = "key", Value = "val" } };
            var endDate = DateTime.UtcNow.AddDays(10);
            var season = new IdNamePair { Id = 2024, Name = "2024-2025" };

            var fixture = new OutrightLeagueFixture
            {
                FixtureName = "League Championship 2024",
                Subscription = subscription,
                Sport = sport,
                Location = location,
                StartDate = startDate,
                LastUpdate = lastUpdate,
                Status = FixtureStatus.Finished,
                ExtraData = extraData,
                EndDate = endDate,
                Season = season
            };
            Assert.Equal("League Championship 2024", fixture.FixtureName);
            Assert.Equal(subscription, fixture.Subscription);
            Assert.Equal(sport, fixture.Sport);
            Assert.Equal(location, fixture.Location);
            Assert.Equal(startDate, fixture.StartDate);
            Assert.Equal(lastUpdate, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.Finished, fixture.Status);
            Assert.Equal(extraData, fixture.ExtraData);
            Assert.Equal(endDate, fixture.EndDate);
            Assert.Equal(season, fixture.Season);
            Assert.Equal(2024, fixture.Season.Id);
            Assert.Equal("2024-2025", fixture.Season.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var fixture = new OutrightLeagueFixture();
            Assert.Null(fixture.FixtureName);
            Assert.Null(fixture.Subscription);
            Assert.Null(fixture.Sport);
            Assert.Null(fixture.Location);
            Assert.Null(fixture.StartDate);
            Assert.Equal(default, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.NotSet, fixture.Status);
            Assert.Null(fixture.ExtraData);
            Assert.Null(fixture.EndDate);
            Assert.Null(fixture.Season);
        }

        [Fact]
        public void EndDate_ShouldDeserializeNullFromJson()
        {
            const string json = """
                {
                    "EndDate": null,
                    "StartDate": "2026-07-04T10:00:00Z",
                    "LastUpdate": "2026-05-03T08:51:37.990863Z",
                    "Status": 1
                }
                """;

            var fixture = JsonSerializer.Deserialize<OutrightLeagueFixture>(json);

            Assert.NotNull(fixture);
            Assert.Null(fixture.EndDate);
            Assert.NotNull(fixture.StartDate);
        }

        [Fact]
        public void FixtureName_ShouldGetAndSetValue()
        {
            var fixture = new OutrightLeagueFixture();
            fixture.FixtureName = "World Series 2024";
            Assert.Equal("World Series 2024", fixture.FixtureName);

            fixture.FixtureName = null;
            Assert.Null(fixture.FixtureName);
        }

        [Fact]
        public void Season_ShouldGetAndSetValue()
        {
            var fixture = new OutrightLeagueFixture();
            var season = new IdNamePair { Id = 2025, Name = "2025-2026" };
            fixture.Season = season;
            Assert.Equal(season, fixture.Season);
            Assert.Equal(2025, fixture.Season.Id);
            Assert.Equal("2025-2026", fixture.Season.Name);
        }
    }
} 