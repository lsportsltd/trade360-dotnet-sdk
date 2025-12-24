using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures
{
    public class ParticipantComprehensiveTests
    {
        [Fact]
        public void Participant_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var participant = new Participant();

            // Assert
            participant.Should().NotBeNull();
            participant.Id.Should().Be(0);
            participant.Name.Should().BeNull();
            participant.Position.Should().BeNull();
            participant.RotationId.Should().BeNull();
            participant.IsActive.Should().Be(-1); // Default value as per class definition
            participant.Form.Should().BeNull();
            participant.Formation.Should().BeNull();
            participant.FixturePlayers.Should().BeNull();
            participant.Gender.Should().BeNull();
            participant.AgeCategory.Should().BeNull();
            participant.Type.Should().BeNull();
        }

        [Fact]
        public void Participant_SetId_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var id = 12345;

            // Act
            participant.Id = id;

            // Assert
            participant.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999999)]
        [InlineData(12345)]
        public void Participant_SetVariousIds_ShouldSetValue(int id)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Id = id;

            // Assert
            participant.Id.Should().Be(id);
        }

        [Fact]
        public void Participant_SetName_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var name = "Manchester United";

            // Act
            participant.Name = name;

            // Assert
            participant.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Manchester United")]
        [InlineData("Liverpool")]
        [InlineData("Chelsea")]
        [InlineData("Arsenal")]
        [InlineData("Tottenham Hotspur")]
        [InlineData("Manchester City")]
        [InlineData("Newcastle United")]
        [InlineData("Brighton & Hove Albion")]
        [InlineData("West Ham United")]
        public void Participant_SetVariousNames_ShouldSetValue(string name)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Name = name;

            // Assert
            participant.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("Team with unicode: 曼联")]
        [InlineData("Team with special chars: @#$%^&*()")]
        [InlineData("Team with numbers: Team 2024")]
        [InlineData("Very long team name that exceeds normal expectations for testing purposes and edge cases")]
        [InlineData("Team\nwith\nnewlines")]
        [InlineData("Team\twith\ttabs")]
        [InlineData("Team with accents: São Paulo FC")]
        [InlineData("Team with umlauts: Bayern München")]
        public void Participant_SetNamesWithSpecialCharacters_ShouldSetValue(string name)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Name = name;

            // Assert
            participant.Name.Should().Be(name);
        }

        [Fact]
        public void Participant_SetPosition_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var position = "1";

            // Act
            participant.Position = position;

            // Assert
            participant.Position.Should().Be(position);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("Home")]
        [InlineData("Away")]
        [InlineData("Draw")]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("Position A")]
        [InlineData("Left")]
        [InlineData("Right")]
        public void Participant_SetVariousPositions_ShouldSetValue(string position)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Position = position;

            // Assert
            participant.Position.Should().Be(position);
        }

        [Fact]
        public void Participant_SetRotationId_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var rotationId = 5;

            // Act
            participant.RotationId = rotationId;

            // Assert
            participant.RotationId.Should().Be(rotationId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        [InlineData(12345)]
        public void Participant_SetVariousRotationIds_ShouldSetValue(int rotationId)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.RotationId = rotationId;

            // Assert
            participant.RotationId.Should().Be(rotationId);
        }

        [Fact]
        public void Participant_SetIsActive_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var isActive = 1;

            // Act
            participant.IsActive = isActive;

            // Assert
            participant.IsActive.Should().Be(isActive);
        }

        [Theory]
        [InlineData(-1)] // Default value
        [InlineData(0)]  // Inactive
        [InlineData(1)]  // Active
        [InlineData(2)]  // Other status
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        public void Participant_SetVariousIsActiveValues_ShouldSetValue(int isActive)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.IsActive = isActive;

            // Assert
            participant.IsActive.Should().Be(isActive);
        }

        [Fact]
        public void Participant_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var participant = new Participant();
            var id = 100;
            var name = "Test Team";
            var position = "1";
            var rotationId = 5;
            var isActive = 1;

            // Act
            participant.Id = id;
            participant.Name = name;
            participant.Position = position;
            participant.RotationId = rotationId;
            participant.IsActive = isActive;

            // Assert
            participant.Id.Should().Be(id);
            participant.Name.Should().Be(name);
            participant.Position.Should().Be(position);
            participant.RotationId.Should().Be(rotationId);
            participant.IsActive.Should().Be(isActive);
        }

        [Fact]
        public void Participant_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Name = null;
            participant.Position = null;
            participant.RotationId = null;

            // Assert
            participant.Name.Should().BeNull();
            participant.Position.Should().BeNull();
            participant.RotationId.Should().BeNull();
            // IsActive cannot be null as it's an int, not int?
        }

        [Fact]
        public void Participant_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var participant = new Participant();

            // Act & Assert - Test that we can set and get each property multiple times
            participant.Id = 1;
            participant.Id.Should().Be(1);
            participant.Id = 2;
            participant.Id.Should().Be(2);

            participant.Name = "Name1";
            participant.Name.Should().Be("Name1");
            participant.Name = "Name2";
            participant.Name.Should().Be("Name2");
            participant.Name = null;
            participant.Name.Should().BeNull();

            participant.Position = "1";
            participant.Position.Should().Be("1");
            participant.Position = "2";
            participant.Position.Should().Be("2");
            participant.Position = null;
            participant.Position.Should().BeNull();

            participant.RotationId = 5;
            participant.RotationId.Should().Be(5);
            participant.RotationId = 10;
            participant.RotationId.Should().Be(10);
            participant.RotationId = null;
            participant.RotationId.Should().BeNull();

            participant.IsActive = 0;
            participant.IsActive.Should().Be(0);
            participant.IsActive = 1;
            participant.IsActive.Should().Be(1);
            participant.IsActive = -1;
            participant.IsActive.Should().Be(-1);
        }

        [Fact]
        public void Participant_WithRealWorldTeamData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var homeTeam = new Participant
            {
                Id = 1,
                Name = "Manchester United",
                Position = "1",
                RotationId = 100,
                IsActive = 1
            };

            var awayTeam = new Participant
            {
                Id = 2,
                Name = "Liverpool",
                Position = "2",
                RotationId = 200,
                IsActive = 1
            };

            // Assert
            homeTeam.Id.Should().Be(1);
            homeTeam.Name.Should().Be("Manchester United");
            homeTeam.Position.Should().Be("1");
            homeTeam.RotationId.Should().Be(100);
            homeTeam.IsActive.Should().Be(1);

            awayTeam.Id.Should().Be(2);
            awayTeam.Name.Should().Be("Liverpool");
            awayTeam.Position.Should().Be("2");
            awayTeam.RotationId.Should().Be(200);
            awayTeam.IsActive.Should().Be(1);
        }

        [Fact]
        public void Participant_WithPlayerData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var player1 = new Participant
            {
                Id = 10,
                Name = "Test Player",
                Position = "Forward",
                RotationId = 7,
                IsActive = 1
            };

            var player2 = new Participant
            {
                Id = 11,
                Name = "Lionel Messi",
                Position = "Forward",
                RotationId = 10,
                IsActive = 1
            };

            // Assert
            player1.Name.Should().Be("Test Player");
            player1.Position.Should().Be("Forward");
            player1.RotationId.Should().Be(7);

            player2.Name.Should().Be("Lionel Messi");
            player2.Position.Should().Be("Forward");
            player2.RotationId.Should().Be(10);
        }

        [Fact]
        public void Participant_WithInactiveStatus_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var inactiveParticipant = new Participant
            {
                Id = 999,
                Name = "Inactive Team",
                Position = "0",
                RotationId = null,
                IsActive = 0
            };

            // Assert
            inactiveParticipant.IsActive.Should().Be(0);
            inactiveParticipant.RotationId.Should().BeNull();
        }

        [Fact]
        public void Participant_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var participant = new Participant
            {
                Id = 1,
                Name = "Test Participant"
            };

            // Act
            var result = participant.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Participant");
        }

        [Fact]
        public void Participant_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var participant = new Participant
            {
                Id = 1,
                Name = "Test Participant",
                Position = "1",
                IsActive = 1
            };

            // Act
            var hashCode1 = participant.GetHashCode();
            var hashCode2 = participant.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Participant_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var participant = new Participant();

            // Assert
            participant.GetType().Should().Be(typeof(Participant));
            participant.GetType().Name.Should().Be("Participant");
            participant.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
        }

        [Fact]
        public void Participant_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var participant = new Participant();

            // Assert
            participant.Id.GetType().Should().Be(typeof(int));
            participant.IsActive.GetType().Should().Be(typeof(int));
            
            // Check nullable properties
            var nameProperty = typeof(Participant).GetProperty("Name");
            nameProperty.Should().NotBeNull();
            nameProperty!.PropertyType.Should().Be(typeof(string));

            var positionProperty = typeof(Participant).GetProperty("Position");
            positionProperty.Should().NotBeNull();
            positionProperty!.PropertyType.Should().Be(typeof(string));

            var rotationIdProperty = typeof(Participant).GetProperty("RotationId");
            rotationIdProperty.Should().NotBeNull();
            rotationIdProperty!.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Participant_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var participant1 = new Participant { Id = 1, Name = "Participant 1", IsActive = 1 };
            var participant2 = new Participant { Id = 2, Name = "Participant 2", IsActive = 0 };

            // Assert
            participant1.Id.Should().NotBe(participant2.Id);
            participant1.Name.Should().NotBe(participant2.Name);
            participant1.IsActive.Should().NotBe(participant2.IsActive);
            participant1.Should().NotBeSameAs(participant2);
        }

        [Fact]
        public void Participant_WithEmptyStringValues_ShouldPreserveEmptyStrings()
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Name = "";
            participant.Position = "";

            // Assert
            participant.Name.Should().Be("");
            participant.Name.Should().NotBeNull();
            participant.Name.Should().BeEmpty();
            
            participant.Position.Should().Be("");
            participant.Position.Should().NotBeNull();
            participant.Position.Should().BeEmpty();
        }

        [Fact]
        public void Participant_WithWhitespaceOnlyValues_ShouldPreserveWhitespace()
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Name = "   ";
            participant.Position = "   ";

            // Assert
            participant.Name.Should().Be("   ");
            participant.Name.Should().NotBeNull();
            participant.Name.Should().NotBeEmpty();
            
            participant.Position.Should().Be("   ");
            participant.Position.Should().NotBeNull();
            participant.Position.Should().NotBeEmpty();
        }

        [Fact]
        public void Participant_DefaultIsActiveValue_ShouldBeMinusOne()
        {
            // Arrange & Act
            var participant = new Participant();

            // Assert
            participant.IsActive.Should().Be(-1);
        }

        [Fact]
        public void Participant_SetForm_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var form = "WWDLW";

            // Act
            participant.Form = form;

            // Assert
            participant.Form.Should().Be(form);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("WWWWW")]
        [InlineData("LLLLL")]
        [InlineData("DDDDD")]
        [InlineData("WDLWL")]
        [InlineData("W")]
        public void Participant_SetVariousForms_ShouldSetValue(string form)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Form = form;

            // Assert
            participant.Form.Should().Be(form);
        }

        [Fact]
        public void Participant_SetFormation_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var formation = "4-3-3";

            // Act
            participant.Formation = formation;

            // Assert
            participant.Formation.Should().Be(formation);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("4-4-2")]
        [InlineData("4-3-3")]
        [InlineData("3-5-2")]
        [InlineData("5-3-2")]
        [InlineData("4-2-3-1")]
        [InlineData("3-4-3")]
        public void Participant_SetVariousFormations_ShouldSetValue(string formation)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Formation = formation;

            // Assert
            participant.Formation.Should().Be(formation);
        }

        [Fact]
        public void Participant_SetFixturePlayers_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var fixturePlayers = new List<FixturePlayer>
            {
                new FixturePlayer { PlayerId = 1, ShirtNumber = "10", IsCaptain = true },
                new FixturePlayer { PlayerId = 2, ShirtNumber = "7", IsCaptain = false }
            };

            // Act
            participant.FixturePlayers = fixturePlayers;

            // Assert
            participant.FixturePlayers.Should().BeEquivalentTo(fixturePlayers);
            participant.FixturePlayers.Should().HaveCount(2);
        }

        [Fact]
        public void Participant_SetEmptyFixturePlayers_ShouldSetEmptyList()
        {
            // Arrange
            var participant = new Participant();
            var emptyList = new List<FixturePlayer>();

            // Act
            participant.FixturePlayers = emptyList;

            // Assert
            participant.FixturePlayers.Should().BeEmpty();
            participant.FixturePlayers.Should().NotBeNull();
        }

        [Fact]
        public void Participant_SetGender_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var gender = 1;

            // Act
            participant.Gender = gender;

            // Assert
            participant.Gender.Should().Be(gender);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Participant_SetVariousGenders_ShouldSetValue(int gender)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Gender = gender;

            // Assert
            participant.Gender.Should().Be(gender);
        }

        [Fact]
        public void Participant_SetAgeCategory_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var ageCategory = 21;

            // Act
            participant.AgeCategory = ageCategory;

            // Assert
            participant.AgeCategory.Should().Be(ageCategory);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(18)]
        [InlineData(21)]
        [InlineData(23)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Participant_SetVariousAgeCategories_ShouldSetValue(int ageCategory)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.AgeCategory = ageCategory;

            // Assert
            participant.AgeCategory.Should().Be(ageCategory);
        }

        [Fact]
        public void Participant_SetType_ShouldSetValue()
        {
            // Arrange
            var participant = new Participant();
            var type = 1;

            // Act
            participant.Type = type;

            // Assert
            participant.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Participant_SetVariousTypes_ShouldSetValue(int type)
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Type = type;

            // Assert
            participant.Type.Should().Be(type);
        }

        [Fact]
        public void Participant_SetAllNewProperties_ShouldSetAllValues()
        {
            // Arrange
            var participant = new Participant();
            var form = "WWDLW";
            var formation = "4-3-3";
            var fixturePlayers = new List<FixturePlayer>
            {
                new FixturePlayer { PlayerId = 10, ShirtNumber = "10" }
            };
            var gender = 1;
            var ageCategory = 21;
            var type = 2;

            // Act
            participant.Form = form;
            participant.Formation = formation;
            participant.FixturePlayers = fixturePlayers;
            participant.Gender = gender;
            participant.AgeCategory = ageCategory;
            participant.Type = type;

            // Assert
            participant.Form.Should().Be(form);
            participant.Formation.Should().Be(formation);
            participant.FixturePlayers.Should().BeEquivalentTo(fixturePlayers);
            participant.Gender.Should().Be(gender);
            participant.AgeCategory.Should().Be(ageCategory);
            participant.Type.Should().Be(type);
        }

        [Fact]
        public void Participant_SetNullNewProperties_ShouldSetNulls()
        {
            // Arrange
            var participant = new Participant();

            // Act
            participant.Form = null;
            participant.Formation = null;
            participant.FixturePlayers = null;
            participant.Gender = null;
            participant.AgeCategory = null;
            participant.Type = null;

            // Assert
            participant.Form.Should().BeNull();
            participant.Formation.Should().BeNull();
            participant.FixturePlayers.Should().BeNull();
            participant.Gender.Should().BeNull();
            participant.AgeCategory.Should().BeNull();
            participant.Type.Should().BeNull();
        }

        [Fact]
        public void Participant_WithRealWorldTeamDataIncludingNewFields_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var team = new Participant
            {
                Id = 1,
                Name = "Manchester United",
                Position = "1",
                RotationId = 100,
                IsActive = 1,
                Form = "WDWWL",
                Formation = "4-2-3-1",
                FixturePlayers = new List<FixturePlayer>
                {
                    new FixturePlayer { PlayerId = 1, ShirtNumber = "1", IsStartingLineup = true },
                    new FixturePlayer { PlayerId = 7, ShirtNumber = "7", IsCaptain = true, IsStartingLineup = true }
                },
                Gender = 1,
                AgeCategory = 0,
                Type = 1
            };

            // Assert
            team.Id.Should().Be(1);
            team.Name.Should().Be("Manchester United");
            team.Form.Should().Be("WDWWL");
            team.Formation.Should().Be("4-2-3-1");
            team.FixturePlayers.Should().HaveCount(2);
            team.Gender.Should().Be(1);
            team.AgeCategory.Should().Be(0);
            team.Type.Should().Be(1);
        }

        [Fact]
        public void Participant_NewProperties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var participantType = typeof(Participant);

            // Assert
            var formProperty = participantType.GetProperty("Form");
            formProperty.Should().NotBeNull();
            formProperty!.PropertyType.Should().Be(typeof(string));

            var formationProperty = participantType.GetProperty("Formation");
            formationProperty.Should().NotBeNull();
            formationProperty!.PropertyType.Should().Be(typeof(string));

            var fixturePlayersProperty = participantType.GetProperty("FixturePlayers");
            fixturePlayersProperty.Should().NotBeNull();
            fixturePlayersProperty!.PropertyType.Should().Be(typeof(List<FixturePlayer>));

            var genderProperty = participantType.GetProperty("Gender");
            genderProperty.Should().NotBeNull();
            genderProperty!.PropertyType.Should().Be(typeof(int?));

            var ageCategoryProperty = participantType.GetProperty("AgeCategory");
            ageCategoryProperty.Should().NotBeNull();
            ageCategoryProperty!.PropertyType.Should().Be(typeof(int?));

            var typeProperty = participantType.GetProperty("Type");
            typeProperty.Should().NotBeNull();
            typeProperty!.PropertyType.Should().Be(typeof(int?));
        }
    }
} 