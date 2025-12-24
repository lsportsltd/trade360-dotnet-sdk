using FluentAssertions;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;
using System;

namespace Trade360SDK.Common.Tests
{
    public class LeagueTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstanceSuccessfully()
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
        public void Properties_ShouldGetAndSetValues()
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = 7;
            league.Name = "Premier League";

            // Assert
            league.Id.Should().Be(7);
            league.Name.Should().Be("Premier League");
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            // Arrange & Act
            var league = new League();

            // Assert
            league.Id.Should().Be(0);
            league.Name.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Id_ShouldHandleVariousIntegerValues(int id)
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = id;

            // Assert
            league.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("Premier League")]
        [InlineData("La Liga")]
        [InlineData("Bundesliga")]
        [InlineData("Serie A")]
        [InlineData("Ligue 1")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        public void Name_ShouldHandleVariousStringValues(string name)
        {
            // Arrange
            var league = new League();

            // Act
            league.Name = name;

            // Assert
            league.Name.Should().Be(name);
        }

        [Fact]
        public void Name_ShouldAllowNullAssignment()
        {
            // Arrange
            var league = new League();
            league.Name = "Test League";

            // Act
            league.Name = null;

            // Assert
            league.Name.Should().BeNull();
        }

        [Fact]
        public void Name_ShouldAllowReassignment()
        {
            // Arrange
            var league = new League();

            // Act
            league.Name = "Original League";
            league.Name = "Updated League";

            // Assert
            league.Name.Should().Be("Updated League");
        }

        [Fact]
        public void Properties_ShouldBeIndependent()
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = 123;
            league.Name = "Test League";

            // Assert
            league.Id.Should().Be(123);
            league.Name.Should().Be("Test League");

            // Modify one property, other should remain unchanged
            league.Id = 456;
            league.Name.Should().Be("Test League");

            league.Name = "Modified League";
            league.Id.Should().Be(456);
        }

        [Fact]
        public void League_ShouldSupportObjectInitializer()
        {
            // Arrange & Act
            var league = new League
            {
                Id = 789,
                Name = "Champions League"
            };

            // Assert
            league.Id.Should().Be(789);
            league.Name.Should().Be("Champions League");
        }

        [Fact]
        public void League_ShouldSupportReflectionAccess()
        {
            // Arrange
            var league = new League();
            var type = typeof(League);
            var idProperty = type.GetProperty("Id");
            var nameProperty = type.GetProperty("Name");

            // Act
            idProperty!.SetValue(league, 999);
            nameProperty!.SetValue(league, "Reflection League");

            // Assert
            var idValue = (int)idProperty.GetValue(league)!;
            var nameValue = (string?)nameProperty.GetValue(league);

            idValue.Should().Be(999);
            nameValue.Should().Be("Reflection League");
        }

        [Fact]
        public void League_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var type = typeof(League);

            // Act & Assert
            type.Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
            type.FullName.Should().Be("Trade360SDK.Common.Entities.Fixtures.League");
        }

        [Fact]
        public void League_ShouldHaveCorrectPropertyTypes()
        {
            // Arrange
            var type = typeof(League);
            var idProperty = type.GetProperty("Id");
            var nameProperty = type.GetProperty("Name");

            // Act & Assert
            idProperty.Should().NotBeNull();
            idProperty!.PropertyType.Should().Be(typeof(int));

            nameProperty.Should().NotBeNull();
            nameProperty!.PropertyType.Should().Be(typeof(string));
        }

        [Fact]
        public void Name_ShouldHandleLongStrings()
        {
            // Arrange
            var league = new League();
            var longName = new string('A', 1000); // 1000 character string

            // Act
            league.Name = longName;

            // Assert
            league.Name.Should().Be(longName);
            league.Name.Should().HaveLength(1000);
        }

        [Fact]
        public void Name_ShouldHandleSpecialCharacters()
        {
            // Arrange
            var league = new League();
            var specialName = "Ñáme Wíth Spëçîál Çhàráctërs & Symbols 123!@#$%^&*()";

            // Act
            league.Name = specialName;

            // Assert
            league.Name.Should().Be(specialName);
        }

        [Fact]
        public void Name_ShouldHandleUnicodeCharacters()
        {
            // Arrange
            var league = new League();
            var unicodeName = "Premier League 中国足球超级联赛 プレミアリーグ";

            // Act
            league.Name = unicodeName;

            // Assert
            league.Name.Should().Be(unicodeName);
        }

        [Fact]
        public void League_ShouldHandleExtremeIds()
        {
            // Arrange
            var league = new League();

            // Act & Assert - Maximum value
            league.Id = int.MaxValue;
            league.Id.Should().Be(int.MaxValue);

            // Act & Assert - Minimum value
            league.Id = int.MinValue;
            league.Id.Should().Be(int.MinValue);

            // Act & Assert - Zero
            league.Id = 0;
            league.Id.Should().Be(0);
        }

        [Fact]
        public void League_PropertyAssignment_ShouldBeAtomic()
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = 500;
            league.Name = "Initial League";

            // Verify initial state
            league.Id.Should().Be(500);
            league.Name.Should().Be("Initial League");

            // Act - Change both properties
            league.Id = 600;
            league.Name = "Updated League";

            // Assert - Both changes should be preserved
            league.Id.Should().Be(600);
            league.Name.Should().Be("Updated League");
        }

        [Fact]
        public void League_MultipleInstancesShouldBeIndependent()
        {
            // Arrange
            var league1 = new League();
            var league2 = new League();

            // Act
            league1.Id = 111;
            league1.Name = "League One";

            league2.Id = 222;
            league2.Name = "League Two";

            // Assert
            league1.Id.Should().Be(111);
            league1.Name.Should().Be("League One");

            league2.Id.Should().Be(222);
            league2.Name.Should().Be("League Two");

            // Instances should be completely independent
            league1.Name.Should().NotBe(league2.Name);
        }

        [Fact]
        public void League_ShouldSupportZeroId()
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = 0; // Explicitly set to zero
            league.Name = "Zero ID League";

            // Assert
            league.Id.Should().Be(0);
            league.Name.Should().Be("Zero ID League");
        }

        [Fact]
        public void League_TypeInformation_ShouldBeCorrect()
        {
            // Arrange
            var league = new League();
            var type = league.GetType();

            // Act & Assert
            type.Name.Should().Be("League");
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Name_ShouldHandleWhitespaceVariations()
        {
            // Arrange
            var league = new League();

            // Act & Assert - Empty string
            league.Name = "";
            league.Name.Should().Be("");

            // Act & Assert - Single space
            league.Name = " ";
            league.Name.Should().Be(" ");

            // Act & Assert - Multiple spaces
            league.Name = "   ";
            league.Name.Should().Be("   ");

            // Act & Assert - Tabs and newlines
            league.Name = "\t\n\r";
            league.Name.Should().Be("\t\n\r");
        }

        [Fact]
        public void League_ShouldMaintainStringReferenceIntegrity()
        {
            // Arrange
            var league = new League();
            var originalString = "Original League Name";
            var sameString = "Original League Name";

            // Act
            league.Name = originalString;

            // Assert
            league.Name.Should().Be(originalString);
            league.Name.Should().Be(sameString); // Same value but different string instance
            // Note: String interning may make this reference comparison equal, but logically they're the same
        }

        [Theory]
        [InlineData(1, "First League")]
        [InlineData(100, "Hundred League")]
        [InlineData(-50, "Negative League")]
        [InlineData(0, null)]
        [InlineData(999, "")]
        [InlineData(42, "  Spaced League  ")]
        public void League_ParameterizedTesting_ShouldWorkCorrectly(int id, string? name)
        {
            // Arrange
            var league = new League();

            // Act
            league.Id = id;
            league.Name = name;

            // Assert
            league.Id.Should().Be(id);
            league.Name.Should().Be(name);
        }

        [Fact]
        public void League_NullNameAfterAssignment_ShouldBeSupported()
        {
            // Arrange
            var league = new League();
            league.Name = "Temporary League";

            // Act
            league.Name = null;

            // Assert
            league.Name.Should().BeNull();
            league.Id.Should().Be(0); // Should not affect ID
        }

        [Fact]
        public void League_SetTour_ShouldSetValue()
        {
            // Arrange
            var league = new League();
            var tour = new IdNamePair { Id = 1, Name = "ATP Tour" };

            // Act
            league.Tour = tour;

            // Assert
            league.Tour.Should().Be(tour);
            league.Tour.Id.Should().Be(1);
            league.Tour.Name.Should().Be("ATP Tour");
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
        [InlineData(int.MinValue)]
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
        [InlineData(int.MaxValue)]
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
        [InlineData(int.MaxValue)]
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
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
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
            var sportCategory = new IdNamePair { Id = 1, Name = "Football" };

            // Act
            league.SportCategory = sportCategory;

            // Assert
            league.SportCategory.Should().Be(sportCategory);
            league.SportCategory.Id.Should().Be(1);
            league.SportCategory.Name.Should().Be("Football");
        }

        [Fact]
        public void League_SetAllNewProperties_ShouldSetAllValues()
        {
            // Arrange
            var league = new League();
            var tour = new IdNamePair { Id = 1, Name = "ATP Tour" };
            var ageCategory = 21;
            var gender = 1;
            var type = 2;
            var numberOfPeriods = 4;
            var sportCategory = new IdNamePair { Id = 5, Name = "Tennis" };

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
        public void League_WithRealWorldDataIncludingNewFields_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var league = new League
            {
                Id = 1,
                Name = "Premier League",
                Tour = null,
                AgeCategory = 0,
                Gender = 1,
                Type = 1,
                NumberOfPeriods = 2,
                SportCategory = new IdNamePair { Id = 1, Name = "Football" }
            };

            // Assert
            league.Id.Should().Be(1);
            league.Name.Should().Be("Premier League");
            league.AgeCategory.Should().Be(0);
            league.Gender.Should().Be(1);
            league.Type.Should().Be(1);
            league.NumberOfPeriods.Should().Be(2);
            league.SportCategory.Name.Should().Be("Football");
        }
    }
} 