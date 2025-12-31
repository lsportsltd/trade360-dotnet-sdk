using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Responses;

public class GetToursResponseTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var response = new GetToursResponse();

        // Assert
        Assert.Null(response.Tours);
    }

    [Fact]
    public void Tours_ShouldBeSettable()
    {
        // Arrange
        var expectedTours = new[]
        {
            new Tour { TourId = 1, TourName = "ATP", SportId = 54094, SportName = "Tennis" },
            new Tour { TourId = 2, TourName = "WTA", SportId = 54094, SportName = "Tennis" }
        };

        // Act
        var response = new GetToursResponse { Tours = expectedTours };

        // Assert
        Assert.Equal(expectedTours, response.Tours);
    }

    [Fact]
    public void Tours_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var response = new GetToursResponse { Tours = null };

        // Assert
        Assert.Null(response.Tours);
    }
}

public class TourTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var tour = new Tour();

        // Assert
        Assert.Equal(0, tour.TourId);
        Assert.Null(tour.TourName);
        Assert.Equal(0, tour.SportId);
        Assert.Null(tour.SportName);
    }

    [Fact]
    public void AllProperties_ShouldBeSettable()
    {
        // Arrange
        var expectedTourId = 1;
        var expectedTourName = "ATP";
        var expectedSportId = 54094;
        var expectedSportName = "Tennis";

        // Act
        var tour = new Tour
        {
            TourId = expectedTourId,
            TourName = expectedTourName,
            SportId = expectedSportId,
            SportName = expectedSportName
        };

        // Assert
        Assert.Equal(expectedTourId, tour.TourId);
        Assert.Equal(expectedTourName, tour.TourName);
        Assert.Equal(expectedSportId, tour.SportId);
        Assert.Equal(expectedSportName, tour.SportName);
    }
}

