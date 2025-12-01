using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetLiveScoreResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var livescore = new Livescore();
            var response = new GetLiveScoreResponse
            {
                FixtureId = 123,
                Livescore = livescore
            };

            Assert.Equal(123, response.FixtureId);
            Assert.Equal(livescore, response.Livescore);
        }
    }
} 