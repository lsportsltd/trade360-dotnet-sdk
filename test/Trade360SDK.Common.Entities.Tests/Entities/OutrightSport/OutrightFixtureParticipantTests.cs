using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightFixtureParticipantTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var extraData = new List<NameValuePair> { new NameValuePair { Name = "key", Value = "val" } };
            var fixturePlayers = new List<FixturePlayer> { new FixturePlayer { PlayerId = 10 } };
            var participant = new OutrightFixtureParticipant
            {
                Id = 5,
                Name = "ParticipantName",
                Position = "Forward",
                RotationId = 10,
                IsActive = 1,
                Form = "WWDLW",
                Formation = "4-3-3",
                FixturePlayers = fixturePlayers,
                Gender = 1,
                AgeCategory = 21,
                Type = 1,
                ExtraData = extraData
            };
            Assert.Equal(5, participant.Id);
            Assert.Equal("ParticipantName", participant.Name);
            Assert.Equal("Forward", participant.Position);
            Assert.Equal(10, participant.RotationId);
            Assert.Equal(1, participant.IsActive);
            Assert.Equal("WWDLW", participant.Form);
            Assert.Equal("4-3-3", participant.Formation);
            Assert.Equal(fixturePlayers, participant.FixturePlayers);
            Assert.Equal(1, participant.Gender);
            Assert.Equal(21, participant.AgeCategory);
            Assert.Equal(1, participant.Type);
            Assert.Equal(extraData, participant.ExtraData);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var participant = new OutrightFixtureParticipant();
            Assert.Null(participant.Name);
            Assert.Null(participant.Position);
            Assert.Null(participant.RotationId);
            Assert.Null(participant.Form);
            Assert.Null(participant.Formation);
            Assert.Null(participant.FixturePlayers);
            Assert.Null(participant.Gender);
            Assert.Null(participant.AgeCategory);
            Assert.Null(participant.Type);
            Assert.Null(participant.ExtraData);
            Assert.Equal(-1, participant.IsActive); // default value
        }

        [Fact]
        public void Form_ShouldGetAndSetValue()
        {
            var participant = new OutrightFixtureParticipant();
            participant.Form = "WWWWW";
            Assert.Equal("WWWWW", participant.Form);

            participant.Form = null;
            Assert.Null(participant.Form);
        }

        [Fact]
        public void Formation_ShouldGetAndSetValue()
        {
            var participant = new OutrightFixtureParticipant();
            participant.Formation = "4-4-2";
            Assert.Equal("4-4-2", participant.Formation);

            participant.Formation = "3-5-2";
            Assert.Equal("3-5-2", participant.Formation);
        }

        [Fact]
        public void FixturePlayers_ShouldGetAndSetValue()
        {
            var participant = new OutrightFixtureParticipant();
            var player1 = new FixturePlayer { PlayerId = 10, ShirtNumber = "10", IsCaptain = true };
            var player2 = new FixturePlayer { PlayerId = 7, ShirtNumber = "7", IsCaptain = false };
            var fixturePlayers = new List<FixturePlayer> { player1, player2 };

            participant.FixturePlayers = fixturePlayers;

            Assert.Equal(fixturePlayers, participant.FixturePlayers);
            Assert.Equal(2, ((List<FixturePlayer>)participant.FixturePlayers).Count);
        }

        [Fact]
        public void GenderAgeCategoryType_ShouldGetAndSetValues()
        {
            var participant = new OutrightFixtureParticipant();
            participant.Gender = 1;
            participant.AgeCategory = 18;
            participant.Type = 2;

            Assert.Equal(1, participant.Gender);
            Assert.Equal(18, participant.AgeCategory);
            Assert.Equal(2, participant.Type);
        }
    }
} 