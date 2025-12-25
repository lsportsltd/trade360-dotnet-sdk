using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests;

public class GetToursRequestDtoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var dto = new GetToursRequestDto();

        // Assert
        Assert.Null(dto.TourId);
        Assert.Equal(0, dto.SportId);
    }

    [Fact]
    public void TourId_ShouldBeSettable()
    {
        // Arrange
        var expectedTourId = 1;

        // Act
        var dto = new GetToursRequestDto { TourId = expectedTourId };

        // Assert
        Assert.Equal(expectedTourId, dto.TourId);
    }

    [Fact]
    public void TourId_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var dto = new GetToursRequestDto { TourId = null };

        // Assert
        Assert.Null(dto.TourId);
    }

    [Fact]
    public void SportId_ShouldBeSettable()
    {
        // Arrange
        var expectedSportId = 54094;

        // Act
        var dto = new GetToursRequestDto { SportId = expectedSportId };

        // Assert
        Assert.Equal(expectedSportId, dto.SportId);
    }

    [Fact]
    public void AllProperties_ShouldBeSettable()
    {
        // Arrange
        var expectedTourId = 1;
        var expectedSportId = 54094;

        // Act
        var dto = new GetToursRequestDto
        {
            TourId = expectedTourId,
            SportId = expectedSportId
        };

        // Assert
        Assert.Equal(expectedTourId, dto.TourId);
        Assert.Equal(expectedSportId, dto.SportId);
    }
}

