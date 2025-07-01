using FluentAssertions;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Livescore
{
    public class CurrentIncidentComprehensiveTests
    {
        [Fact]
        public void CurrentIncident_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Arrange & Act
            var incident = new CurrentIncident();

            // Assert
            incident.Id.Should().Be(0);
            incident.Name.Should().BeNull();
            incident.LastUpdate.Should().Be(default);
        }

        [Fact]
        public void Id_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var incident = new CurrentIncident();
            const long expectedId = 12345L;

            // Act
            incident.Id = expectedId;

            // Assert
            incident.Id.Should().Be(expectedId);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void Id_WithVariousValues_ShouldStoreCorrectly(long id)
        {
            // Arrange
            var incident = new CurrentIncident();

            // Act
            incident.Id = id;

            // Assert
            incident.Id.Should().Be(id);
        }

        [Fact]
        public void Name_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var incident = new CurrentIncident();
            const string expectedName = "Test Incident";

            // Act
            incident.Name = expectedName;

            // Assert
            incident.Name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Short")]
        [InlineData("Very long incident name with special characters !@#$%^&*()")]
        public void Name_WithVariousValues_ShouldStoreCorrectly(string name)
        {
            // Arrange
            var incident = new CurrentIncident();

            // Act
            incident.Name = name;

            // Assert
            incident.Name.Should().Be(name);
        }

        [Fact]
        public void LastUpdate_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var incident = new CurrentIncident();
            var expectedDateTime = DateTime.UtcNow;

            // Act
            incident.LastUpdate = expectedDateTime;

            // Assert
            incident.LastUpdate.Should().Be(expectedDateTime);
        }

        [Theory]
        [MemberData(nameof(GetDateTimeTestData))]
        public void LastUpdate_WithVariousValues_ShouldStoreCorrectly(DateTime dateTime)
        {
            // Arrange
            var incident = new CurrentIncident();

            // Act
            incident.LastUpdate = dateTime;

            // Assert
            incident.LastUpdate.Should().Be(dateTime);
        }

        [Fact]
        public void CurrentIncident_WithAllPropertiesSet_ShouldRetainAllValues()
        {
            // Arrange
            const long expectedId = 999L;
            const string expectedName = "Goal Scored";
            var expectedLastUpdate = new DateTime(2023, 12, 25, 15, 30, 45);

            var incident = new CurrentIncident
            {
                Id = expectedId,
                Name = expectedName,
                LastUpdate = expectedLastUpdate
            };

            // Act & Assert
            incident.Id.Should().Be(expectedId);
            incident.Name.Should().Be(expectedName);
            incident.LastUpdate.Should().Be(expectedLastUpdate);
        }

        [Fact]
        public void CurrentIncident_PropertiesCanBeOverwritten()
        {
            // Arrange
            var incident = new CurrentIncident
            {
                Id = 123L,
                Name = "Initial Name",
                LastUpdate = DateTime.MinValue
            };

            const long newId = 456L;
            const string newName = "Updated Name";
            var newLastUpdate = DateTime.MaxValue;

            // Act
            incident.Id = newId;
            incident.Name = newName;
            incident.LastUpdate = newLastUpdate;

            // Assert
            incident.Id.Should().Be(newId);
            incident.Name.Should().Be(newName);
            incident.LastUpdate.Should().Be(newLastUpdate);
        }

        [Fact]
        public void CurrentIncident_Name_CanBeSetToNull()
        {
            // Arrange
            var incident = new CurrentIncident
            {
                Name = "Some Name"
            };

            // Act
            incident.Name = null;

            // Assert
            incident.Name.Should().BeNull();
        }

        public static IEnumerable<object[]> GetDateTimeTestData()
        {
            yield return new object[] { DateTime.MinValue };
            yield return new object[] { DateTime.MaxValue };
            yield return new object[] { new DateTime(2023, 1, 1) };
            yield return new object[] { new DateTime(2023, 12, 31, 23, 59, 59) };
            yield return new object[] { DateTime.UtcNow };
            yield return new object[] { default(DateTime) };
        }
    }
} 