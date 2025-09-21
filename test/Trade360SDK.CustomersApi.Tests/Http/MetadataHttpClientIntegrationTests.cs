using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Entities.Base;
using Trade360SDK.CustomersApi.Mapper;
using Xunit;
using Sport = Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Sport;

namespace Trade360SDK.CustomersApi.Tests;

public class MetadataHttpClientIntegrationTests
{
    private readonly TestServer _server;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly MetadataHttpClient _client;

    public MetadataHttpClientIntegrationTests()
    {
        // Setup TestServer with a simple handler
        _server = new TestServer(new WebHostBuilder()
            .Configure(app => { })
            .ConfigureServices(services => { }));
        _httpClient = _server.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost");

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        var httpClientFactory = new TestHttpClientFactory(_httpClient);
        var packageCredentials = new PackageCredentials { PackageId = 1, Username = "user", Password = "pass" };
        _client = new MetadataHttpClient(httpClientFactory, "http://localhost", packageCredentials, _mapper);
    }

    [Fact]
    public async Task GetSportsAsync_Integration_ReturnsExpectedSports()
    {
        // Arrange: Setup a fake HTTP response
        var expectedSports = new List<Sport> { new Sport { Id = 1, Name = "Football" }, new Sport { Id = 2, Name = "Basketball" } };
        var baseResponse = new BaseResponse<SportsCollectionResponse>
        {
            Header = new HeaderResponse { RequestId = Guid.NewGuid().ToString() },
            Body = new SportsCollectionResponse { Sports = expectedSports }
        };
        var json = JsonSerializer.Serialize(baseResponse);
        var messageHandler = new FakeHttpMessageHandler(json);
        var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://localhost") };
        var httpClientFactory = new TestHttpClientFactory(httpClient);
        var packageCredentials = new PackageCredentials { PackageId = 1, Username = "user", Password = "pass" };
        var client = new MetadataHttpClient(httpClientFactory, "http://localhost", packageCredentials, _mapper);

        // Act
        var result = await client.GetSportsAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Football");
    }

    [Fact]
    public async Task GetCitiesAsync_Integration_ReturnsExpectedCities()
    {
        // Arrange: Setup a fake HTTP response
        var expectedCities = new List<City> 
        { 
            new City { CityId = 1, Name = "New York", State = new IdNamePair { Id = 10, Name = "New York State" } }, 
            new City { CityId = 2, Name = "Los Angeles", State = new IdNamePair { Id = 20, Name = "California" } } 
        };
        var baseResponse = new BaseResponse<GetCitiesResponse>
        {
            Header = new HeaderResponse { RequestId = Guid.NewGuid().ToString() },
            Body = new GetCitiesResponse { Data = expectedCities }
        };
        var json = JsonSerializer.Serialize(baseResponse);
        var messageHandler = new FakeHttpMessageHandler(json);
        var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://localhost") };
        var httpClientFactory = new TestHttpClientFactory(httpClient);
        var packageCredentials = new PackageCredentials { PackageId = 1, Username = "user", Password = "pass" };
        var client = new MetadataHttpClient(httpClientFactory, "http://localhost", packageCredentials, _mapper);

        // Act
        var result = await client.GetCitiesAsync(new GetCitiesRequestDto { Filter = new CityFilterDto { StateIds = new[] { 10 } } }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("New York");
        result.First().CityId.Should().Be(1);
        result.First().State?.Id.Should().Be(10);
    }

    [Fact]
    public async Task GetStatesAsync_Integration_ReturnsExpectedStates()
    {
        // Arrange: Setup a fake HTTP response
        var expectedStates = new List<State> 
        { 
            new State { StateId = 1, Name = "California", Country = new IdNamePair { Id = 100, Name = "USA" } }, 
            new State { StateId = 2, Name = "Texas", Country = new IdNamePair { Id = 100, Name = "USA" } } 
        };
        var baseResponse = new BaseResponse<GetStatesResponse>
        {
            Header = new HeaderResponse { RequestId = Guid.NewGuid().ToString() },
            Body = new GetStatesResponse { Data = expectedStates }
        };
        var json = JsonSerializer.Serialize(baseResponse);
        var messageHandler = new FakeHttpMessageHandler(json);
        var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://localhost") };
        var httpClientFactory = new TestHttpClientFactory(httpClient);
        var packageCredentials = new PackageCredentials { PackageId = 1, Username = "user", Password = "pass" };
        var client = new MetadataHttpClient(httpClientFactory, "http://localhost", packageCredentials, _mapper);

        // Act
        var result = await client.GetStatesAsync(new GetStatesRequestDto { Filter = new StateFilterDto { CountryIds = new[] { 100 } } }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("California");
        result.First().StateId.Should().Be(1);
        result.First().Country?.Id.Should().Be(100);
    }

    [Fact]
    public async Task GetVenuesAsync_Integration_ReturnsExpectedVenues()
    {
        // Arrange: Setup a fake HTTP response
        var expectedVenues = new List<Venue> 
        { 
            new Venue { VenueId = 1, Name = "Wembley Stadium", City = new IdNamePair { Id = 10, Name = "London" } }, 
            new Venue { VenueId = 2, Name = "Madison Square Garden", City = new IdNamePair { Id = 20, Name = "New York" } } 
        };
        var baseResponse = new BaseResponse<GetVenuesResponse>
        {
            Header = new HeaderResponse { RequestId = Guid.NewGuid().ToString() },
            Body = new GetVenuesResponse { Data = expectedVenues }
        };
        var json = JsonSerializer.Serialize(baseResponse);
        var messageHandler = new FakeHttpMessageHandler(json);
        var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://localhost") };
        var httpClientFactory = new TestHttpClientFactory(httpClient);
        var packageCredentials = new PackageCredentials { PackageId = 1, Username = "user", Password = "pass" };
        var client = new MetadataHttpClient(httpClientFactory, "http://localhost", packageCredentials, _mapper);

        // Act
        var result = await client.GetVenuesAsync(new GetVenuesRequestDto { Filter = new VenueFilterDto { CityIds = new[] { 10 } } }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Wembley Stadium");
        result.First().VenueId.Should().Be(1);
        result.First().City?.Id.Should().Be(10);
        result.First().City?.Name.Should().Be("London");
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _json;
        public FakeHttpMessageHandler(string json) => _json = json;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }

    private class TestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;
        public TestHttpClientFactory(HttpClient httpClient) => _httpClient = httpClient;
        public HttpClient CreateClient(string name) => _httpClient;
        public HttpClient CreateClient() => _httpClient;
    }
} 