using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Livescore;

namespace Trade360SDK.Common.Entities.Tests.Entities.Livescore;

public class CurrentIncidentIncidentTypeTests
{
    [Fact]
    public void Id_ShouldBeNullableIncidentType()
    {
        var incident = new CurrentIncident();
        
        incident.Id.Should().BeNull();
    }

    [Fact]
    public void Id_ShouldAcceptIncidentTypeValues()
    {
        var incident = new CurrentIncident { Id = IncidentType.Goal };
        
        incident.Id.Should().Be(IncidentType.Goal);
    }

    [Theory]
    [InlineData(IncidentType.NotSet)]
    [InlineData(IncidentType.Corners)]
    [InlineData(IncidentType.YellowCard)]
    [InlineData(IncidentType.RedCard)]
    [InlineData(IncidentType.Goal)]
    [InlineData(IncidentType.PenaltyGoal)]
    [InlineData(IncidentType.MissedPenalty)]
    public void Id_ShouldAcceptVariousIncidentTypes(IncidentType incidentType)
    {
        var incident = new CurrentIncident { Id = incidentType };
        
        incident.Id.Should().Be(incidentType);
    }

    [Fact]
    public void Id_ShouldAllowNullValue()
    {
        var incident = new CurrentIncident { Id = null };
        
        incident.Id.Should().BeNull();
    }

    [Fact]
    public void Id_ShouldBeReassignable()
    {
        var incident = new CurrentIncident { Id = IncidentType.Corners };
        
        incident.Id = IncidentType.Goal;
        
        incident.Id.Should().Be(IncidentType.Goal);
    }

    [Fact]
    public void Id_ShouldBeReassignableToNull()
    {
        var incident = new CurrentIncident { Id = IncidentType.Goal };
        
        incident.Id = null;
        
        incident.Id.Should().BeNull();
    }

    [Fact]
    public void Id_WhenSetToGoal_ShouldReturnGoal()
    {
        var incident = new CurrentIncident();
        
        incident.Id = IncidentType.Goal;
        
        incident.Id.Should().Be(IncidentType.Goal);
        incident.Id.Should().NotBe(IncidentType.Corners);
    }

    [Fact]
    public void Id_WhenSetToYellowCard_ShouldHaveCorrectValue()
    {
        var incident = new CurrentIncident { Id = IncidentType.YellowCard };
        
        incident.Id.Should().Be(IncidentType.YellowCard);
        ((int?)incident.Id).Should().Be(6);
    }

    [Fact]
    public void Id_WhenSetToRedCard_ShouldHaveCorrectValue()
    {
        var incident = new CurrentIncident { Id = IncidentType.RedCard };
        
        incident.Id.Should().Be(IncidentType.RedCard);
        ((int?)incident.Id).Should().Be(7);
    }

    [Fact]
    public void Id_CanBeUsedInObjectInitializer()
    {
        var incident = new CurrentIncident
        {
            Id = IncidentType.PenaltyGoal,
            Name = "Penalty Goal",
            Confirmation = IncidentConfirmation.Confirmed
        };
        
        incident.Id.Should().Be(IncidentType.PenaltyGoal);
        incident.Name.Should().Be("Penalty Goal");
    }

    [Fact]
    public void Id_WithPlayerSpecificIncidents_ShouldWork()
    {
        var incident1 = new CurrentIncident { Id = IncidentType.PlayerShotsOnTarget };
        var incident2 = new CurrentIncident { Id = IncidentType.PlayerShotsOffTarget };
        
        incident1.Id.Should().Be(IncidentType.PlayerShotsOnTarget);
        incident2.Id.Should().Be(IncidentType.PlayerShotsOffTarget);
        incident1.Id.Should().NotBe(incident2.Id);
    }

    [Theory]
    [InlineData(IncidentType.Goal, 9)]
    [InlineData(IncidentType.YellowCard, 6)]
    [InlineData(IncidentType.RedCard, 7)]
    [InlineData(IncidentType.Corners, 1)]
    [InlineData(IncidentType.PenaltyGoal, 25)]
    public void Id_ShouldHaveExpectedIntegerValue(IncidentType incidentType, int expectedValue)
    {
        var incident = new CurrentIncident { Id = incidentType };
        
        ((int?)incident.Id).Should().Be(expectedValue);
    }

