using System;
using FluentAssertions;
using Trade360SDK.Common.Entities.Incidents;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Incidents
{
    public class IncidentComprehensiveTests
    {
        [Fact]
        public void Incident_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Act
            var incident = new Incident();

            // Assert
            incident.SportId.Should().Be(0);
            incident.SportName.Should().BeNull();
            incident.IncidentId.Should().Be(0);
            incident.IncidentName.Should().BeNull();
            incident.Description.Should().BeNull();
            incident.LastUpdate.Should().Be(default(DateTime));
            incident.CreationDate.Should().Be(default(DateTime));
        }

        [Fact]
        public void Incident_SportId_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedSportId = 123;

            // Act
            incident.SportId = expectedSportId;

            // Assert
            incident.SportId.Should().Be(expectedSportId);
        }

        [Fact]
        public void Incident_SportName_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedSportName = "Football";

            // Act
            incident.SportName = expectedSportName;

            // Assert
            incident.SportName.Should().Be(expectedSportName);
        }

        [Fact]
        public void Incident_IncidentId_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedIncidentId = 456;

            // Act
            incident.IncidentId = expectedIncidentId;

            // Assert
            incident.IncidentId.Should().Be(expectedIncidentId);
        }

        [Fact]
        public void Incident_IncidentName_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedIncidentName = "Goal Scored";

            // Act
            incident.IncidentName = expectedIncidentName;

            // Assert
            incident.IncidentName.Should().Be(expectedIncidentName);
        }

        [Fact]
        public void Incident_Description_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedDescription = "Player scored a goal in the 45th minute";

            // Act
            incident.Description = expectedDescription;

            // Assert
            incident.Description.Should().Be(expectedDescription);
        }

        [Fact]
        public void Incident_LastUpdate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedLastUpdate = new DateTime(2023, 12, 25, 15, 30, 45);

            // Act
            incident.LastUpdate = expectedLastUpdate;

            // Assert
            incident.LastUpdate.Should().Be(expectedLastUpdate);
        }

        [Fact]
        public void Incident_CreationDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var expectedCreationDate = new DateTime(2023, 12, 25, 14, 0, 0);

            // Act
            incident.CreationDate = expectedCreationDate;

            // Assert
            incident.CreationDate.Should().Be(expectedCreationDate);
        }

        [Fact]
        public void Incident_WithCompleteData_ShouldSetAllProperties()
        {
            // Arrange
            var incident = new Incident();
            var sportId = 1;
            var sportName = "Soccer";
            var incidentId = 101;
            var incidentName = "Red Card";
            var description = "Player received red card for dangerous play";
            var lastUpdate = DateTime.UtcNow;
            var creationDate = DateTime.UtcNow.AddMinutes(-5);

            // Act
            incident.SportId = sportId;
            incident.SportName = sportName;
            incident.IncidentId = incidentId;
            incident.IncidentName = incidentName;
            incident.Description = description;
            incident.LastUpdate = lastUpdate;
            incident.CreationDate = creationDate;

            // Assert
            incident.SportId.Should().Be(sportId);
            incident.SportName.Should().Be(sportName);
            incident.IncidentId.Should().Be(incidentId);
            incident.IncidentName.Should().Be(incidentName);
            incident.Description.Should().Be(description);
            incident.LastUpdate.Should().Be(lastUpdate);
            incident.CreationDate.Should().Be(creationDate);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Basketball")]
        [InlineData("American Football")]
        [InlineData("Tennis")]
        public void Incident_SportName_ShouldHandleVariousStrings(string sportName)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.SportName = sportName;

            // Assert
            incident.SportName.Should().Be(sportName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Penalty")]
        [InlineData("Substitution")]
        [InlineData("Timeout")]
        public void Incident_IncidentName_ShouldHandleVariousStrings(string incidentName)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.IncidentName = incidentName;

            // Assert
            incident.IncidentName.Should().Be(incidentName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Short description")]
        [InlineData("A very long description that contains multiple sentences and detailed information about what happened during the incident")]
        public void Incident_Description_ShouldHandleVariousStrings(string description)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.Description = description;

            // Assert
            incident.Description.Should().Be(description);
        }

        [Fact]
        public void Incident_SportId_ShouldHandleNegativeValues()
        {
            // Arrange
            var incident = new Incident();
            var negativeSportId = -1;

            // Act
            incident.SportId = negativeSportId;

            // Assert
            incident.SportId.Should().Be(negativeSportId);
        }

        [Fact]
        public void Incident_IncidentId_ShouldHandleNegativeValues()
        {
            // Arrange
            var incident = new Incident();
            var negativeIncidentId = -1;

            // Act
            incident.IncidentId = negativeIncidentId;

            // Assert
            incident.IncidentId.Should().Be(negativeIncidentId);
        }

        [Fact]
        public void Incident_DateTimes_ShouldHandleMinMaxValues()
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.LastUpdate = DateTime.MinValue;
            incident.CreationDate = DateTime.MaxValue;

            // Assert
            incident.LastUpdate.Should().Be(DateTime.MinValue);
            incident.CreationDate.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void Incident_DateTimes_ShouldHandleUtcValues()
        {
            // Arrange
            var incident = new Incident();
            var utcNow = DateTime.UtcNow;

            // Act
            incident.LastUpdate = utcNow;
            incident.CreationDate = utcNow;

            // Assert
            incident.LastUpdate.Should().Be(utcNow);
            incident.CreationDate.Should().Be(utcNow);
        }

        [Fact]
        public void Incident_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var unicodeSportName = "足球";
            var unicodeIncidentName = "进球";
            var unicodeDescription = "球员在第45分钟进球";

            // Act
            incident.SportName = unicodeSportName;
            incident.IncidentName = unicodeIncidentName;
            incident.Description = unicodeDescription;

            // Assert
            incident.SportName.Should().Be(unicodeSportName);
            incident.IncidentName.Should().Be(unicodeIncidentName);
            incident.Description.Should().Be(unicodeDescription);
        }

        [Fact]
        public void Incident_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var specialCharacters = "!@#$%^&*()_+-=[]{}|;':\",./<>?";

            // Act
            incident.SportName = specialCharacters;
            incident.IncidentName = specialCharacters;
            incident.Description = specialCharacters;

            // Assert
            incident.SportName.Should().Be(specialCharacters);
            incident.IncidentName.Should().Be(specialCharacters);
            incident.Description.Should().Be(specialCharacters);
        }

        [Fact]
        public void Incident_PropertyAssignment_ShouldAllowNullValues()
        {
            // Arrange
            var incident = new Incident
            {
                SportName = "Initial Sport",
                IncidentName = "Initial Incident",
                Description = "Initial Description"
            };

            // Act
            incident.SportName = null;
            incident.IncidentName = null;
            incident.Description = null;

            // Assert
            incident.SportName.Should().BeNull();
            incident.IncidentName.Should().BeNull();
            incident.Description.Should().BeNull();
        }
    }
} 