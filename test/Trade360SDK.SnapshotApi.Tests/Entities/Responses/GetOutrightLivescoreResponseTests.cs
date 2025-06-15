using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightLivescoreResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var outrightScore = new OutrightLivescoreScore { Status = Trade360SDK.Common.Entities.Enums.FixtureStatus.Finished, ParticipantResults = null };
            var eventItem = new OutrightScoreEventResponse { FixtureId = 555, OutrightScore = outrightScore };
            var response = new GetOutrightLivescoreResponse
            {
                Id = 444,
                Name = "LivescoreResp",
                Type = 9,
                Events = new List<OutrightScoreEventResponse> { eventItem }
            };

            Assert.Equal(444, response.Id);
            Assert.Equal("LivescoreResp", response.Name);
            Assert.Equal(9, response.Type);
            Assert.NotNull(response.Events);
            var evt = Assert.Single(response.Events);
            Assert.Equal(555, evt.FixtureId);
            Assert.NotNull(evt.OutrightScore);
            Assert.Equal(Trade360SDK.Common.Entities.Enums.FixtureStatus.Finished, evt.OutrightScore.Status);
        }
    }
} 