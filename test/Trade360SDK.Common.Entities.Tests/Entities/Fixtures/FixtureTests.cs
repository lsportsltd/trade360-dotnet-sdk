using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class FixtureTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var sport = new Sport();
            var location = new Location();
            var league = new League();
            var startDate = DateTime.UtcNow;
            var lastUpdate = DateTime.UtcNow.AddMinutes(-1);
            var participants = new List<Participant> { new Participant() };
            var extraData = new List<NameValuePair> { new NameValuePair() };
            var subscription = new Subscription();
            var fixture = new Fixture
            {
                Sport = sport,
                Location = location,
                League = league,
                StartDate = startDate,
                LastUpdate = lastUpdate,
                Status = FixtureStatus.Finished,
                Participants = participants,
                FixtureExtraData = extraData,
                ExternalFixtureId = "EXT123",
                Subscription = subscription
            };
            Assert.Equal(sport, fixture.Sport);
            Assert.Equal(location, fixture.Location);
            Assert.Equal(league, fixture.League);
            Assert.Equal(startDate, fixture.StartDate);
            Assert.Equal(lastUpdate, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.Finished, fixture.Status);
            Assert.Equal(participants, fixture.Participants);
            Assert.Equal(extraData, fixture.FixtureExtraData);
            Assert.Equal("EXT123", fixture.ExternalFixtureId);
            Assert.Equal(subscription, fixture.Subscription);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var fixture = new Fixture();
            Assert.Null(fixture.Sport);
            Assert.Null(fixture.Location);
            Assert.Null(fixture.League);
            Assert.Null(fixture.StartDate);
            Assert.Equal(default, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.NotSet, fixture.Status);
            Assert.Null(fixture.Participants);
            Assert.Null(fixture.FixtureExtraData);
            Assert.Null(fixture.ExternalFixtureId);
            Assert.Null(fixture.Subscription);
        }
    }
} 