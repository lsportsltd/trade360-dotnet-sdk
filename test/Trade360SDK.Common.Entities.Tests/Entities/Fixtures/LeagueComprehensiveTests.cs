using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures
{
    public class LeagueComprehensiveTests
    {
        [Fact]
        public void League_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var league = new League();

            // Assert
            league.Should().NotBeNull();
            league.Id.Should().Be(0);
            league.Name.Should().BeNull();
            league.Tour.Should().BeNull();
            league.AgeCategory.Should().BeNull();
            league.Gender.Should().BeNull();
            league.Type.Should().BeNull();
            league.NumberOfPeriods.Should().BeNull();
            league.SportCategory.Should().BeNull();
        }

        [Fact]
        public void League_SetId_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var id = 12345;

            // Act
            league.Id = id;

            // Assert
            league.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999999)]
        [InlineData(12345)]
        public void League_SetVariousIds_ShouldSetValue(int id)
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = id;

            // Assert
            league.Id.Should().Be(id);
        }

        [Fact]
        public void League_SetName_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var name = "Premier League";

            // Act
            league.Name = name;

            // Assert
            league.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Premier League")]
        [InlineData("La Liga")]
        [InlineData("Serie A")]
        [InlineData("Bundesliga")]
        [InlineData("Ligue 1")]
        [InlineData("Champions League")]
        [InlineData("Europa League")]
        [InlineData("World Cup")]
        [InlineData("UEFA Nations League")]
        public void League_SetVariousNames_ShouldSetValue(string name)
        {
            // Arrange
            var league = new League();

            // Act
            league.Name = name;

            // Assert
            league.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("League with unicode: 英超联赛")]
        [InlineData("League with special chars: @#$%^&*()")]
        [InlineData("League with numbers: 2023-24 Season")]
        [InlineData("Very long league name that exceeds normal expectations for testing purposes and edge cases")]
        [InlineData("League\nwith\nnewlines")]
        [InlineData("League\twith\ttabs")]
        public void League_SetNamesWithSpecialCharacters_ShouldSetValue(string name)
        {
            // Arrange
            var league = new League();

            // Act
            league.Name = name;

            // Assert
            league.Name.Should().Be(name);
        }

        [Fact]
        public void League_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var league = new League();
            var id = 100;
            var name = "Test League";

            // Act
            league.Id = id;
            league.Name = name;

            // Assert
            league.Id.Should().Be(id);
            league.Name.Should().Be(name);
        }

        [Fact]
        public void League_SetNullName_ShouldSetNull()
        {
            // Arrange
            var league = new League();

            // Act
            league.Name = null;

            // Assert
            league.Name.Should().BeNull();
        }

        [Fact]
        public void League_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var league = new League();

            // Act & Assert - Test that we can set and get each property multiple times
            league.Id = 1;
            league.Id.Should().Be(1);
            league.Id = 2;
            league.Id.Should().Be(2);

            league.Name = "Name1";
            league.Name.Should().Be("Name1");
            league.Name = "Name2";
            league.Name.Should().Be("Name2");
            league.Name = null;
            league.Name.Should().BeNull();
        }

        [Fact]
        public void League_WithRealWorldData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var premierLeague = new League
            {
                Id = 1,
                Name = "Premier League"
            };

            var championsLeague = new League
            {
                Id = 2,
                Name = "UEFA Champions League"
            };

            var worldCup = new League
            {
                Id = 3,
                Name = "FIFA World Cup 2024"
            };

            // Assert
            premierLeague.Id.Should().Be(1);
            premierLeague.Name.Should().Be("Premier League");

            championsLeague.Id.Should().Be(2);
            championsLeague.Name.Should().Be("UEFA Champions League");

            worldCup.Id.Should().Be(3);
            worldCup.Name.Should().Be("FIFA World Cup 2024");
        }

        [Fact]
        public void League_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var league = new League
            {
                Id = 1,
                Name = "Test League"
            };

            // Act
            var result = league.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("League");
        }

        [Fact]
        public void League_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var league = new League
            {
                Id = 1,
                Name = "Test League"
            };

            // Act
            var hashCode1 = league.GetHashCode();
            var hashCode2 = league.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void League_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var league = new League();

            // Assert
            league.GetType().Should().Be(typeof(League));
            league.GetType().Name.Should().Be("League");
            league.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
        }

        [Fact]
        public void League_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var league = new League();

            // Assert
            league.Id.GetType().Should().Be(typeof(int));
            // Name can be null, so we test the property type differently
            var nameProperty = typeof(League).GetProperty("Name");
            nameProperty.Should().NotBeNull();
            nameProperty!.PropertyType.Should().Be(typeof(string));
        }

        [Fact]
        public void League_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var league1 = new League { Id = 1, Name = "League 1" };
            var league2 = new League { Id = 2, Name = "League 2" };

            // Assert
            league1.Id.Should().NotBe(league2.Id);
            league1.Name.Should().NotBe(league2.Name);
            league1.Should().NotBeSameAs(league2);
        }

        [Fact]
        public void League_WithEmptyStringName_ShouldPreserveEmptyString()
        {
            // Arrange
            var league = new League();
            var emptyName = "";

            // Act
            league.Name = emptyName;

            // Assert
            league.Name.Should().Be(emptyName);
            league.Name.Should().NotBeNull();
            league.Name.Should().BeEmpty();
        }

        [Fact]
        public void League_WithWhitespaceOnlyName_ShouldPreserveWhitespace()
        {
            // Arrange
            var league = new League();
            var whitespaceName = "   ";

            // Act
            league.Name = whitespaceName;

            // Assert
            league.Name.Should().Be(whitespaceName);
            league.Name.Should().NotBeNull();
            league.Name.Should().NotBeEmpty();
        }

        [Fact]
        public void League_SetTour_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var tour = new IdNamePair { Id = 1, Name = "World Tour" };

            // Act
            league.Tour = tour;

            // Assert
            league.Tour.Should().Be(tour);
            league.Tour.Id.Should().Be(1);
            league.Tour.Name.Should().Be("World Tour");
        }

        [Fact]
        public void League_SetAgeCategory_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var ageCategory = 21;

            // Act
            league.AgeCategory = ageCategory;

            // Assert
            league.AgeCategory.Should().Be(ageCategory);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(18)]
        [InlineData(21)]
        [InlineData(23)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void League_SetVariousAgeCategories_ShouldSetValue(int ageCategory)
        {
            // Arrange
            var league = new League();

            // Act
            league.AgeCategory = ageCategory;

            // Assert
            league.AgeCategory.Should().Be(ageCategory);
        }

        [Fact]
        public void League_SetGender_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var gender = 1;

            // Act
            league.Gender = gender;

            // Assert
            league.Gender.Should().Be(gender);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-1)]
        public void League_SetVariousGenders_ShouldSetValue(int gender)
        {
            // Arrange
            var league = new League();

            // Act
            league.Gender = gender;

            // Assert
            league.Gender.Should().Be(gender);
        }

        [Fact]
        public void League_SetType_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var type = 1;

            // Act
            league.Type = type;

            // Assert
            league.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(-1)]
        public void League_SetVariousTypes_ShouldSetValue(int type)
        {
            // Arrange
            var league = new League();

            // Act
            league.Type = type;

            // Assert
            league.Type.Should().Be(type);
        }

        [Fact]
        public void League_SetNumberOfPeriods_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var numberOfPeriods = 2;

            // Act
            league.NumberOfPeriods = numberOfPeriods;

            // Assert
            league.NumberOfPeriods.Should().Be(numberOfPeriods);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        public void League_SetVariousNumberOfPeriods_ShouldSetValue(int numberOfPeriods)
        {
            // Arrange
            var league = new League();

            // Act
            league.NumberOfPeriods = numberOfPeriods;

            // Assert
            league.NumberOfPeriods.Should().Be(numberOfPeriods);
        }

        [Fact]
        public void League_SetSportCategory_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var sportCategory = new IdNamePair { Id = 1, Name = "Team Sports" };

            // Act
            league.SportCategory = sportCategory;

            // Assert
            league.SportCategory.Should().Be(sportCategory);
            league.SportCategory.Id.Should().Be(1);
            league.SportCategory.Name.Should().Be("Team Sports");
        }

        [Fact]
        public void League_SetAllNewProperties_ShouldSetAllValues()
        {
            // Arrange
            var league = new League();
            var tour = new IdNamePair { Id = 1, Name = "Champions Tour" };
            var ageCategory = 18;
            var gender = 1;
            var type = 2;
            var numberOfPeriods = 4;
            var sportCategory = new IdNamePair { Id = 3, Name = "Individual Sports" };

            // Act
            league.Tour = tour;
            league.AgeCategory = ageCategory;
            league.Gender = gender;
            league.Type = type;
            league.NumberOfPeriods = numberOfPeriods;
            league.SportCategory = sportCategory;

            // Assert
            league.Tour.Should().Be(tour);
            league.AgeCategory.Should().Be(ageCategory);
            league.Gender.Should().Be(gender);
            league.Type.Should().Be(type);
            league.NumberOfPeriods.Should().Be(numberOfPeriods);
            league.SportCategory.Should().Be(sportCategory);
        }

        [Fact]
        public void League_SetNullNewProperties_ShouldSetNulls()
        {
            // Arrange
            var league = new League();

            // Act
            league.Tour = null;
            league.AgeCategory = null;
            league.Gender = null;
            league.Type = null;
            league.NumberOfPeriods = null;
            league.SportCategory = null;

            // Assert
            league.Tour.Should().BeNull();
            league.AgeCategory.Should().BeNull();
            league.Gender.Should().BeNull();
            league.Type.Should().BeNull();
            league.NumberOfPeriods.Should().BeNull();
            league.SportCategory.Should().BeNull();
        }

        [Fact]
        public void League_NewProperties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var leagueType = typeof(League);

            // Assert
            leagueType.GetProperty("Tour")!.PropertyType.Should().Be(typeof(IdNamePair));
            leagueType.GetProperty("AgeCategory")!.PropertyType.Should().Be(typeof(int?));
            leagueType.GetProperty("Gender")!.PropertyType.Should().Be(typeof(int?));
            leagueType.GetProperty("Type")!.PropertyType.Should().Be(typeof(int?));
            leagueType.GetProperty("NumberOfPeriods")!.PropertyType.Should().Be(typeof(int?));
            leagueType.GetProperty("SportCategory")!.PropertyType.Should().Be(typeof(IdNamePair));
        }

        [Fact]
        public void League_WithCompleteRealWorldData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var league = new League
            {
                Id = 1,
                Name = "UEFA Champions League",
                Tour = new IdNamePair { Id = 10, Name = "European Competition" },
                AgeCategory = 0,
                Gender = 1,
                Type = 1,
                NumberOfPeriods = 2,
                SportCategory = new IdNamePair { Id = 1, Name = "Football" }
            };

            // Assert
            league.Id.Should().Be(1);
            league.Name.Should().Be("UEFA Champions League");
            league.Tour.Name.Should().Be("European Competition");
            league.AgeCategory.Should().Be(0);
            league.Gender.Should().Be(1);
            league.Type.Should().Be(1);
            league.NumberOfPeriods.Should().Be(2);
            league.SportCategory.Name.Should().Be("Football");
        }

        [Fact]
        public void League_WithYouthLeagueData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var youthLeague = new League
            {
                Id = 100,
                Name = "U21 Premier League",
                Tour = null,
                AgeCategory = 21,
                Gender = 1,
                Type = 2,
                NumberOfPeriods = 2,
                SportCategory = new IdNamePair { Id = 1, Name = "Football" }
            };

            // Assert
            youthLeague.Name.Should().Be("U21 Premier League");
            youthLeague.AgeCategory.Should().Be(21);
            youthLeague.Tour.Should().BeNull();
        }
    }
} 