    [Fact]
    public void Id_ShouldSupportComparison()
    {
        var incident1 = new CurrentIncident { Id = IncidentType.Goal };
        var incident2 = new CurrentIncident { Id = IncidentType.Goal };
        var incident3 = new CurrentIncident { Id = IncidentType.Corners };
        
        incident1.Id.Should().Be(incident2.Id);
        incident1.Id.Should().NotBe(incident3.Id);
    }

    [Fact]
    public void Id_ShouldWorkInConditionals()
    {
        var incident = new CurrentIncident { Id = IncidentType.Goal };
        
        if (incident.Id == IncidentType.Goal)
        {
            incident.Id.Should().Be(IncidentType.Goal);
        }
        else
        {
            Assert.Fail("Conditional should have been true");
        }
    }

    [Fact]
    public void Id_ShouldSupportSwitchStatement()
    {
        var incident = new CurrentIncident { Id = IncidentType.MissedPenalty };
        var result = "";

        switch (incident.Id)
        {
            case IncidentType.Goal:
                result = "Goal";
                break;
            case IncidentType.MissedPenalty:
                result = "Penalty Missed";
                break;
            default:
                result = "Other";
                break;
        }

        result.Should().Be("Penalty Missed");
    }

    [Fact]
    public void Id_WhenNull_ShouldNotEqualAnyIncidentType()
    {
        var incident = new CurrentIncident { Id = null };
        
        incident.Id.Should().NotBe(IncidentType.Goal);
        incident.Id.Should().NotBe(IncidentType.Corners);
        incident.Id.Should().BeNull();
    }

    [Fact]
    public void Id_HasValue_ShouldBeTrueWhenSet()
    {
        var incident = new CurrentIncident { Id = IncidentType.Goal };
        
        incident.Id.HasValue.Should().BeTrue();
        incident.Id.Value.Should().Be(IncidentType.Goal);
    }

    [Fact]
    public void Id_HasValue_ShouldBeFalseWhenNull()
    {
        var incident = new CurrentIncident { Id = null };
        
        incident.Id.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Id_ShouldWorkWithMultipleIncidents()
    {
        var incidents = new[]
        {
            new CurrentIncident { Id = IncidentType.Goal, Name = "Goal" },
            new CurrentIncident { Id = IncidentType.YellowCard, Name = "Yellow Card" },
            new CurrentIncident { Id = IncidentType.Corners, Name = "Corner" },
            new CurrentIncident { Id = null, Name = "Unknown" }
        };

        incidents[0].Id.Should().Be(IncidentType.Goal);
        incidents[1].Id.Should().Be(IncidentType.YellowCard);
        incidents[2].Id.Should().Be(IncidentType.Corners);
        incidents[3].Id.Should().BeNull();
    }

    [Fact]
    public void Id_IndependentFromOtherProperties()
    {
        var incident = new CurrentIncident
        {
            Id = IncidentType.Goal,
            Name = "Goal",
            Confirmation = IncidentConfirmation.Confirmed
        };
        
        incident.Id.Should().Be(IncidentType.Goal);
        incident.Name.Should().Be("Goal");
        
        // Changing Id shouldn't affect other properties
        incident.Id = IncidentType.YellowCard;
        incident.Name.Should().Be("Goal");
        incident.Confirmation.Should().Be(IncidentConfirmation.Confirmed);
    }

    [Fact]
    public void Id_WhenSetToNotSet_ShouldHaveZeroValue()
    {
        var incident = new CurrentIncident { Id = IncidentType.NotSet };
        
        incident.Id.Should().Be(IncidentType.NotSet);
        ((int?)incident.Id).Should().Be(0);
    }

    [Fact]
    public void Id_ShouldSupportOwnGoalScenarios()
    {
        var incident = new CurrentIncident { Id = IncidentType.OwnGoal };
        
        incident.Id.Should().Be(IncidentType.OwnGoal);
        ((int?)incident.Id).Should().Be(24);
    }
}

