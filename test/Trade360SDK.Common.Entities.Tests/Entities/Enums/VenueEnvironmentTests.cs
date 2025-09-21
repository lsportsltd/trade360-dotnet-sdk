using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Enums;

public class VenueEnvironmentTests
{
    [Fact]
    public void VenueEnvironment_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)VenueEnvironment.Indoors);
        Assert.Equal(1, (int)VenueEnvironment.Outdoors);
    }

    [Theory]
    [InlineData(VenueEnvironment.Indoors, 0)]
    [InlineData(VenueEnvironment.Outdoors, 1)]
    public void VenueEnvironment_CastToInt_ShouldReturnCorrectValue(VenueEnvironment environment, int expectedValue)
    {
        // Act & Assert
        Assert.Equal(expectedValue, (int)environment);
    }

    [Theory]
    [InlineData(VenueEnvironment.Indoors, "Indoors")]
    [InlineData(VenueEnvironment.Outdoors, "Outdoors")]
    public void VenueEnvironment_ToString_ShouldReturnEnumName(VenueEnvironment environment, string expectedName)
    {
        // Act & Assert
        Assert.Equal(expectedName, environment.ToString());
    }

    [Fact]
    public void VenueEnvironment_GetValues_ShouldReturnAllValues()
    {
        // Act
        var values = Enum.GetValues<VenueEnvironment>();

        // Assert
        Assert.Equal(2, values.Length);
        Assert.Contains(VenueEnvironment.Indoors, values);
        Assert.Contains(VenueEnvironment.Outdoors, values);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(-1, false)]
    public void VenueEnvironment_IsDefined_ShouldReturnCorrectResult(int value, bool expectedResult)
    {
        // Act & Assert
        Assert.Equal(expectedResult, Enum.IsDefined(typeof(VenueEnvironment), value));
    }

    [Fact]
    public void VenueEnvironment_GetNames_ShouldReturnCorrectNames()
    {
        // Act
        var names = Enum.GetNames<VenueEnvironment>();

        // Assert
        Assert.Equal(2, names.Length);
        Assert.Contains("Indoors", names);
        Assert.Contains("Outdoors", names);
    }
}
