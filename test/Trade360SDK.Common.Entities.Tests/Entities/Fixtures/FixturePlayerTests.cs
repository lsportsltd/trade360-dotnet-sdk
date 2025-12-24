using FluentAssertions;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures
{
    public class FixturePlayerTests
    {
        [Fact]
        public void FixturePlayer_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var fixturePlayer = new FixturePlayer();

            // Assert
            fixturePlayer.Should().NotBeNull();
            fixturePlayer.PlayerId.Should().BeNull();
            fixturePlayer.ShirtNumber.Should().BeNull();
            fixturePlayer.IsCaptain.Should().BeNull();
            fixturePlayer.IsStartingLineup.Should().BeNull();
            fixturePlayer.Position.Should().BeNull();
            fixturePlayer.State.Should().BeNull();
            fixturePlayer.Player.Should().BeNull();
        }

        [Fact]
        public void FixturePlayer_SetPlayerId_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();
            var playerId = 12345;

            // Act
            fixturePlayer.PlayerId = playerId;

            // Assert
            fixturePlayer.PlayerId.Should().Be(playerId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999999)]
        public void FixturePlayer_SetVariousPlayerIds_ShouldSetValue(int playerId)
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.PlayerId = playerId;

            // Assert
            fixturePlayer.PlayerId.Should().Be(playerId);
        }

        [Fact]
        public void FixturePlayer_SetShirtNumber_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();
            var shirtNumber = "10";

            // Act
            fixturePlayer.ShirtNumber = shirtNumber;

            // Assert
            fixturePlayer.ShirtNumber.Should().Be(shirtNumber);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("1")]
        [InlineData("10")]
        [InlineData("99")]
        [InlineData("7")]
        [InlineData("23")]
        [InlineData("00")]
        public void FixturePlayer_SetVariousShirtNumbers_ShouldSetValue(string shirtNumber)
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.ShirtNumber = shirtNumber;

            // Assert
            fixturePlayer.ShirtNumber.Should().Be(shirtNumber);
        }

        [Fact]
        public void FixturePlayer_SetIsCaptain_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.IsCaptain = true;

            // Assert
            fixturePlayer.IsCaptain.Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FixturePlayer_SetVariousIsCaptainValues_ShouldSetValue(bool isCaptain)
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.IsCaptain = isCaptain;

            // Assert
            fixturePlayer.IsCaptain.Should().Be(isCaptain);
        }

        [Fact]
        public void FixturePlayer_SetIsStartingLineup_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.IsStartingLineup = true;

            // Assert
            fixturePlayer.IsStartingLineup.Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FixturePlayer_SetVariousIsStartingLineupValues_ShouldSetValue(bool isStartingLineup)
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.IsStartingLineup = isStartingLineup;

            // Assert
            fixturePlayer.IsStartingLineup.Should().Be(isStartingLineup);
        }

        [Fact]
        public void FixturePlayer_SetPosition_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();
            var position = new IdNamePair { Id = 1, Name = "Goalkeeper" };

            // Act
            fixturePlayer.Position = position;

            // Assert
            fixturePlayer.Position.Should().Be(position);
            fixturePlayer.Position.Id.Should().Be(1);
            fixturePlayer.Position.Name.Should().Be("Goalkeeper");
        }

        [Fact]
        public void FixturePlayer_SetState_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();
            var state = new IdNamePair { Id = 1, Name = "Active" };

            // Act
            fixturePlayer.State = state;

            // Assert
            fixturePlayer.State.Should().Be(state);
            fixturePlayer.State.Id.Should().Be(1);
            fixturePlayer.State.Name.Should().Be("Active");
        }

        [Fact]
        public void FixturePlayer_SetPlayer_ShouldSetValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();
            var player = new Player
            {
                Id = 7,
                Name = "Test Player",
                TeamId = 100,
                Type = PlayerType.Player
            };

            // Act
            fixturePlayer.Player = player;

            // Assert
            fixturePlayer.Player.Should().Be(player);
            fixturePlayer.Player.Id.Should().Be(7);
            fixturePlayer.Player.Name.Should().Be("Test Player");
            fixturePlayer.Player.TeamId.Should().Be(100);
            fixturePlayer.Player.Type.Should().Be(PlayerType.Player);
        }

        [Fact]
        public void FixturePlayer_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();
            var playerId = 100;
            var shirtNumber = "10";
            var isCaptain = true;
            var isStartingLineup = true;
            var position = new IdNamePair { Id = 2, Name = "Forward" };
            var state = new IdNamePair { Id = 1, Name = "Playing" };
            var player = new Player { Id = 100, Name = "Test Player" };

            // Act
            fixturePlayer.PlayerId = playerId;
            fixturePlayer.ShirtNumber = shirtNumber;
            fixturePlayer.IsCaptain = isCaptain;
            fixturePlayer.IsStartingLineup = isStartingLineup;
            fixturePlayer.Position = position;
            fixturePlayer.State = state;
            fixturePlayer.Player = player;

            // Assert
            fixturePlayer.PlayerId.Should().Be(playerId);
            fixturePlayer.ShirtNumber.Should().Be(shirtNumber);
            fixturePlayer.IsCaptain.Should().Be(isCaptain);
            fixturePlayer.IsStartingLineup.Should().Be(isStartingLineup);
            fixturePlayer.Position.Should().Be(position);
            fixturePlayer.State.Should().Be(state);
            fixturePlayer.Player.Should().Be(player);
        }

        [Fact]
        public void FixturePlayer_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act
            fixturePlayer.PlayerId = null;
            fixturePlayer.ShirtNumber = null;
            fixturePlayer.IsCaptain = null;
            fixturePlayer.IsStartingLineup = null;
            fixturePlayer.Position = null;
            fixturePlayer.State = null;
            fixturePlayer.Player = null;

            // Assert
            fixturePlayer.PlayerId.Should().BeNull();
            fixturePlayer.ShirtNumber.Should().BeNull();
            fixturePlayer.IsCaptain.Should().BeNull();
            fixturePlayer.IsStartingLineup.Should().BeNull();
            fixturePlayer.Position.Should().BeNull();
            fixturePlayer.State.Should().BeNull();
            fixturePlayer.Player.Should().BeNull();
        }

        [Fact]
        public void FixturePlayer_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer();

            // Act & Assert - Test that we can set and get each property multiple times
            fixturePlayer.PlayerId = 1;
            fixturePlayer.PlayerId.Should().Be(1);
            fixturePlayer.PlayerId = 2;
            fixturePlayer.PlayerId.Should().Be(2);
            fixturePlayer.PlayerId = null;
            fixturePlayer.PlayerId.Should().BeNull();

            fixturePlayer.ShirtNumber = "10";
            fixturePlayer.ShirtNumber.Should().Be("10");
            fixturePlayer.ShirtNumber = "7";
            fixturePlayer.ShirtNumber.Should().Be("7");
            fixturePlayer.ShirtNumber = null;
            fixturePlayer.ShirtNumber.Should().BeNull();

            fixturePlayer.IsCaptain = true;
            fixturePlayer.IsCaptain.Should().BeTrue();
            fixturePlayer.IsCaptain = false;
            fixturePlayer.IsCaptain.Should().BeFalse();
            fixturePlayer.IsCaptain = null;
            fixturePlayer.IsCaptain.Should().BeNull();
        }

        [Fact]
        public void FixturePlayer_WithRealWorldData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var fixturePlayer = new FixturePlayer
            {
                PlayerId = 7,
                ShirtNumber = "7",
                IsCaptain = true,
                IsStartingLineup = true,
                Position = new IdNamePair { Id = 4, Name = "Forward" },
                State = new IdNamePair { Id = 1, Name = "Playing" },
                Player = new Player
                {
                    Id = 7,
                    Name = "Test Player",
                    TeamId = 101,
                    NationalityId = 1,
                    Type = PlayerType.Player,
                    NationalTeamId = 50
                }
            };

            // Assert
            fixturePlayer.PlayerId.Should().Be(7);
            fixturePlayer.ShirtNumber.Should().Be("7");
            fixturePlayer.IsCaptain.Should().BeTrue();
            fixturePlayer.IsStartingLineup.Should().BeTrue();
            fixturePlayer.Position.Name.Should().Be("Forward");
            fixturePlayer.State.Name.Should().Be("Playing");
            fixturePlayer.Player.Name.Should().Be("Test Player");
        }

        [Fact]
        public void FixturePlayer_WithSubstituteData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var substitute = new FixturePlayer
            {
                PlayerId = 21,
                ShirtNumber = "21",
                IsCaptain = false,
                IsStartingLineup = false,
                Position = new IdNamePair { Id = 3, Name = "Midfielder" },
                State = new IdNamePair { Id = 2, Name = "Bench" },
                Player = new Player
                {
                    Id = 21,
                    Name = "Reserve Player",
                    Type = PlayerType.Player
                }
            };

            // Assert
            substitute.IsCaptain.Should().BeFalse();
            substitute.IsStartingLineup.Should().BeFalse();
            substitute.State.Name.Should().Be("Bench");
        }

        [Fact]
        public void FixturePlayer_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer
            {
                PlayerId = 1,
                ShirtNumber = "10"
            };

            // Act
            var result = fixturePlayer.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("FixturePlayer");
        }

        [Fact]
        public void FixturePlayer_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var fixturePlayer = new FixturePlayer
            {
                PlayerId = 1,
                ShirtNumber = "10"
            };

            // Act
            var hashCode1 = fixturePlayer.GetHashCode();
            var hashCode2 = fixturePlayer.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void FixturePlayer_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var fixturePlayer = new FixturePlayer();

            // Assert
            fixturePlayer.GetType().Should().Be(typeof(FixturePlayer));
            fixturePlayer.GetType().Name.Should().Be("FixturePlayer");
            fixturePlayer.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
        }

        [Fact]
        public void FixturePlayer_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var fixturePlayerType = typeof(FixturePlayer);

            // Assert
            fixturePlayerType.GetProperty("PlayerId")!.PropertyType.Should().Be(typeof(int?));
            fixturePlayerType.GetProperty("ShirtNumber")!.PropertyType.Should().Be(typeof(string));
            fixturePlayerType.GetProperty("IsCaptain")!.PropertyType.Should().Be(typeof(bool?));
            fixturePlayerType.GetProperty("IsStartingLineup")!.PropertyType.Should().Be(typeof(bool?));
            fixturePlayerType.GetProperty("Position")!.PropertyType.Should().Be(typeof(IdNamePair));
            fixturePlayerType.GetProperty("State")!.PropertyType.Should().Be(typeof(IdNamePair));
            fixturePlayerType.GetProperty("Player")!.PropertyType.Should().Be(typeof(Player));
        }

        [Fact]
        public void FixturePlayer_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var player1 = new FixturePlayer { PlayerId = 1, ShirtNumber = "10" };
            var player2 = new FixturePlayer { PlayerId = 2, ShirtNumber = "7" };

            // Assert
            player1.PlayerId.Should().NotBe(player2.PlayerId);
            player1.ShirtNumber.Should().NotBe(player2.ShirtNumber);
            player1.Should().NotBeSameAs(player2);
        }
    }
}

