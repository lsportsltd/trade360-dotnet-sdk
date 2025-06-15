using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ParticipantTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var participant = new Participant
            {
                Id = 5,
                Name = "PlayerName",
                Position = "Forward",
                RotationId = 10,
                IsActive = 1
            };
            Assert.Equal(5, participant.Id);
            Assert.Equal("PlayerName", participant.Name);
            Assert.Equal("Forward", participant.Position);
            Assert.Equal(10, participant.RotationId);
            Assert.Equal(1, participant.IsActive);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var participant = new Participant();
            Assert.Equal(0, participant.Id);
            Assert.Null(participant.Name);
            Assert.Null(participant.Position);
            Assert.Null(participant.RotationId);
            Assert.Equal(-1, participant.IsActive);
        }
    }
} 