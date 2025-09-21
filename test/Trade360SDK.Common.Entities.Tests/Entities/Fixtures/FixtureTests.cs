using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures
{
    public class FixtureTests
    {
        [Fact]
        public void Fixture_DefaultConstructor_ShouldCreateInstanceWithNullProperties()
        {
            // Act
            var fixture = new Fixture();

            // Assert
            fixture.Should().NotBeNull();
            fixture.Sport.Should().BeNull();
            fixture.Location.Should().BeNull();
            fixture.League.Should().BeNull();
            fixture.StartDate.Should().BeNull();
            fixture.LastUpdate.Should().Be(default(DateTime));
            fixture.Status.Should().Be(default(FixtureStatus));
            fixture.Participants.Should().BeNull();
            fixture.FixtureExtraData.Should().BeNull();
            fixture.ExternalFixtureId.Should().BeNull();
            fixture.Subscription.Should().BeNull();
            fixture.Venue.Should().BeNull();
            fixture.Stage.Should().BeNull();
            fixture.Round.Should().BeNull();
        }

        [Fact]
        public void Fixture_SetSport_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var sport = new Sport { Id = 1, Name = "Football" };

            // Act
            fixture.Sport = sport;

            // Assert
            fixture.Sport.Should().Be(sport);
            fixture.Sport.Id.Should().Be(1);
            fixture.Sport.Name.Should().Be("Football");
        }

        [Fact]
        public void Fixture_SetLocation_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var location = new Location { Id = 1, Name = "England" };

            // Act
            fixture.Location = location;

            // Assert
            fixture.Location.Should().Be(location);
            fixture.Location.Id.Should().Be(1);
            fixture.Location.Name.Should().Be("England");
        }

        [Fact]
        public void Fixture_SetLeague_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var league = new League { Id = 1, Name = "Premier League" };

            // Act
            fixture.League = league;

            // Assert
            fixture.League.Should().Be(league);
            fixture.League.Id.Should().Be(1);
            fixture.League.Name.Should().Be("Premier League");
        }

        [Fact]
        public void Fixture_SetStartDate_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var startDate = new DateTime(2023, 12, 1, 15, 30, 0, DateTimeKind.Utc);

            // Act
            fixture.StartDate = startDate;

            // Assert
            fixture.StartDate.Should().Be(startDate);
        }

        [Fact]
        public void Fixture_SetLastUpdate_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var lastUpdate = new DateTime(2023, 12, 1, 10, 0, 0, DateTimeKind.Utc);

            // Act
            fixture.LastUpdate = lastUpdate;

            // Assert
            fixture.LastUpdate.Should().Be(lastUpdate);
        }

        [Fact]
        public void Fixture_SetStatus_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var status = FixtureStatus.InProgress;

            // Act
            fixture.Status = status;

            // Assert
            fixture.Status.Should().Be(status);
        }

        [Fact]
        public void Fixture_SetParticipants_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var participants = new List<Participant>
            {
                new Participant { Id = 1, Name = "Team A" },
                new Participant { Id = 2, Name = "Team B" }
            };

            // Act
            fixture.Participants = participants;

            // Assert
            fixture.Participants.Should().BeEquivalentTo(participants);
            fixture.Participants.Should().HaveCount(2);
            fixture.Participants.First().Name.Should().Be("Team A");
            fixture.Participants.Last().Name.Should().Be("Team B");
        }

        [Fact]
        public void Fixture_SetFixtureExtraData_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var extraData = new List<NameValuePair>
            {
                new NameValuePair { Name = "Weather", Value = "Sunny" },
                new NameValuePair { Name = "Temperature", Value = "20°C" }
            };

            // Act
            fixture.FixtureExtraData = extraData;

            // Assert
            fixture.FixtureExtraData.Should().BeEquivalentTo(extraData);
            fixture.FixtureExtraData.Should().HaveCount(2);
        }

        [Fact]
        public void Fixture_SetExternalFixtureId_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var externalId = "EXT-12345";

            // Act
            fixture.ExternalFixtureId = externalId;

            // Assert
            fixture.ExternalFixtureId.Should().Be(externalId);
        }

        [Fact]
        public void Fixture_SetSubscription_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var subscription = new Subscription();

            // Act
            fixture.Subscription = subscription;

            // Assert
            fixture.Subscription.Should().Be(subscription);
        }

        [Fact]
        public void Fixture_SetVenue_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var venue = new FixtureVenue
            {
                Id = 1,
                Name = "Wembley Stadium",
                Assignment = VenueAssignment.Home
            };

            // Act
            fixture.Venue = venue;

            // Assert
            fixture.Venue.Should().Be(venue);
            fixture.Venue.Id.Should().Be(1);
            fixture.Venue.Name.Should().Be("Wembley Stadium");
            fixture.Venue.Assignment.Should().Be(VenueAssignment.Home);
        }

        [Fact]
        public void Fixture_SetStage_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var stage = new IdNamePair { Id = 1, Name = "Group Stage" };

            // Act
            fixture.Stage = stage;

            // Assert
            fixture.Stage.Should().Be(stage);
            fixture.Stage.Id.Should().Be(1);
            fixture.Stage.Name.Should().Be("Group Stage");
        }

        [Fact]
        public void Fixture_SetRound_ShouldSetValue()
        {
            // Arrange
            var fixture = new Fixture();
            var round = new IdNamePair { Id = 2, Name = "Round of 16" };

            // Act
            fixture.Round = round;

            // Assert
            fixture.Round.Should().Be(round);
            fixture.Round.Id.Should().Be(2);
            fixture.Round.Name.Should().Be("Round of 16");
        }

        [Fact]
        public void Fixture_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var fixture = new Fixture();
            var sport = new Sport { Id = 1, Name = "Football" };
            var location = new Location { Id = 1, Name = "England" };
            var league = new League { Id = 1, Name = "Premier League" };
            var startDate = new DateTime(2023, 12, 1, 15, 30, 0, DateTimeKind.Utc);
            var lastUpdate = new DateTime(2023, 12, 1, 10, 0, 0, DateTimeKind.Utc);
            var status = FixtureStatus.InProgress;
            var participants = new List<Participant> { new Participant { Id = 1, Name = "Team A" } };
            var extraData = new List<NameValuePair> { new NameValuePair { Name = "Weather", Value = "Sunny" } };
            var externalId = "EXT-12345";
            var subscription = new Subscription();
            var venue = new FixtureVenue
            {
                Id = 1,
                Name = "Old Trafford",
                Assignment = VenueAssignment.Home
            };
            var stage = new IdNamePair { Id = 1, Name = "Knockout Stage" };
            var round = new IdNamePair { Id = 2, Name = "Final" };

            // Act
            fixture.Sport = sport;
            fixture.Location = location;
            fixture.League = league;
            fixture.StartDate = startDate;
            fixture.LastUpdate = lastUpdate;
            fixture.Status = status;
            fixture.Participants = participants;
            fixture.FixtureExtraData = extraData;
            fixture.ExternalFixtureId = externalId;
            fixture.Subscription = subscription;
            fixture.Venue = venue;
            fixture.Stage = stage;
            fixture.Round = round;

            // Assert
            fixture.Sport.Should().Be(sport);
            fixture.Location.Should().Be(location);
            fixture.League.Should().Be(league);
            fixture.StartDate.Should().Be(startDate);
            fixture.LastUpdate.Should().Be(lastUpdate);
            fixture.Status.Should().Be(status);
            fixture.Participants.Should().BeEquivalentTo(participants);
            fixture.FixtureExtraData.Should().BeEquivalentTo(extraData);
            fixture.ExternalFixtureId.Should().Be(externalId);
            fixture.Subscription.Should().Be(subscription);
            fixture.Venue.Should().Be(venue);
            fixture.Stage.Should().Be(stage);
            fixture.Round.Should().Be(round);
        }

        [Fact]
        public void Fixture_SetEmptyCollections_ShouldSetValues()
        {
            // Arrange
            var fixture = new Fixture();
            var emptyParticipants = new List<Participant>();
            var emptyExtraData = new List<NameValuePair>();

            // Act
            fixture.Participants = emptyParticipants;
            fixture.FixtureExtraData = emptyExtraData;

            // Assert
            fixture.Participants.Should().BeEmpty();
            fixture.FixtureExtraData.Should().BeEmpty();
        }

        [Fact]
        public void Fixture_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            fixture.Sport = null;
            fixture.Location = null;
            fixture.League = null;
            fixture.StartDate = null;
            fixture.Participants = null;
            fixture.FixtureExtraData = null;
            fixture.ExternalFixtureId = null;
            fixture.Subscription = null;
            fixture.Venue = null;
            fixture.Stage = null;
            fixture.Round = null;

            // Assert
            fixture.Sport.Should().BeNull();
            fixture.Location.Should().BeNull();
            fixture.League.Should().BeNull();
            fixture.StartDate.Should().BeNull();
            fixture.Participants.Should().BeNull();
            fixture.FixtureExtraData.Should().BeNull();
            fixture.ExternalFixtureId.Should().BeNull();
            fixture.Subscription.Should().BeNull();
            fixture.Venue.Should().BeNull();
            fixture.Stage.Should().BeNull();
            fixture.Round.Should().BeNull();
        }

        [Theory]
        [InlineData(FixtureStatus.NotSet)]
        [InlineData(FixtureStatus.NSY)]
        [InlineData(FixtureStatus.InProgress)]
        [InlineData(FixtureStatus.Finished)]
        [InlineData(FixtureStatus.Cancelled)]
        [InlineData(FixtureStatus.Postponed)]
        public void Fixture_SetVariousStatuses_ShouldSetValue(FixtureStatus status)
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            fixture.Status = status;

            // Assert
            fixture.Status.Should().Be(status);
        }

        [Fact]
        public void Fixture_SetDateTimeMinMax_ShouldSetValues()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            fixture.StartDate = DateTime.MinValue;
            fixture.LastUpdate = DateTime.MaxValue;

            // Assert
            fixture.StartDate.Should().Be(DateTime.MinValue);
            fixture.LastUpdate.Should().Be(DateTime.MaxValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("EXT-12345")]
        [InlineData("very-long-external-fixture-id-with-many-characters")]
        public void Fixture_SetVariousExternalIds_ShouldSetValue(string externalId)
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            fixture.ExternalFixtureId = externalId;

            // Assert
            fixture.ExternalFixtureId.Should().Be(externalId);
        }

        [Fact]
        public void Fixture_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert - Test that we can set and get each property multiple times
            fixture.ExternalFixtureId = "test1";
            fixture.ExternalFixtureId.Should().Be("test1");
            fixture.ExternalFixtureId = "test2";
            fixture.ExternalFixtureId.Should().Be("test2");

            var status1 = FixtureStatus.NotSet;
            var status2 = FixtureStatus.InProgress;
            fixture.Status = status1;
            fixture.Status.Should().Be(status1);
            fixture.Status = status2;
            fixture.Status.Should().Be(status2);
        }
    }
} 