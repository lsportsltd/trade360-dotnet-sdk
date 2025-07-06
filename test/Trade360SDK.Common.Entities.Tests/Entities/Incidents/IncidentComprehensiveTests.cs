using System;
using FluentAssertions;
using Trade360SDK.Common.Entities.Incidents;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Incidents
{
    public class IncidentComprehensiveTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var incident = new Incident();

            // Assert
            incident.SportId.Should().Be(default(int));
            incident.SportName.Should().BeNull();
            incident.IncidentId.Should().Be(default(int));
            incident.IncidentName.Should().BeNull();
            incident.Description.Should().BeNull();
            incident.LastUpdate.Should().Be(default(DateTime));
            incident.CreationDate.Should().Be(default(DateTime));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SportId_WithVariousValues_ShouldStoreCorrectly(int sportId)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.SportId = sportId;

            // Assert
            incident.SportId.Should().Be(sportId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Football")]
        [InlineData("Basketball")]
        [InlineData("Tennis")]
        [InlineData("Very Long Sport Name That Contains Many Characters")]
        [InlineData("Sport123")]
        [InlineData("Special@Sport!")]
        public void SportName_WithValidStrings_ShouldStoreCorrectly(string sportName)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.SportName = sportName;

            // Assert
            incident.SportName.Should().Be(sportName);
        }

        [Fact]
        public void SportName_WithNull_ShouldAcceptValue()
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.SportName = null;

            // Assert
            incident.SportName.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(-5)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void IncidentId_WithVariousValues_ShouldStoreCorrectly(int incidentId)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.IncidentId = incidentId;

            // Assert
            incident.IncidentId.Should().Be(incidentId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Match Started")]
        [InlineData("Goal Scored")]
        [InlineData("Red Card")]
        [InlineData("Half Time")]
        [InlineData("Very Long Incident Name With Many Details")]
        public void IncidentName_WithValidStrings_ShouldStoreCorrectly(string incidentName)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.IncidentName = incidentName;

            // Assert
            incident.IncidentName.Should().Be(incidentName);
        }

        [Fact]
        public void IncidentName_WithNull_ShouldAcceptValue()
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.IncidentName = null;

            // Assert
            incident.IncidentName.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Simple description")]
        [InlineData("Very detailed description of the incident that happened during the match")]
        [InlineData("Special Characters: @#$%^&*()")]
        [InlineData("Unicode: 测试描述")]
        public void Description_WithValidStrings_ShouldStoreCorrectly(string description)
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.Description = description;

            // Assert
            incident.Description.Should().Be(description);
        }

        [Fact]
        public void Description_WithNull_ShouldAcceptValue()
        {
            // Arrange
            var incident = new Incident();

            // Act
            incident.Description = null;

            // Assert
            incident.Description.Should().BeNull();
        }

        [Fact]
        public void LastUpdate_WithCurrentDateTime_ShouldStoreCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var currentTime = DateTime.Now;

            // Act
            incident.LastUpdate = currentTime;

            // Assert
            incident.LastUpdate.Should().Be(currentTime);
        }

        [Fact]
        public void CreationDate_WithCurrentDateTime_ShouldStoreCorrectly()
        {
            // Arrange
            var incident = new Incident();
            var currentTime = DateTime.Now;

            // Act
            incident.CreationDate = currentTime;

            // Assert
            incident.CreationDate.Should().Be(currentTime);
        }

        [Theory]
        [InlineData("2023-01-01")]
        [InlineData("2025-12-31")]
        [InlineData("1990-06-15")]
        public void LastUpdate_WithSpecificDates_ShouldStoreCorrectly(string dateString)
        {
            // Arrange
            var incident = new Incident();
            var specificDate = DateTime.Parse(dateString);

            // Act
            incident.LastUpdate = specificDate;

            // Assert
            incident.LastUpdate.Should().Be(specificDate);
        }

        [Theory]
        [InlineData("2022-03-15")]
        [InlineData("2024-08-20")]
        [InlineData("2000-01-01")]
        public void CreationDate_WithSpecificDates_ShouldStoreCorrectly(string dateString)
        {
            // Arrange
            var incident = new Incident();
            var specificDate = DateTime.Parse(dateString);

            // Act
            incident.CreationDate = specificDate;

            // Assert
            incident.CreationDate.Should().Be(specificDate);
        }

        [Fact]
        public void Properties_CanBeSetIndependently()
        {
            // Arrange
            var incident = new Incident();
            const int expectedSportId = 1;
            const string expectedSportName = "Football";
            const int expectedIncidentId = 123;
            const string expectedIncidentName = "Goal";
            const string expectedDescription = "Amazing goal!";
            var expectedLastUpdate = new DateTime(2023, 6, 15, 14, 30, 0);
            var expectedCreationDate = new DateTime(2023, 6, 15, 14, 25, 0);

            // Act
            incident.SportId = expectedSportId;
            incident.SportName = expectedSportName;
            incident.IncidentId = expectedIncidentId;
            incident.IncidentName = expectedIncidentName;
            incident.Description = expectedDescription;
            incident.LastUpdate = expectedLastUpdate;
            incident.CreationDate = expectedCreationDate;

            // Assert
            incident.SportId.Should().Be(expectedSportId);
            incident.SportName.Should().Be(expectedSportName);
            incident.IncidentId.Should().Be(expectedIncidentId);
            incident.IncidentName.Should().Be(expectedIncidentName);
            incident.Description.Should().Be(expectedDescription);
            incident.LastUpdate.Should().Be(expectedLastUpdate);
            incident.CreationDate.Should().Be(expectedCreationDate);
        }

        [Fact]
        public void ObjectInitializer_ShouldSetAllProperties()
        {
            // Arrange
            const int expectedSportId = 2;
            const string expectedSportName = "Basketball";
            const int expectedIncidentId = 456;
            const string expectedIncidentName = "Timeout";
            const string expectedDescription = "Team timeout called";
            var expectedLastUpdate = new DateTime(2023, 7, 20, 16, 45, 30);
            var expectedCreationDate = new DateTime(2023, 7, 20, 16, 40, 15);

            // Act
            var incident = new Incident
            {
                SportId = expectedSportId,
                SportName = expectedSportName,
                IncidentId = expectedIncidentId,
                IncidentName = expectedIncidentName,
                Description = expectedDescription,
                LastUpdate = expectedLastUpdate,
                CreationDate = expectedCreationDate
            };

            // Assert
            incident.SportId.Should().Be(expectedSportId);
            incident.SportName.Should().Be(expectedSportName);
            incident.IncidentId.Should().Be(expectedIncidentId);
            incident.IncidentName.Should().Be(expectedIncidentName);
            incident.Description.Should().Be(expectedDescription);
            incident.LastUpdate.Should().Be(expectedLastUpdate);
            incident.CreationDate.Should().Be(expectedCreationDate);
        }

        [Fact]
        public void Properties_CanBeOverwritten()
        {
            // Arrange
            var incident = new Incident
            {
                SportId = 1,
                SportName = "Tennis",
                IncidentId = 789,
                IncidentName = "Serve",
                Description = "First serve",
                LastUpdate = DateTime.MinValue,
                CreationDate = DateTime.MinValue
            };

            const int newSportId = 5;
            const string newSportName = "Hockey";
            const int newIncidentId = 999;
            const string newIncidentName = "Goal";
            const string newDescription = "Power play goal";
            var newLastUpdate = DateTime.MaxValue;
            var newCreationDate = DateTime.MaxValue;

            // Act
            incident.SportId = newSportId;
            incident.SportName = newSportName;
            incident.IncidentId = newIncidentId;
            incident.IncidentName = newIncidentName;
            incident.Description = newDescription;
            incident.LastUpdate = newLastUpdate;
            incident.CreationDate = newCreationDate;

            // Assert
            incident.SportId.Should().Be(newSportId);
            incident.SportName.Should().Be(newSportName);
            incident.IncidentId.Should().Be(newIncidentId);
            incident.IncidentName.Should().Be(newIncidentName);
            incident.Description.Should().Be(newDescription);
            incident.LastUpdate.Should().Be(newLastUpdate);
            incident.CreationDate.Should().Be(newCreationDate);
        }

        [Fact]
        public void DateTimeProperties_WithMinAndMaxValues_ShouldStoreCorrectly()
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
        public void ToString_ShouldNotThrow()
        {
            // Arrange
            var incident = new Incident
            {
                SportId = 3,
                SportName = "Baseball",
                IncidentId = 321,
                IncidentName = "Home Run",
                Description = "Grand slam!",
                LastUpdate = DateTime.Now,
                CreationDate = DateTime.Today
            };

            // Act & Assert
            var act = () => incident.ToString();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetHashCode_ShouldNotThrow()
        {
            // Arrange
            var incident = new Incident
            {
                SportId = 4,
                SportName = "Soccer",
                IncidentId = 654,
                IncidentName = "Corner Kick",
                Description = "Left corner",
                LastUpdate = DateTime.UtcNow,
                CreationDate = DateTime.Today.AddHours(-2)
            };

            // Act & Assert
            var act = () => incident.GetHashCode();
            act.Should().NotThrow();
        }

        [Fact]
        public void Equals_WithSameReference_ShouldReturnTrue()
        {
            // Arrange
            var incident = new Incident
            {
                SportId = 6,
                SportName = "Volleyball",
                IncidentId = 987,
                IncidentName = "Spike",
                Description = "Winning spike",
                LastUpdate = DateTime.Now,
                CreationDate = DateTime.Today
            };

            // Act & Assert
            incident.Equals(incident).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var incident = new Incident
            {
                SportId = 7,
                SportName = "Cricket",
                IncidentId = 111,
                IncidentName = "Wicket",
                Description = "Clean bowled",
                LastUpdate = DateTime.Now,
                CreationDate = DateTime.Today
            };

            // Act & Assert
            incident.Equals(null).Should().BeFalse();
        }
    }
} 