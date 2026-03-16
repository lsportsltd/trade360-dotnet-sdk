using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.BetBuilderApi.Requests
{
    public class BetBuilderPlayerTests
    {
        [Fact]
        public void BetBuilderPlayer_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var player = new BetBuilderPlayer();

            player.Should().NotBeNull();
            player.Position.Should().BeNull();
            player.Starting.Should().BeNull();
            player.RushingYards.Should().BeNull();
            player.ReceivingYards.Should().BeNull();
            player.PassingYards.Should().BeNull();
            player.TotalTouchdowns.Should().BeNull();
            player.RushingTouchdowns.Should().BeNull();
            player.ReceivingTouchdowns.Should().BeNull();
            player.PassingTouchdowns.Should().BeNull();
            player.PassCompletions.Should().BeNull();
            player.Receptions.Should().BeNull();
            player.Interceptions.Should().BeNull();
            player.RushingAttempts.Should().BeNull();
            player.Fieldgoals.Should().BeNull();
        }

        [Fact]
        public void BetBuilderPlayer_SetProperties_ShouldReturnCorrectValues()
        {
            var player = new BetBuilderPlayer
            {
                Position = "QB",
                Starting = "true",
                RushingYards = 50,
                PassingYards = 300,
                TotalTouchdowns = 3,
                PassCompletions = 25
            };

            player.Position.Should().Be("QB");
            player.Starting.Should().Be("true");
            player.RushingYards.Should().Be(50);
            player.PassingYards.Should().Be(300);
            player.TotalTouchdowns.Should().Be(3);
            player.PassCompletions.Should().Be(25);
        }
    }
}
