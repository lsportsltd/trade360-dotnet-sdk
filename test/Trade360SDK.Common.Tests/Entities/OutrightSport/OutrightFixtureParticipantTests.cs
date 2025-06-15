using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightSport;
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
            var participant = new OutrightFixtureParticipant
            {
                Id = 5,
                Name = "ParticipantName",
                Position = "Forward",
                RotationId = 10,
                IsActive = 1,
                ExtraData = extraData
            };
            Assert.Equal(5, participant.Id);
            Assert.Equal("ParticipantName", participant.Name);
            Assert.Equal("Forward", participant.Position);
            Assert.Equal(10, participant.RotationId);
            Assert.Equal(1, participant.IsActive);
            Assert.Equal(extraData, participant.ExtraData);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var participant = new OutrightFixtureParticipant();
            Assert.Null(participant.Name);
            Assert.Null(participant.Position);
            Assert.Null(participant.RotationId);
            Assert.Null(participant.ExtraData);
            Assert.Equal(-1, participant.IsActive); // default value
        }
    }
} 