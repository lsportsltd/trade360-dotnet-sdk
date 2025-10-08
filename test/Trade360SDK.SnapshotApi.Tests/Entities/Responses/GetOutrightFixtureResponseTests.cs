using System;
using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightFixtureResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var sport = new Sport { Id = 1, Name = "Football" };
            var location = new Location { Id = 10, Name = "Stadium" };
            var participant = new Participant { Id = 100, Name = "Team A", Position = "1", RotationId = 5, IsActive = 1 };
            var extraData = new NameValuePair { Name = "Key", Value = "Value" };
            var venue = new FixtureVenue
            {
                Id = 1,
                Name = "Test Stadium",
                Assignment = VenueAssignment.Home
            };
            var outrightFixture = new OutrightFixtureSnapshotResponse
            {
                Sport = sport,
                Location = location,
                StartDate = new DateTime(2024, 1, 1),
                LastUpdate = DateTime.UtcNow,
                Status = FixtureStatus.InProgress,
                Participants = new List<Participant> { participant },
                ExtraData = new List<NameValuePair> { extraData },
                Subscription = null,
                Venue = venue
            };
            var eventItem = new SnapshotOutrightFixtureEvent
            {
                FixtureId = 123,
                OutrightFixture = outrightFixture
            };
            var response = new GetOutrightFixtureResponse
            {
                Id = 42,
                Name = "Test Fixture",
                Type = 7,
                Events = new List<SnapshotOutrightFixtureEvent> { eventItem }
            };

            Assert.Equal(42, response.Id);
            Assert.Equal("Test Fixture", response.Name);
            Assert.Equal(7, response.Type);
            Assert.NotNull(response.Events);
            var evt = Assert.Single(response.Events);
            Assert.Equal(123, evt.FixtureId);
            Assert.NotNull(evt.OutrightFixture);
            Assert.Equal(sport, evt.OutrightFixture.Sport);
            Assert.Equal(location, evt.OutrightFixture.Location);
            Assert.Equal(new DateTime(2024, 1, 1), evt.OutrightFixture.StartDate);
            Assert.Equal(FixtureStatus.InProgress, evt.OutrightFixture.Status);
            Assert.NotNull(evt.OutrightFixture.Participants);
            Assert.Single(evt.OutrightFixture.Participants);
            Assert.Equal(participant.Name, Assert.Single(evt.OutrightFixture.Participants).Name);
            Assert.NotNull(evt.OutrightFixture.ExtraData);
            Assert.Equal("Key", Assert.Single(evt.OutrightFixture.ExtraData).Name);
            Assert.Equal(venue, evt.OutrightFixture.Venue);
            Assert.NotNull(evt.OutrightFixture.Venue);
            Assert.Equal(1, evt.OutrightFixture.Venue.Id);
            Assert.Equal("Test Stadium", evt.OutrightFixture.Venue.Name);
            Assert.Equal(VenueAssignment.Home, evt.OutrightFixture.Venue.Assignment);
        }

        [Fact]
        public void CanAssignAndRetrieveAllProperties_WithNullVenue()
        {
            var sport = new Sport { Id = 1, Name = "Football" };
            var location = new Location { Id = 10, Name = "Stadium" };
            var participant = new Participant { Id = 100, Name = "Team A", Position = "1", RotationId = 5, IsActive = 1 };
            var extraData = new NameValuePair { Name = "Key", Value = "Value" };
            var outrightFixture = new OutrightFixtureSnapshotResponse
            {
                Sport = sport,
                Location = location,
                StartDate = new DateTime(2024, 1, 1),
                LastUpdate = DateTime.UtcNow,
                Status = FixtureStatus.InProgress,
                Participants = new List<Participant> { participant },
                ExtraData = new List<NameValuePair> { extraData },
                Subscription = null,
                Venue = null
            };
            var eventItem = new SnapshotOutrightFixtureEvent
            {
                FixtureId = 123,
                OutrightFixture = outrightFixture
            };
            var response = new GetOutrightFixtureResponse
            {
                Id = 42,
                Name = "Test Fixture",
                Type = 7,
                Events = new List<SnapshotOutrightFixtureEvent> { eventItem }
            };

            Assert.Equal(42, response.Id);
            Assert.Equal("Test Fixture", response.Name);
            Assert.Equal(7, response.Type);
            Assert.NotNull(response.Events);
            var evt = Assert.Single(response.Events);
            Assert.Equal(123, evt.FixtureId);
            Assert.NotNull(evt.OutrightFixture);
            Assert.Equal(sport, evt.OutrightFixture.Sport);
            Assert.Equal(location, evt.OutrightFixture.Location);
            Assert.Equal(new DateTime(2024, 1, 1), evt.OutrightFixture.StartDate);
            Assert.Equal(FixtureStatus.InProgress, evt.OutrightFixture.Status);
            Assert.NotNull(evt.OutrightFixture.Participants);
            Assert.Single(evt.OutrightFixture.Participants);
            Assert.Equal(participant.Name, Assert.Single(evt.OutrightFixture.Participants).Name);
            Assert.NotNull(evt.OutrightFixture.ExtraData);
            Assert.Equal("Key", Assert.Single(evt.OutrightFixture.ExtraData).Name);
            Assert.Null(evt.OutrightFixture.Venue);
        }
    }
} 