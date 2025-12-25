using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests;

public class GetSeasonsRequestDtoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var dto = new GetSeasonsRequestDto();

        // Assert
        Assert.Null(dto.SeasonId);
    }

    [Fact]
    public void SeasonId_ShouldBeSettable()
    {
        // Arrange
        var expectedSeasonId = 12;

        // Act
        var dto = new GetSeasonsRequestDto { SeasonId = expectedSeasonId };

        // Assert
        Assert.Equal(expectedSeasonId, dto.SeasonId);
    }

    [Fact]
    public void SeasonId_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var dto = new GetSeasonsRequestDto { SeasonId = null };

        // Assert
        Assert.Null(dto.SeasonId);
    }
}

