using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures
{
    public class LocationComprehensiveTests
    {
        [Fact]
        public void Location_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var location = new Location();

            // Assert
            location.Should().NotBeNull();
            location.Id.Should().Be(0);
            location.Name.Should().BeNull();
        }

        [Fact]
        public void Location_SetId_ShouldSetValue()
        {
            // Arrange
            var location = new Location();
            var id = 12345;

            // Act
            location.Id = id;

            // Assert
            location.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999999)]
        [InlineData(12345)]
        public void Location_SetVariousIds_ShouldSetValue(int id)
        {
            // Arrange
            var location = new Location();

            // Act
            location.Id = id;

            // Assert
            location.Id.Should().Be(id);
        }

        [Fact]
        public void Location_SetName_ShouldSetValue()
        {
            // Arrange
            var location = new Location();
            var name = "London";

            // Act
            location.Name = name;

            // Assert
            location.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("London")]
        [InlineData("Manchester")]
        [InlineData("Liverpool")]
        [InlineData("Birmingham")]
        [InlineData("Leeds")]
        [InlineData("Newcastle")]
        [InlineData("Brighton")]
        [InlineData("Southampton")]
        [InlineData("Wolverhampton")]
        public void Location_SetVariousNames_ShouldSetValue(string name)
        {
            // Arrange
            var location = new Location();

            // Act
            location.Name = name;

            // Assert
            location.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("Location with unicode: 伦敦")]
        [InlineData("Location with special chars: @#$%^&*()")]
        [InlineData("Location with numbers: Area 51")]
        [InlineData("Very long location name that exceeds normal expectations for testing purposes and edge cases")]
        [InlineData("Location\nwith\nnewlines")]
        [InlineData("Location\twith\ttabs")]
        [InlineData("Location with accents: São Paulo")]
        [InlineData("Location with umlauts: München")]
        public void Location_SetNamesWithSpecialCharacters_ShouldSetValue(string name)
        {
            // Arrange
            var location = new Location();

            // Act
            location.Name = name;

            // Assert
            location.Name.Should().Be(name);
        }

        [Fact]
        public void Location_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var location = new Location();
            var id = 100;
            var name = "Test Location";

            // Act
            location.Id = id;
            location.Name = name;

            // Assert
            location.Id.Should().Be(id);
            location.Name.Should().Be(name);
        }

        [Fact]
        public void Location_SetNullName_ShouldSetNull()
        {
            // Arrange
            var location = new Location();

            // Act
            location.Name = null;

            // Assert
            location.Name.Should().BeNull();
        }

        [Fact]
        public void Location_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var location = new Location();

            // Act & Assert - Test that we can set and get each property multiple times
            location.Id = 1;
            location.Id.Should().Be(1);
            location.Id = 2;
            location.Id.Should().Be(2);

            location.Name = "Name1";
            location.Name.Should().Be("Name1");
            location.Name = "Name2";
            location.Name.Should().Be("Name2");
            location.Name = null;
            location.Name.Should().BeNull();
        }

        [Fact]
        public void Location_WithRealWorldData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var london = new Location
            {
                Id = 1,
                Name = "London"
            };

            var manchester = new Location
            {
                Id = 2,
                Name = "Manchester"
            };

            var barcelona = new Location
            {
                Id = 3,
                Name = "Barcelona"
            };

            var madrid = new Location
            {
                Id = 4,
                Name = "Madrid"
            };

            // Assert
            london.Id.Should().Be(1);
            london.Name.Should().Be("London");

            manchester.Id.Should().Be(2);
            manchester.Name.Should().Be("Manchester");

            barcelona.Id.Should().Be(3);
            barcelona.Name.Should().Be("Barcelona");

            madrid.Id.Should().Be(4);
            madrid.Name.Should().Be("Madrid");
        }

        [Fact]
        public void Location_WithCountryNames_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var england = new Location { Id = 1, Name = "England" };
            var spain = new Location { Id = 2, Name = "Spain" };
            var italy = new Location { Id = 3, Name = "Italy" };
            var germany = new Location { Id = 4, Name = "Germany" };
            var france = new Location { Id = 5, Name = "France" };

            // Assert
            england.Name.Should().Be("England");
            spain.Name.Should().Be("Spain");
            italy.Name.Should().Be("Italy");
            germany.Name.Should().Be("Germany");
            france.Name.Should().Be("France");
        }

        [Fact]
        public void Location_WithStadiumNames_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var oldTrafford = new Location { Id = 1, Name = "Old Trafford" };
            var anfield = new Location { Id = 2, Name = "Anfield" };
            var emirates = new Location { Id = 3, Name = "Emirates Stadium" };
            var stamfordBridge = new Location { Id = 4, Name = "Stamford Bridge" };

            // Assert
            oldTrafford.Name.Should().Be("Old Trafford");
            anfield.Name.Should().Be("Anfield");
            emirates.Name.Should().Be("Emirates Stadium");
            stamfordBridge.Name.Should().Be("Stamford Bridge");
        }

        [Fact]
        public void Location_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var location = new Location
            {
                Id = 1,
                Name = "Test Location"
            };

            // Act
            var result = location.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Location");
        }

        [Fact]
        public void Location_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var location = new Location
            {
                Id = 1,
                Name = "Test Location"
            };

            // Act
            var hashCode1 = location.GetHashCode();
            var hashCode2 = location.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Location_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var location = new Location();

            // Assert
            location.GetType().Should().Be(typeof(Location));
            location.GetType().Name.Should().Be("Location");
            location.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
        }

        [Fact]
        public void Location_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var location = new Location();

            // Assert
            location.Id.GetType().Should().Be(typeof(int));
            // Name can be null, so we test the property type differently
            var nameProperty = typeof(Location).GetProperty("Name");
            nameProperty.Should().NotBeNull();
            nameProperty!.PropertyType.Should().Be(typeof(string));
        }

        [Fact]
        public void Location_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var location1 = new Location { Id = 1, Name = "Location 1" };
            var location2 = new Location { Id = 2, Name = "Location 2" };

            // Assert
            location1.Id.Should().NotBe(location2.Id);
            location1.Name.Should().NotBe(location2.Name);
            location1.Should().NotBeSameAs(location2);
        }

        [Fact]
        public void Location_WithEmptyStringName_ShouldPreserveEmptyString()
        {
            // Arrange
            var location = new Location();
            var emptyName = "";

            // Act
            location.Name = emptyName;

            // Assert
            location.Name.Should().Be(emptyName);
            location.Name.Should().NotBeNull();
            location.Name.Should().BeEmpty();
        }

        [Fact]
        public void Location_WithWhitespaceOnlyName_ShouldPreserveWhitespace()
        {
            // Arrange
            var location = new Location();
            var whitespaceName = "   ";

            // Act
            location.Name = whitespaceName;

            // Assert
            location.Name.Should().Be(whitespaceName);
            location.Name.Should().NotBeNull();
            location.Name.Should().NotBeEmpty();
        }

        [Fact]
        public void Location_WithInternationalNames_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var tokyo = new Location { Id = 1, Name = "東京" };
            var moscow = new Location { Id = 2, Name = "Москва" };
            var cairo = new Location { Id = 3, Name = "القاهرة" };
            var athens = new Location { Id = 4, Name = "Αθήνα" };

            // Assert
            tokyo.Name.Should().Be("東京");
            moscow.Name.Should().Be("Москва");
            cairo.Name.Should().Be("القاهرة");
            athens.Name.Should().Be("Αθήνα");
        }
    }
} 