using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests;

public class GetStatesRequestDtoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var dto = new GetStatesRequestDto();

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void Filter_ShouldBeSettable()
    {
        // Arrange
        var expectedFilter = new StateFilterDto
        {
            CountryIds = new[] { 1, 2 },
            StateIds = new[] { 10, 20 }
        };

        // Act
        var dto = new GetStatesRequestDto { Filter = expectedFilter };

        // Assert
        Assert.Equal(expectedFilter, dto.Filter);
        Assert.Equal(new[] { 1, 2 }, dto.Filter.CountryIds);
        Assert.Equal(new[] { 10, 20 }, dto.Filter.StateIds);
    }

    [Fact]
    public void Filter_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var dto = new GetStatesRequestDto { Filter = null };

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void StateFilterDto_DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var filter = new StateFilterDto();

        // Assert
        Assert.Null(filter.CountryIds);
        Assert.Null(filter.StateIds);
    }

    [Fact]
    public void StateFilterDto_Properties_ShouldBeSettable()
    {
        // Arrange
        var countryIds = new[] { 1, 2, 3 };
        var stateIds = new[] { 10, 20 };

        // Act
        var filter = new StateFilterDto
        {
            CountryIds = countryIds,
            StateIds = stateIds
        };

        // Assert
        Assert.Equal(countryIds, filter.CountryIds);
        Assert.Equal(stateIds, filter.StateIds);
    }
}
