using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Enums;

public class IncidentTypeTests
{
    [Fact]
    public void IncidentType_ShouldHaveNotSetAsZero()
    {
        var notSet = IncidentType.NotSet;
        
        ((int)notSet).Should().Be(0);
    }

    [Fact]
    public void IncidentType_ShouldHaveExpectedValues()
    {
        // Basic stats
        ((int)IncidentType.Corners).Should().Be(1);
        ((int)IncidentType.ShotsOnTarget).Should().Be(2);
        ((int)IncidentType.ShotsOffTarget).Should().Be(3);
        ((int)IncidentType.Possession).Should().Be(11);
        
        // Cards and penalties
        ((int)IncidentType.YellowCard).Should().Be(6);
        ((int)IncidentType.RedCard).Should().Be(7);
        ((int)IncidentType.MissedPenalty).Should().Be(40);
        ((int)IncidentType.PenaltyGoal).Should().Be(25);
        
        // Goals
        ((int)IncidentType.Goal).Should().Be(9);
        ((int)IncidentType.OwnGoal).Should().Be(24);
    }

    [Fact]
    public void IncidentType_ShouldAllowConversionFromInt()
    {
        var cornerValue = 1;
        var incident = (IncidentType)cornerValue;
        
        incident.Should().Be(IncidentType.Corners);
    }

    [Fact]
    public void IncidentType_ShouldAllowConversionToInt()
    {
        var incident = IncidentType.RedCard;
        var value = (int)incident;
        
        value.Should().Be(7);
    }

    [Theory]
    [InlineData(IncidentType.NotSet, 0)]
    [InlineData(IncidentType.Corners, 1)]
    [InlineData(IncidentType.YellowCard, 6)]
    [InlineData(IncidentType.Goal, 9)]
    [InlineData(IncidentType.PenaltyGoal, 25)]
    [InlineData(IncidentType.PlayerShotsOffTarget, 2006)]
    public void IncidentType_SpecificValues_ShouldMatchExpectedIntegers(IncidentType incident, int expectedValue)
    {
        ((int)incident).Should().Be(expectedValue);
    }

    [Fact]
    public void IncidentType_ShouldBeDefinedEnum()
    {
        var incident = IncidentType.Corners;
        
        Enum.IsDefined(typeof(IncidentType), incident).Should().BeTrue();
    }

    [Fact]
    public void IncidentType_ShouldHavePlayerSpecificIncidents()
    {
        ((int)IncidentType.PlayerShotsOnTarget).Should().Be(2005);
        ((int)IncidentType.PlayerShotsOffTarget).Should().Be(2006);
    }

    [Fact]
    public void IncidentType_ShouldSupportComparison()
    {
        var corner = IncidentType.Corners;
        var goal = IncidentType.Goal;
        
        corner.Should().NotBe(goal);
        corner.Should().Be(IncidentType.Corners);
    }

    [Fact]
    public void IncidentType_ShouldSupportNullable()
    {
        IncidentType? nullableIncident = null;
        
        nullableIncident.Should().BeNull();
        
        nullableIncident = IncidentType.Goal;
        nullableIncident.Should().HaveValue();
        nullableIncident.Value.Should().Be(IncidentType.Goal);
    }

    [Fact]
    public void IncidentType_GetValues_ShouldReturnAllEnumValues()
    {
        var values = Enum.GetValues<IncidentType>();
        
        values.Should().NotBeEmpty();
        values.Should().Contain(IncidentType.NotSet);
        values.Should().Contain(IncidentType.Corners);
        values.Should().Contain(IncidentType.Goal);
    }

    [Theory]
    [InlineData("NotSet", IncidentType.NotSet)]
    [InlineData("Corners", IncidentType.Corners)]
    [InlineData("YellowCard", IncidentType.YellowCard)]
    [InlineData("Goal", IncidentType.Goal)]
    public void IncidentType_Parse_ShouldConvertStringToEnum(string name, IncidentType expected)
    {
        var result = Enum.Parse<IncidentType>(name);
        
        result.Should().Be(expected);
    }

    [Fact]
    public void IncidentType_GetName_ShouldReturnCorrectString()
    {
        var name = Enum.GetName(IncidentType.YellowCard);
        
        name.Should().Be("YellowCard");
    }

    [Fact]
    public void IncidentType_ShouldSupportSwitchStatement()
    {
        var incident = IncidentType.Goal;
        var result = "";

        switch (incident)
        {
            case IncidentType.NotSet:
                result = "Not Set";
                break;
            case IncidentType.Goal:
                result = "Goal";
                break;
            case IncidentType.YellowCard:
                result = "Yellow Card";
                break;
            default:
                result = "Other";
                break;
        }

        result.Should().Be("Goal");
    }
}

