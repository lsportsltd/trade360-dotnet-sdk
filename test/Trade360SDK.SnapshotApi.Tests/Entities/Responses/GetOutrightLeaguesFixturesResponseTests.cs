using System;
using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightLeaguesFixturesResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var outrightLeague = new OutrightLeagueFixtureSnapshotResponse
            {
                Subscription = null,
                Sport = new Sport { Id = 2, Name = "Basketball" },
                Location = new Location { Id = 20, Name = "Arena" },
                LastUpdate = DateTime.UtcNow,
                Status = FixtureStatus.Finished,
                ExtraData = new List<NameValuePair> { new NameValuePair { Name = "LeagueKey", Value = "LeagueValue" } },
                EndDate = DateTime.UtcNow.AddDays(30)
            };
            var competition = new CompetitionResponse
            {
                FixtureId = 456,
                OutrightLeague = outrightLeague
            };
            var snapshotCompetition = new SnapshotOutrightCompetitionsResponse
            {
                Id = 99,
                Name = "CompetitionName",
                Type = 3,
                Events = new List<CompetitionResponse> { competition }
            };
            var response = new GetOutrightLeaguesFixturesResponse
            {
                Id = 77,
                Name = "LeaguesFixture",
                Type = 5,
                Competitions = new List<SnapshotOutrightCompetitionsResponse> { snapshotCompetition }
            };

            Assert.Equal(77, response.Id);
            Assert.Equal("LeaguesFixture", response.Name);
            Assert.Equal(5, response.Type);
            Assert.NotNull(response.Competitions);
            var comp = Assert.Single(response.Competitions);
            Assert.Equal(99, comp.Id);
            Assert.Equal("CompetitionName", comp.Name);
            Assert.Equal(3, comp.Type);
            var evt = Assert.Single(comp.Events);
            Assert.Equal(456, evt.FixtureId);
            Assert.NotNull(evt.OutrightLeague);
            Assert.Equal(FixtureStatus.Finished, evt.OutrightLeague.Status);
            Assert.Equal("Basketball", evt.OutrightLeague.Sport.Name);
            Assert.Equal("Arena", evt.OutrightLeague.Location.Name);
            Assert.NotNull(evt.OutrightLeague.ExtraData);
            Assert.Equal("LeagueKey", Assert.Single(evt.OutrightLeague.ExtraData).Name);
            Assert.NotNull(evt.OutrightLeague.EndDate);
            Assert.Equal(outrightLeague.EndDate, evt.OutrightLeague.EndDate);
        }
    }
} 