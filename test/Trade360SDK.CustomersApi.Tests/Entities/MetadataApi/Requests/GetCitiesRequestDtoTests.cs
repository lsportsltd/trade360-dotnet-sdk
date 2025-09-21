using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests;

public class GetCitiesRequestDtoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var dto = new GetCitiesRequestDto();

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void Filter_ShouldBeSettable()
    {
        // Arrange
        var expectedFilter = new CityFilterDto
        {
            CountryIds = new[] { 1, 2 },
            StateIds = new[] { 10, 20 },
            CityIds = new[] { 100, 200 }
        };

        // Act
        var dto = new GetCitiesRequestDto { Filter = expectedFilter };

        // Assert
        Assert.Equal(expectedFilter, dto.Filter);
        Assert.Equal(new[] { 1, 2 }, dto.Filter.CountryIds);
        Assert.Equal(new[] { 10, 20 }, dto.Filter.StateIds);
        Assert.Equal(new[] { 100, 200 }, dto.Filter.CityIds);
    }

    [Fact]
    public void Filter_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var dto = new GetCitiesRequestDto { Filter = null };

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void CityFilterDto_DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var filter = new CityFilterDto();

        // Assert
        Assert.Null(filter.CountryIds);
        Assert.Null(filter.StateIds);
        Assert.Null(filter.CityIds);
    }

    [Fact]
    public void CityFilterDto_Properties_ShouldBeSettable()
    {
        // Arrange
        var countryIds = new[] { 1, 2, 3 };
        var stateIds = new[] { 10, 20 };
        var cityIds = new[] { 100 };

        // Act
        var filter = new CityFilterDto
        {
            CountryIds = countryIds,
            StateIds = stateIds,
            CityIds = cityIds
        };

        // Assert
        Assert.Equal(countryIds, filter.CountryIds);
        Assert.Equal(stateIds, filter.StateIds);
        Assert.Equal(cityIds, filter.CityIds);
    }
}
