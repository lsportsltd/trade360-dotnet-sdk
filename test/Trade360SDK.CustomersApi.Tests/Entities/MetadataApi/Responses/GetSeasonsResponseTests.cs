using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Responses;

public class GetSeasonsResponseTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var response = new GetSeasonsResponse();

        // Assert
        Assert.Null(response.Seasons);
    }

    [Fact]
    public void Seasons_ShouldBeSettable()
    {
        // Arrange
        var expectedSeasons = new[]
        {
            new Season { SeasonId = 12, SeasonName = "2008" },
            new Season { SeasonId = 13, SeasonName = "2009" }
        };

        // Act
        var response = new GetSeasonsResponse { Seasons = expectedSeasons };

        // Assert
        Assert.Equal(expectedSeasons, response.Seasons);
    }

    [Fact]
    public void Seasons_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var response = new GetSeasonsResponse { Seasons = null };

        // Assert
        Assert.Null(response.Seasons);
    }
}

public class SeasonTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var season = new Season();

        // Assert
        Assert.Equal(0, season.SeasonId);
        Assert.Null(season.SeasonName);
    }

    [Fact]
    public void AllProperties_ShouldBeSettable()
    {
        // Arrange
        var expectedSeasonId = 12;
        var expectedSeasonName = "2008";

        // Act
        var season = new Season
        {
            SeasonId = expectedSeasonId,
            SeasonName = expectedSeasonName
        };

        // Assert
        Assert.Equal(expectedSeasonId, season.SeasonId);
        Assert.Equal(expectedSeasonName, season.SeasonName);
    }
}

