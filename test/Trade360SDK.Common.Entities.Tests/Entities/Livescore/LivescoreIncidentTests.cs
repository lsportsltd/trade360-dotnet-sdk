using System.Collections.Generic;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class LivescoreIncidentTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var results = new List<Result> { new Result() };
            var incident = new Incident
            {
                Period = 1,
                IncidentType = 2,
                Seconds = 45,
                ParticipantPosition = "Forward",
                PlayerId = "P123",
                PlayerName = "John Doe",
                Results = results
            };
            Assert.Equal(1, incident.Period);
            Assert.Equal(2, incident.IncidentType);
            Assert.Equal(45, incident.Seconds);
            Assert.Equal("Forward", incident.ParticipantPosition);
            Assert.Equal("P123", incident.PlayerId);
            Assert.Equal("John Doe", incident.PlayerName);
            Assert.Equal(results, incident.Results);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var incident = new Incident();
            Assert.Null(incident.ParticipantPosition);
            Assert.Null(incident.PlayerId);
            Assert.Null(incident.PlayerName);
            Assert.Null(incident.Results);
            Assert.Equal(0, incident.Period);
            Assert.Equal(0, incident.IncidentType);
            Assert.Equal(0, incident.Seconds);
        }
    }
} 