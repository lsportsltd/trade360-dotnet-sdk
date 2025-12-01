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
            var fixture = new OutrightFixture
            {
                Sport = sport,
                Location = location,
                StartDate = startDate,
                LastUpdate = lastUpdate,
                Status = FixtureStatus.Finished,
                Participants = participants,
                ExtraData = extraData,
                Subscription = subscription,
                Venue = venue
            };
            Assert.Equal(sport, fixture.Sport);
            Assert.Equal(location, fixture.Location);
            Assert.Equal(startDate, fixture.StartDate);
            Assert.Equal(lastUpdate, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.Finished, fixture.Status);
            Assert.Equal(participants, fixture.Participants);
            Assert.Equal(extraData, fixture.ExtraData);
            Assert.Equal(subscription, fixture.Subscription);
            Assert.Equal(venue, fixture.Venue);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var fixture = new OutrightFixture();
            Assert.Null(fixture.Sport);
            Assert.Null(fixture.Location);
            Assert.Null(fixture.StartDate);
            Assert.Equal(default, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.NotSet, fixture.Status);
            Assert.Null(fixture.Participants);
            Assert.Null(fixture.ExtraData);
            Assert.Null(fixture.Subscription);
            Assert.Null(fixture.Venue);
        }
    }
} 