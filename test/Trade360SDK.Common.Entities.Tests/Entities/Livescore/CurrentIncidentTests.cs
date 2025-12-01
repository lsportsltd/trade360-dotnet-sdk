using System;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class CurrentIncidentTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var now = DateTime.UtcNow;
            var incident = new CurrentIncident
            {
                Id = IncidentType.MissedPenalty,
                Name = "IncidentName",
                LastUpdate = now,
                Confirmation = IncidentConfirmation.Cancelled
            };
            Assert.Equal(IncidentType.MissedPenalty, incident.Id);
            Assert.Equal("IncidentName", incident.Name);
            Assert.Equal(now, incident.LastUpdate);
            Assert.Equal(IncidentConfirmation.Cancelled, incident.Confirmation);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var incident = new CurrentIncident();
            Assert.Null(incident.Id);
            Assert.Null(incident.Name);
            Assert.Equal(default, incident.LastUpdate);
            Assert.Null(incident.Confirmation);
        }
    }
} 