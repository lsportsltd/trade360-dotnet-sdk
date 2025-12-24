using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ParticipantTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var fixturePlayers = new List<FixturePlayer>
            {
                new FixturePlayer { PlayerId = 1, ShirtNumber = "10" }
            };
            
            var participant = new Participant
            {
                Id = 5,
                Name = "PlayerName",
                Position = "Forward",
                RotationId = 10,
                IsActive = 1,
                Form = "WWDLW",
                Formation = "4-3-3",
                FixturePlayers = fixturePlayers,
                Gender = 1,
                AgeCategory = 2,
                Type = 3
            };
            Assert.Equal(5, participant.Id);
            Assert.Equal("PlayerName", participant.Name);
            Assert.Equal("Forward", participant.Position);
            Assert.Equal(10, participant.RotationId);
            Assert.Equal(1, participant.IsActive);
            Assert.Equal("WWDLW", participant.Form);
            Assert.Equal("4-3-3", participant.Formation);
            Assert.Equal(fixturePlayers, participant.FixturePlayers);
            Assert.Equal(1, participant.Gender);
            Assert.Equal(2, participant.AgeCategory);
            Assert.Equal(3, participant.Type);
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
            Assert.Null(participant.Form);
            Assert.Null(participant.Formation);
            Assert.Null(participant.FixturePlayers);
            Assert.Null(participant.Gender);
            Assert.Null(participant.AgeCategory);
            Assert.Null(participant.Type);
        }
    }
} 