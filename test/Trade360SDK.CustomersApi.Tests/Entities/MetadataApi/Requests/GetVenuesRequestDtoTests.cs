using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests;

public class GetVenuesRequestDtoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var dto = new GetVenuesRequestDto();

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void Filter_ShouldBeSettable()
    {
        // Arrange
        var expectedFilter = new VenueFilterDto
        {
            VenueIds = new[] { 1, 2 },
            CountryIds = new[] { 10, 20 },
            StateIds = new[] { 100, 200 },
            CityIds = new[] { 1000, 2000 }
        };

        // Act
        var dto = new GetVenuesRequestDto { Filter = expectedFilter };

        // Assert
        Assert.Equal(expectedFilter, dto.Filter);
        Assert.Equal(new[] { 1, 2 }, dto.Filter.VenueIds);
        Assert.Equal(new[] { 10, 20 }, dto.Filter.CountryIds);
        Assert.Equal(new[] { 100, 200 }, dto.Filter.StateIds);
        Assert.Equal(new[] { 1000, 2000 }, dto.Filter.CityIds);
    }

    [Fact]
    public void Filter_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var dto = new GetVenuesRequestDto { Filter = null };

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void VenueFilterDto_DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var filter = new VenueFilterDto();

        // Assert
        Assert.Null(filter.VenueIds);
        Assert.Null(filter.CountryIds);
        Assert.Null(filter.StateIds);
        Assert.Null(filter.CityIds);
    }

    [Fact]
    public void VenueFilterDto_Properties_ShouldBeSettable()
    {
        // Arrange
        var venueIds = new[] { 1, 2 };
        var countryIds = new[] { 10, 20, 30 };
        var stateIds = new[] { 100, 200 };
        var cityIds = new[] { 1000 };

        // Act
        var filter = new VenueFilterDto
        {
            VenueIds = venueIds,
            CountryIds = countryIds,
            StateIds = stateIds,
            CityIds = cityIds
        };

        // Assert
        Assert.Equal(venueIds, filter.VenueIds);
        Assert.Equal(countryIds, filter.CountryIds);
        Assert.Equal(stateIds, filter.StateIds);
        Assert.Equal(cityIds, filter.CityIds);
    }
}
