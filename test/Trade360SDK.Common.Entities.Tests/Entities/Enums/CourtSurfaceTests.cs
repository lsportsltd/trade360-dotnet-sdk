using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Enums;

public class CourtSurfaceTests
{
    [Fact]
    public void CourtSurface_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)CourtSurface.Grass);
        Assert.Equal(1, (int)CourtSurface.Hard);
        Assert.Equal(2, (int)CourtSurface.Clay);
        Assert.Equal(3, (int)CourtSurface.ArtificialGrass);
    }

    [Theory]
    [InlineData(CourtSurface.Grass, 0)]
    [InlineData(CourtSurface.Hard, 1)]
    [InlineData(CourtSurface.Clay, 2)]
    [InlineData(CourtSurface.ArtificialGrass, 3)]
    public void CourtSurface_CastToInt_ShouldReturnCorrectValue(CourtSurface surface, int expectedValue)
    {
        // Act & Assert
        Assert.Equal(expectedValue, (int)surface);
    }

    [Theory]
    [InlineData(CourtSurface.Grass, "Grass")]
    [InlineData(CourtSurface.Hard, "Hard")]
    [InlineData(CourtSurface.Clay, "Clay")]
    [InlineData(CourtSurface.ArtificialGrass, "ArtificialGrass")]
    public void CourtSurface_ToString_ShouldReturnEnumName(CourtSurface surface, string expectedName)
    {
        // Act & Assert
        Assert.Equal(expectedName, surface.ToString());
    }

    [Fact]
    public void CourtSurface_GetValues_ShouldReturnAllValues()
    {
        // Act
        var values = Enum.GetValues<CourtSurface>();

        // Assert
        Assert.Equal(4, values.Length);
        Assert.Contains(CourtSurface.Grass, values);
        Assert.Contains(CourtSurface.Hard, values);
        Assert.Contains(CourtSurface.Clay, values);
        Assert.Contains(CourtSurface.ArtificialGrass, values);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(-1, false)]
    public void CourtSurface_IsDefined_ShouldReturnCorrectResult(int value, bool expectedResult)
    {
        // Act & Assert
        Assert.Equal(expectedResult, Enum.IsDefined(typeof(CourtSurface), value));
    }

    [Fact]
    public void CourtSurface_GetNames_ShouldReturnCorrectNames()
    {
        // Act
        var names = Enum.GetNames<CourtSurface>();

        // Assert
        Assert.Equal(4, names.Length);
        Assert.Contains("Grass", names);
        Assert.Contains("Hard", names);
        Assert.Contains("Clay", names);
        Assert.Contains("ArtificialGrass", names);
    }
}
