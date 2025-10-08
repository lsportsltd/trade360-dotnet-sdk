using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using System.Text.Json;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Mapper;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests;

public class MapperProfileTests
{
    private readonly IMapper _mapper;

    public MapperProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_GetFixtureMetadataRequestDto_To_GetFixtureMetadataRequest_MapsDatesCorrectly()
    {
        var dto = new GetFixtureMetadataRequestDto
        {
            FromDate = new DateTime(2024, 6, 1),
            ToDate = new DateTime(2024, 6, 30),
            SportIds = new List<int> { 1 },
            LocationIds = new List<int> { 2 },
            LeagueIds = new List<int> { 3 }
        };

        var result = _mapper.Map<GetFixtureMetadataRequest>(dto);
        result.FromDate.Should().Be("06/01/2024");
        result.ToDate.Should().Be("06/30/2024");
        result.SportIds.Should().BeEquivalentTo(new List<int> { 1 });
        result.LocationIds.Should().BeEquivalentTo(new List<int> { 2 });
        result.LeagueIds.Should().BeEquivalentTo(new List<int> { 3 });
    }

    [Fact]
    public void Map_GetTranslationsRequestDto_To_GetTranslationsRequest_MapsAllFields()
    {
        var dto = new GetTranslationsRequestDto
        {
            Languages = new List<int> { 1, 2 },
            SportIds = new List<int> { 3 },
            LocationIds = new List<int> { 4 },
            LeagueIds = new List<int> { 5 },
            MarketIds = new List<int> { 6 },
            ParticipantIds = new List<int> { 7 }
        };

        var result = _mapper.Map<GetTranslationsRequest>(dto);
        result.Languages.Should().BeEquivalentTo(new List<int> { 1, 2 });
        result.SportIds.Should().BeEquivalentTo(new List<int> { 3 });
        result.LocationIds.Should().BeEquivalentTo(new List<int> { 4 });
        result.LeagueIds.Should().BeEquivalentTo(new List<int> { 5 });
        result.MarketIds.Should().BeEquivalentTo(new List<int> { 6 });
        result.ParticipantIds.Should().BeEquivalentTo(new List<int> { 7 });
    }

    [Fact]
    public void SerializeDeserialize_GetTranslationsRequestDto_ShouldPreserveData()
    {
        var dto = new GetTranslationsRequestDto
        {
            Languages = new List<int> { 1, 2 },
            SportIds = new List<int> { 3 },
            LocationIds = new List<int> { 4 },
            LeagueIds = new List<int> { 5 },
            MarketIds = new List<int> { 6 },
            ParticipantIds = new List<int> { 7 },
            LanguageId = 99
        };

        var json = JsonSerializer.Serialize(dto);
        var deserialized = JsonSerializer.Deserialize<GetTranslationsRequestDto>(json);
        deserialized.Should().NotBeNull();
        deserialized!.Languages.Should().BeEquivalentTo(new List<int> { 1, 2 });
        deserialized.SportIds.Should().BeEquivalentTo(new List<int> { 3 });
        deserialized.LocationIds.Should().BeEquivalentTo(new List<int> { 4 });
        deserialized.LeagueIds.Should().BeEquivalentTo(new List<int> { 5 });
        deserialized.MarketIds.Should().BeEquivalentTo(new List<int> { 6 });
        deserialized.ParticipantIds.Should().BeEquivalentTo(new List<int> { 7 });
        deserialized.LanguageId.Should().Be(99);
    }

    [Fact]
    public void Map_GetCitiesRequestDto_To_GetCitiesRequest_MapsFilter()
    {
        var dto = new GetCitiesRequestDto
        {
            Filter = new CityFilterDto { StateIds = new[] { 123 } }
        };

        var result = _mapper.Map<GetCitiesRequest>(dto);
        result.Filter?.StateIds.Should().Contain(123);
    }

    [Fact]
    public void Map_GetCitiesRequestDto_WithNullFilter_To_GetCitiesRequest_MapsNull()
    {
        var dto = new GetCitiesRequestDto
        {
            Filter = null
        };

        var result = _mapper.Map<GetCitiesRequest>(dto);
        result.Filter.Should().BeNull();
    }

    [Fact]
    public void Map_GetStatesRequestDto_To_GetStatesRequest_MapsFilter()
    {
        var dto = new GetStatesRequestDto
        {
            Filter = new StateFilterDto { CountryIds = new[] { 456 } }
        };

        var result = _mapper.Map<GetStatesRequest>(dto);
        result.Filter?.CountryIds.Should().Contain(456);
    }

    [Fact]
    public void Map_GetStatesRequestDto_WithNullFilter_To_GetStatesRequest_MapsNull()
    {
        var dto = new GetStatesRequestDto
        {
            Filter = null
        };

        var result = _mapper.Map<GetStatesRequest>(dto);
        result.Filter.Should().BeNull();
    }

    [Fact]
    public void Map_GetVenuesRequestDto_To_GetVenuesRequest_MapsFilter()
    {
        var dto = new GetVenuesRequestDto
        {
            Filter = new VenueFilterDto { CityIds = new[] { 789 } }
        };

        var result = _mapper.Map<GetVenuesRequest>(dto);
        result.Filter?.CityIds.Should().Contain(789);
    }

    [Fact]
    public void Map_GetVenuesRequestDto_WithNullFilter_To_GetVenuesRequest_MapsNull()
    {
        var dto = new GetVenuesRequestDto
        {
            Filter = null
        };

        var result = _mapper.Map<GetVenuesRequest>(dto);
        result.Filter.Should().BeNull();
    }

    [Fact]
    public void SerializeDeserialize_GetCitiesRequestDto_ShouldPreserveData()
    {
        var dto = new GetCitiesRequestDto
        {
            Filter = new CityFilterDto { StateIds = new[] { 123 } }
        };

        var json = JsonSerializer.Serialize(dto);
        var deserialized = JsonSerializer.Deserialize<GetCitiesRequestDto>(json);
        deserialized.Should().NotBeNull();
        deserialized!.Filter?.StateIds.Should().Contain(123);
    }

    [Fact]
    public void SerializeDeserialize_GetStatesRequestDto_ShouldPreserveData()
    {
        var dto = new GetStatesRequestDto
        {
            Filter = new StateFilterDto { CountryIds = new[] { 456 } }
        };

        var json = JsonSerializer.Serialize(dto);
        var deserialized = JsonSerializer.Deserialize<GetStatesRequestDto>(json);
        deserialized.Should().NotBeNull();
        deserialized!.Filter?.CountryIds.Should().Contain(456);
    }

    [Fact]
    public void SerializeDeserialize_GetVenuesRequestDto_ShouldPreserveData()
    {
        var dto = new GetVenuesRequestDto
        {
            Filter = new VenueFilterDto { CityIds = new[] { 789 } }
        };

        var json = JsonSerializer.Serialize(dto);
        var deserialized = JsonSerializer.Deserialize<GetVenuesRequestDto>(json);
        deserialized.Should().NotBeNull();
        deserialized!.Filter?.CityIds.Should().Contain(789);
    }
} 