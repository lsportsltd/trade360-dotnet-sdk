using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Enums;

public class VenueAssignmentTests
{
    [Fact]
    public void VenueAssignment_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)VenueAssignment.Home);
        Assert.Equal(1, (int)VenueAssignment.Away);
        Assert.Equal(2, (int)VenueAssignment.Neutral);
    }

    [Theory]
    [InlineData(VenueAssignment.Home, 0)]
    [InlineData(VenueAssignment.Away, 1)]
    [InlineData(VenueAssignment.Neutral, 2)]
    public void VenueAssignment_CastToInt_ShouldReturnCorrectValue(VenueAssignment assignment, int expectedValue)
    {
        // Act & Assert
        Assert.Equal(expectedValue, (int)assignment);
    }

    [Theory]
    [InlineData(VenueAssignment.Home, "Home")]
    [InlineData(VenueAssignment.Away, "Away")]
    [InlineData(VenueAssignment.Neutral, "Neutral")]
    public void VenueAssignment_ToString_ShouldReturnEnumName(VenueAssignment assignment, string expectedName)
    {
        // Act & Assert
        Assert.Equal(expectedName, assignment.ToString());
    }

    [Fact]
    public void VenueAssignment_GetValues_ShouldReturnAllValues()
    {
        // Act
        var values = Enum.GetValues<VenueAssignment>();

        // Assert
        Assert.Equal(3, values.Length);
        Assert.Contains(VenueAssignment.Home, values);
        Assert.Contains(VenueAssignment.Away, values);
        Assert.Contains(VenueAssignment.Neutral, values);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(-1, false)]
    public void VenueAssignment_IsDefined_ShouldReturnCorrectResult(int value, bool expectedResult)
    {
        // Act & Assert
        Assert.Equal(expectedResult, Enum.IsDefined(typeof(VenueAssignment), value));
    }

    [Fact]
    public void VenueAssignment_GetNames_ShouldReturnCorrectNames()
    {
        // Act
        var names = Enum.GetNames<VenueAssignment>();

        // Assert
        Assert.Equal(3, names.Length);
        Assert.Contains("Home", names);
        Assert.Contains("Away", names);
        Assert.Contains("Neutral", names);
    }
}
