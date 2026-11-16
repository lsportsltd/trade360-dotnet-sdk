using System.Text.Json;
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

        [Fact]
        public void Deserialize_GetLivescoreAndGetEvents_KeepStoppageTimeSeconds()
        {
            const string withStoppage = """
                {"FixtureId":10,"Livescore":{"Scoreboard":{"Clock":{"Seconds":5400,"StoppageTimeSeconds":300}}}}
                """;
            const string cleared = """
                {"FixtureId":10,"Livescore":{"Scoreboard":{"Clock":{"Seconds":5400}}}}
                """;

            var livescore = JsonSerializer.Deserialize<GetLiveScoreResponse>(withStoppage);
            var events = JsonSerializer.Deserialize<GetEventsResponse>(withStoppage);
            var clearedLivescore = JsonSerializer.Deserialize<GetLiveScoreResponse>(cleared);

            Assert.Equal(300, livescore!.Livescore!.Scoreboard!.Clock!.StoppageTimeSeconds);
            Assert.Equal(300, events!.Livescore!.Scoreboard!.Clock!.StoppageTimeSeconds);
            Assert.Null(clearedLivescore!.Livescore!.Scoreboard!.Clock!.StoppageTimeSeconds);
        }
    }
} 