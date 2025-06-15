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
using Trade360SDK.CustomersApi.Mapper;
using Xunit;

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
        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<SportsCollectionResponse>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse { RequestId = Guid.NewGuid().ToString() },
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