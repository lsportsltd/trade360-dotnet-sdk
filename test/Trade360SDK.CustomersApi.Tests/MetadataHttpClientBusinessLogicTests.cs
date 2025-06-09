using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Tests;

public class MetadataHttpClientBusinessLogicTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<ILogger<MetadataHttpClient>> _mockLogger;
    private readonly Trade360Settings _settings;
    private readonly MetadataHttpClient _client;

    public MetadataHttpClientBusinessLogicTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<MetadataHttpClient>>();
        
        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/"
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        _client = new MetadataHttpClient(_mockHttpClientFactory.Object, _settings, _mockLogger.Object);
    }

    [Fact]
    public async Task GetSportsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetSportsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<Sport> 
            { 
                new Sport { Id = 1, Name = "Football" },
                new Sport { Id = 2, Name = "Basketball" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetSportsRequest { Languages = new List<int> { 1, 2 } };
        var result = await _client.GetSportsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().Name.Should().Be("Football");
    }

    [Fact]
    public async Task GetLocationsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetLocationsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<Location> 
            { 
                new Location { Id = 1, Name = "England" },
                new Location { Id = 2, Name = "Spain" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetLocationsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetLocationsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().Name.Should().Be("England");
    }

    [Fact]
    public async Task GetLeaguesAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetLeaguesResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<League> 
            { 
                new League { Id = 1, Name = "Premier League" },
                new League { Id = 2, Name = "La Liga" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetLeaguesRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetLeaguesAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().Name.Should().Be("Premier League");
    }

    [Fact]
    public async Task GetMarketsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetMarketsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<Market> 
            { 
                new Market { Id = 1, Name = "1X2" },
                new Market { Id = 2, Name = "Over/Under" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetMarketsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetMarketsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().Name.Should().Be("1X2");
    }

    [Fact]
    public async Task GetTranslationsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new TranslationResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new TranslationBody
            {
                Sports = new List<Sport> { new Sport { Id = 1, Name = "Football" } },
                Leagues = new List<League> { new League { Id = 1, Name = "Premier League" } },
                Markets = new List<Market> { new Market { Id = 1, Name = "1X2" } }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetTranslationsRequest 
        { 
            Languages = new List<int> { 1 },
            SportIds = new List<int> { 1 },
            LeagueIds = new List<int> { 1 },
            MarketIds = new List<int> { 1 }
        };

        var result = await _client.GetTranslationsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Sports.Should().HaveCount(1);
        result.Body.Leagues.Should().HaveCount(1);
        result.Body.Markets.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetCompetitionsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetCompetitionsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<Competition> 
            { 
                new Competition { Id = 1, Name = "World Cup" },
                new Competition { Id = 2, Name = "Champions League" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetCompetitionsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetCompetitionsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().Name.Should().Be("World Cup");
    }

    [Fact]
    public async Task GetIncidentsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetIncidentsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<Incident> 
            { 
                new Incident { Id = 1, Name = "Goal" },
                new Incident { Id = 2, Name = "Yellow Card" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetIncidentsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetIncidentsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().Name.Should().Be("Goal");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetSportsAsync_WithHttpErrors_ShouldReturnErrorResponse(HttpStatusCode statusCode)
    {
        var errorResponse = new GetSportsResponse
        {
            Header = new HeaderResponse 
            { 
                HttpStatusCode = statusCode,
                Errors = new List<string> { $"HTTP {(int)statusCode} error occurred" }
            }
        };

        SetupHttpResponse(errorResponse, statusCode);

        var request = new GetSportsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetSportsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(statusCode);
        result.Header.Errors.Should().Contain($"HTTP {(int)statusCode} error occurred");
    }

    [Fact]
    public async Task GetSportsAsync_WithLargeDataSet_ShouldHandleCorrectly()
    {
        var largeSportsList = Enumerable.Range(1, 1000)
            .Select(i => new Sport { Id = i, Name = $"Sport {i}" })
            .ToList();

        var expectedResponse = new GetSportsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = largeSportsList
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetSportsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetSportsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1000);
        result.Body.Last().Name.Should().Be("Sport 1000");
    }

    [Fact]
    public async Task GetTranslationsAsync_WithMultipleLanguages_ShouldHandleCorrectly()
    {
        var expectedResponse = new TranslationResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new TranslationBody
            {
                Sports = new List<Sport> 
                { 
                    new Sport { Id = 1, Name = "Football" },
                    new Sport { Id = 2, Name = "Basketball" }
                },
                Leagues = new List<League> 
                { 
                    new League { Id = 1, Name = "Premier League" },
                    new League { Id = 2, Name = "NBA" }
                }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetTranslationsRequest 
        { 
            Languages = new List<int> { 1, 2, 3 },
            SportIds = new List<int> { 1, 2 },
            LeagueIds = new List<int> { 1, 2 }
        };

        var result = await _client.GetTranslationsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Sports.Should().HaveCount(2);
        result.Body.Leagues.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSportsAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var request = new GetSportsRequest { Languages = new List<int> { 1 } };
        var act = async () => await _client.GetSportsAsync(request, cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetLocationsAsync_WithEmptyResponse_ShouldHandleCorrectly()
    {
        var expectedResponse = new GetLocationsResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<Location>()
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetLocationsRequest { Languages = new List<int> { 1 } };
        var result = await _client.GetLocationsAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().BeEmpty();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var json = JsonSerializer.Serialize(responseObject);
        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);
    }
}
