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
                Id = 42,
                Name = "IncidentName",
                LastUpdate = now,
                Confirmation = IncidentConfirmation.Cancelled
            };
            Assert.Equal(42, incident.Id);
            Assert.Equal("IncidentName", incident.Name);
            Assert.Equal(now, incident.LastUpdate);
            Assert.Equal(IncidentConfirmation.Cancelled, incident.Confirmation);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var incident = new CurrentIncident();
            Assert.Equal(0, incident.Id);
            Assert.Null(incident.Name);
            Assert.Equal(default, incident.LastUpdate);
        }
    }
} 