using System;
using Trade360SDK.Common.Entities.Incidents;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class IncidentTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var now = DateTime.UtcNow;
            var incident = new Incident
            {
                SportId = 1,
                SportName = "Football",
                IncidentId = 100,
                IncidentName = "Goal",
                Description = "Scored a goal",
                LastUpdate = now,
                CreationDate = now.AddMinutes(-5)
            };
            Assert.Equal(1, incident.SportId);
            Assert.Equal("Football", incident.SportName);
            Assert.Equal(100, incident.IncidentId);
            Assert.Equal("Goal", incident.IncidentName);
            Assert.Equal("Scored a goal", incident.Description);
            Assert.Equal(now, incident.LastUpdate);
            Assert.Equal(now.AddMinutes(-5), incident.CreationDate);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var incident = new Incident();
            Assert.Null(incident.SportName);
            Assert.Null(incident.IncidentName);
            Assert.Null(incident.Description);
        }
    }
} 