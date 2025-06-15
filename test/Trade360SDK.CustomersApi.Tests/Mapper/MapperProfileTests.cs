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
} 