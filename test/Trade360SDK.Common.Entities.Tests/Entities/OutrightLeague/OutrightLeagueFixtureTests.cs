using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightLeague;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests
{
    public class OutrightLeagueFixtureTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var subscription = new Subscription();
            var sport = new Sport();
            var location = new Location();
            var lastUpdate = DateTime.UtcNow;
            var extraData = new List<NameValuePair> { new NameValuePair { Name = "key", Value = "val" } };
            var fixture = new OutrightLeagueFixture
            {
                Subscription = subscription,
                Sport = sport,
                Location = location,
                LastUpdate = lastUpdate,
                Status = FixtureStatus.Finished,
                ExtraData = extraData
            };
            Assert.Equal(subscription, fixture.Subscription);
            Assert.Equal(sport, fixture.Sport);
            Assert.Equal(location, fixture.Location);
            Assert.Equal(lastUpdate, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.Finished, fixture.Status);
            Assert.Equal(extraData, fixture.ExtraData);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var fixture = new OutrightLeagueFixture();
            Assert.Null(fixture.Subscription);
            Assert.Null(fixture.Sport);
            Assert.Null(fixture.Location);
            Assert.Equal(default, fixture.LastUpdate);
            Assert.Equal(FixtureStatus.NotSet, fixture.Status);
            Assert.Null(fixture.ExtraData);
        }
    }
} 