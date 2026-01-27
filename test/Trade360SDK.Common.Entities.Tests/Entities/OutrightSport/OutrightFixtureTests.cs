using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightFixtureTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var sport = new Sport();
            var location = new Location();
            var startDate = DateTime.UtcNow;
            var lastUpdate = DateTime.UtcNow.AddMinutes(-1);
            var participants = new List<OutrightFixtureParticipant> { new OutrightFixtureParticipant { Id = 1 } };
            var extraData = new List<NameValuePair> { new NameValuePair { Name = "key", Value = "val" } };
            var subscription = new Subscription();
            var venue = new FixtureVenue
            {
                Id = 1,
                Name = "Test Venue",
                Assignment = VenueAssignment.Home
            };
            var season = new IdNamePair { Id = 2024, Name = "2024-2025" };
            var fixture = new OutrightFixture
            {
                FixtureName = "World Cup 2024",
                Sport = sport,
                Location = location,
                StartDate = startDate,
                LastUpdate = lastUpdate,
                Status = FixtureStatus.Finished,
                Participants = participants,
                ExtraData = extraData,
                Subscription = subscription,
                Venue = venue,
                Season = season
            };
            Assert.Equal("World Cup 2024", fixture.FixtureName);
            Assert.Equal(sport, fixture.Sport);
            Assert.Equal(location, fixture.Location);
            Assert.Equal(startDate, fixture.StartDate);
            Assert.Equal(lastUpdate, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.Finished, fixture.Status);
            Assert.Equal(participants, fixture.Participants);
            Assert.Equal(extraData, fixture.ExtraData);
            Assert.Equal(subscription, fixture.Subscription);
            Assert.Equal(venue, fixture.Venue);
            Assert.Equal(season, fixture.Season);
            Assert.Equal(2024, fixture.Season.Id);
            Assert.Equal("2024-2025", fixture.Season.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var fixture = new OutrightFixture();
            Assert.Null(fixture.FixtureName);
            Assert.Null(fixture.Sport);
            Assert.Null(fixture.Location);
            Assert.Null(fixture.StartDate);
            Assert.Equal(default, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.NotSet, fixture.Status);
            Assert.Null(fixture.Participants);
            Assert.Null(fixture.ExtraData);
            Assert.Null(fixture.Subscription);
            Assert.Null(fixture.Venue);
            Assert.Null(fixture.Season);
        }

        [Fact]
        public void FixtureName_ShouldGetAndSetValue()
        {
            var fixture = new OutrightFixture();
            fixture.FixtureName = "Champions League Final";
            Assert.Equal("Champions League Final", fixture.FixtureName);

            fixture.FixtureName = null;
            Assert.Null(fixture.FixtureName);
        }

        [Fact]
        public void Season_ShouldGetAndSetValue()
        {
            var fixture = new OutrightFixture();
            var season = new IdNamePair { Id = 2025, Name = "2025-2026" };
            fixture.Season = season;
            Assert.Equal(season, fixture.Season);
            Assert.Equal(2025, fixture.Season.Id);
            Assert.Equal("2025-2026", fixture.Season.Name);
        }

        [Fact]
        public void Participants_ShouldContainEnhancedFields()
        {
            var player1 = new FixturePlayer { PlayerId = 10, ShirtNumber = "10", IsCaptain = true };
            var player2 = new FixturePlayer { PlayerId = 7, ShirtNumber = "7", IsCaptain = false };
            var fixturePlayers = new List<FixturePlayer> { player1, player2 };

            var participant = new OutrightFixtureParticipant
            {
                Id = 1,
                Name = "Manchester United",
                Position = "1",
                RotationId = 100,
                IsActive = 1,
                Form = "WDWWL",
                Formation = "4-2-3-1",
                FixturePlayers = fixturePlayers,
                Gender = 1,
                AgeCategory = 0,
                Type = 1
            };

            var fixture = new OutrightFixture
            {
                FixtureName = "Premier League Winner 2024",
                Season = new IdNamePair { Id = 2024, Name = "2024-2025" },
                Participants = new List<OutrightFixtureParticipant> { participant }
            };

            Assert.Equal("Premier League Winner 2024", fixture.FixtureName);
            Assert.Equal(2024, fixture.Season.Id);
            Assert.NotNull(fixture.Participants);
            
            var firstParticipant = ((List<OutrightFixtureParticipant>)fixture.Participants)[0];
            Assert.Equal("Manchester United", firstParticipant.Name);
            Assert.Equal("WDWWL", firstParticipant.Form);
            Assert.Equal("4-2-3-1", firstParticipant.Formation);
            Assert.Equal(2, ((List<FixturePlayer>)firstParticipant.FixturePlayers).Count);
            Assert.Equal(1, firstParticipant.Gender);
            Assert.Equal(0, firstParticipant.AgeCategory);
            Assert.Equal(1, firstParticipant.Type);
        }
    }
} 