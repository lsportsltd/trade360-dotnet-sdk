using System;
using FluentAssertions;
using Trade360SDK.Common.Entities.Fixtures;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures
{
    public class PlayerTests
    {
        [Fact]
        public void Player_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var player = new Player();

            // Assert
            player.Should().NotBeNull();
            player.Id.Should().Be(0);
            player.Name.Should().BeNull();
            player.TeamId.Should().BeNull();
            player.NationalityId.Should().BeNull();
            player.BirthDate.Should().BeNull();
            player.Type.Should().BeNull();
            player.NationalTeamId.Should().BeNull();
        }

        [Fact]
        public void Player_SetId_ShouldSetValue()
        {
            // Arrange
            var player = new Player();
            var id = 12345;

            // Act
            player.Id = id;

            // Assert
            player.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999999)]
        [InlineData(12345)]
        public void Player_SetVariousIds_ShouldSetValue(int id)
        {
            // Arrange
            var player = new Player();

            // Act
            player.Id = id;

            // Assert
            player.Id.Should().Be(id);
        }

        [Fact]
        public void Player_SetName_ShouldSetValue()
        {
            // Arrange
            var player = new Player();
            var name = "Test Player";

            // Act
            player.Name = name;

            // Assert
            player.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Test Player")]
        public void Player_SetVariousNames_ShouldSetValue(string name)
        {
            // Arrange
            var player = new Player();

            // Act
            player.Name = name;

            // Assert
            player.Name.Should().Be(name);
        }

        [Fact]
        public void Player_SetTeamId_ShouldSetValue()
        {
            // Arrange
            var player = new Player();
            var teamId = 100;

            // Act
            player.TeamId = teamId;

            // Assert
            player.TeamId.Should().Be(teamId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        public void Player_SetVariousTeamIds_ShouldSetValue(int teamId)
        {
            // Arrange
            var player = new Player();

            // Act
            player.TeamId = teamId;

            // Assert
            player.TeamId.Should().Be(teamId);
        }

        [Fact]
        public void Player_SetNationalityId_ShouldSetValue()
        {
            // Arrange
            var player = new Player();
            var nationalityId = 50;

            // Act
            player.NationalityId = nationalityId;

            // Assert
            player.NationalityId.Should().Be(nationalityId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(195)] // Approximate number of countries
        public void Player_SetVariousNationalityIds_ShouldSetValue(int nationalityId)
        {
            // Arrange
            var player = new Player();

            // Act
            player.NationalityId = nationalityId;

            // Assert
            player.NationalityId.Should().Be(nationalityId);
        }

        [Fact]
        public void Player_SetBirthDate_ShouldSetValue()
        {
            // Arrange
            var player = new Player();
            var birthDate = new DateTime(1985, 2, 5, 0, 0, 0, DateTimeKind.Utc);

            // Act
            player.BirthDate = birthDate;

            // Assert
            player.BirthDate.Should().Be(birthDate);
        }

        [Fact]
        public void Player_SetBirthDateMinMax_ShouldSetValues()
        {
            // Arrange
            var player = new Player();

            // Act & Assert - Min value
            player.BirthDate = DateTime.MinValue;
            player.BirthDate.Should().Be(DateTime.MinValue);

            // Act & Assert - Max value
            player.BirthDate = DateTime.MaxValue;
            player.BirthDate.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void Player_SetType_ShouldSetValue()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Type = PlayerType.Player;

            // Assert
            player.Type.Should().Be(PlayerType.Player);
        }

        [Theory]
        [InlineData(PlayerType.Player)]
        [InlineData(PlayerType.Other)]
        [InlineData(PlayerType.Coach)]
        public void Player_SetVariousTypes_ShouldSetValue(PlayerType type)
        {
            // Arrange
            var player = new Player();

            // Act
            player.Type = type;

            // Assert
            player.Type.Should().Be(type);
        }

        [Fact]
        public void Player_SetNationalTeamId_ShouldSetValue()
        {
            // Arrange
            var player = new Player();
            var nationalTeamId = 25;

            // Act
            player.NationalTeamId = nationalTeamId;

            // Assert
            player.NationalTeamId.Should().Be(nationalTeamId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        public void Player_SetVariousNationalTeamIds_ShouldSetValue(int nationalTeamId)
        {
            // Arrange
            var player = new Player();

            // Act
            player.NationalTeamId = nationalTeamId;

            // Assert
            player.NationalTeamId.Should().Be(nationalTeamId);
        }

        [Fact]
        public void Player_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var player = new Player();
            var id = 100;
            var name = "Test Player";
            var teamId = 50;
            var nationalityId = 10;
            var birthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var type = PlayerType.Player;
            var nationalTeamId = 5;

            // Act
            player.Id = id;
            player.Name = name;
            player.TeamId = teamId;
            player.NationalityId = nationalityId;
            player.BirthDate = birthDate;
            player.Type = type;
            player.NationalTeamId = nationalTeamId;

            // Assert
            player.Id.Should().Be(id);
            player.Name.Should().Be(name);
            player.TeamId.Should().Be(teamId);
            player.NationalityId.Should().Be(nationalityId);
            player.BirthDate.Should().Be(birthDate);
            player.Type.Should().Be(type);
            player.NationalTeamId.Should().Be(nationalTeamId);
        }

        [Fact]
        public void Player_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Name = null;
            player.TeamId = null;
            player.NationalityId = null;
            player.BirthDate = null;
            player.Type = null;
            player.NationalTeamId = null;

            // Assert
            player.Name.Should().BeNull();
            player.TeamId.Should().BeNull();
            player.NationalityId.Should().BeNull();
            player.BirthDate.Should().BeNull();
            player.Type.Should().BeNull();
            player.NationalTeamId.Should().BeNull();
        }

        [Fact]
        public void Player_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act & Assert - Test that we can set and get each property multiple times
            player.Id = 1;
            player.Id.Should().Be(1);
            player.Id = 2;
            player.Id.Should().Be(2);

            player.Name = "Name1";
            player.Name.Should().Be("Name1");
            player.Name = "Name2";
            player.Name.Should().Be("Name2");
            player.Name = null;
            player.Name.Should().BeNull();

            player.TeamId = 100;
            player.TeamId.Should().Be(100);
            player.TeamId = null;
            player.TeamId.Should().BeNull();

            player.Type = PlayerType.Player;
            player.Type.Should().Be(PlayerType.Player);
            player.Type = PlayerType.Coach;
            player.Type.Should().Be(PlayerType.Coach);
            player.Type = null;
            player.Type.Should().BeNull();
        }

        [Fact]
        public void Player_WithRealWorldData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var player = new Player
            {
                Id = 7,
                Name = "Test Player",
                TeamId = 101,
                NationalityId = 1, // Portugal
                BirthDate = new DateTime(1985, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                Type = PlayerType.Player,
                NationalTeamId = 50 // Portugal National Team
            };

            // Assert
            player.Id.Should().Be(7);
            player.Name.Should().Be("Test Player");
            player.TeamId.Should().Be(101);
            player.NationalityId.Should().Be(1);
            player.BirthDate.Should().Be(new DateTime(1985, 2, 5, 0, 0, 0, DateTimeKind.Utc));
            player.Type.Should().Be(PlayerType.Player);
            player.NationalTeamId.Should().Be(50);
        }

        [Fact]
        public void Player_WithCoachType_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var coach = new Player
            {
                Id = 1,
                Name = "Test Player",
                TeamId = 200,
                NationalityId = 2, // Spain
                BirthDate = new DateTime(1971, 1, 18, 0, 0, 0, DateTimeKind.Utc),
                Type = PlayerType.Coach,
                NationalTeamId = null
            };

            // Assert
            coach.Name.Should().Be("Test Player");
            coach.Type.Should().Be(PlayerType.Coach);
            coach.NationalTeamId.Should().BeNull();
        }

        [Fact]
        public void Player_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Player"
            };

            // Act
            var result = player.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Player");
        }

        [Fact]
        public void Player_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                Name = "Test Player"
            };

            // Act
            var hashCode1 = player.GetHashCode();
            var hashCode2 = player.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Player_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var player = new Player();

            // Assert
            player.GetType().Should().Be(typeof(Player));
            player.GetType().Name.Should().Be("Player");
            player.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
        }

        [Fact]
        public void Player_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var playerType = typeof(Player);

            // Assert
            playerType.GetProperty("Id")!.PropertyType.Should().Be(typeof(int));
            playerType.GetProperty("Name")!.PropertyType.Should().Be(typeof(string));
            playerType.GetProperty("TeamId")!.PropertyType.Should().Be(typeof(int?));
            playerType.GetProperty("NationalityId")!.PropertyType.Should().Be(typeof(int?));
            playerType.GetProperty("BirthDate")!.PropertyType.Should().Be(typeof(DateTime?));
            playerType.GetProperty("Type")!.PropertyType.Should().Be(typeof(PlayerType?));
            playerType.GetProperty("NationalTeamId")!.PropertyType.Should().Be(typeof(int?));
        }

        [Fact]
        public void Player_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var player1 = new Player { Id = 1, Name = "Player 1" };
            var player2 = new Player { Id = 2, Name = "Player 2" };

            // Assert
            player1.Id.Should().NotBe(player2.Id);
            player1.Name.Should().NotBe(player2.Name);
            player1.Should().NotBeSameAs(player2);
        }
    }
}

