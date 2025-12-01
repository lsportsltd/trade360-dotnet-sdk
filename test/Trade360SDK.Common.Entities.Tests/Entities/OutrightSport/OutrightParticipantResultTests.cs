using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightParticipantResultTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var result = new OutrightParticipantResult
            {
                ParticipantId = 1,
                Name = "Participant",
                Result = 2
            };
            Assert.Equal(1, result.ParticipantId);
            Assert.Equal("Participant", result.Name);
            Assert.Equal(2, result.Result);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var result = new OutrightParticipantResult();
            Assert.Null(result.Name);
        }
    }
} 