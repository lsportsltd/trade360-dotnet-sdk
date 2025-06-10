using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.Common.Entities.Incidents;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class TestHttpClientFactory : IHttpClientFactory
{
    private readonly HttpClient _httpClient;

    public TestHttpClientFactory(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public HttpClient CreateClient(string name) => _httpClient;
    
    public HttpClient CreateClient() => _httpClient;
}

public class MetadataHttpClientBusinessLogicTests
{
    private readonly MetadataHttpClient _client;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IMapper> _mockMapper;

    public MetadataHttpClientBusinessLogicTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockMapper = new Mock<IMapper>();
        
        var packageCredentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "testuser",
            Password = "testpass"
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.example.com/")
        };

        var httpClientFactory = new TestHttpClientFactory(httpClient);
        _client = new MetadataHttpClient(httpClientFactory, "https://api.example.com/", packageCredentials, _mockMapper.Object);
    }

    [Fact]
    public async Task GetSportsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new SportsCollectionResponse
        {
            Sports = new List<Sport> 
            { 
                new Sport { Id = 1, Name = "Football" },
                new Sport { Id = 2, Name = "Basketball" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetSportsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Football");
    }

    [Fact]
    public async Task GetLocationsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new LocationsCollectionResponse
        {
            Locations = new List<Location> 
            { 
                new Location { Id = 1, Name = "England" },
                new Location { Id = 2, Name = "Spain" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetLocationsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("England");
    }

    [Fact]
    public async Task GetLeaguesAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new LeaguesCollectionResponse
        {
            Leagues = new List<League> 
            { 
                new League { Id = 1, Name = "Premier League" },
                new League { Id = 2, Name = "La Liga" }
            }
        };
        var requestDto = new GetLeaguesRequestDto { SportIds = new List<int> { 1 }, LanguageId = 1 };
        var mappedRequest = new GetLeaguesRequest();

        _mockMapper.Setup(x => x.Map<GetLeaguesRequest>(requestDto)).Returns(mappedRequest);
        SetupHttpResponse(expectedResponse);

        var result = await _client.GetLeaguesAsync(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Premier League");
        _mockMapper.Verify(x => x.Map<GetLeaguesRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetMarketsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new MarketsCollectionResponse
        {
            Markets = new List<Market> 
            { 
                new Market { Id = 1, Name = "1X2" },
                new Market { Id = 2, Name = "Over/Under" }
            }
        };
        var requestDto = new GetMarketsRequestDto { SportIds = new List<int> { 1 }, LanguageId = 1 };
        var mappedRequest = new GetMarketsRequest();

        _mockMapper.Setup(x => x.Map<GetMarketsRequest>(requestDto)).Returns(mappedRequest);
        SetupHttpResponse(expectedResponse);

        var result = await _client.GetMarketsAsync(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("1X2");
        _mockMapper.Verify(x => x.Map<GetMarketsRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetTranslationsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new TranslationResponse
        {
            Sports = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Football" } } }
            },
            Leagues = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Premier League" } } }
            },
            Markets = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "1X2" } } }
            }
        };
        var requestDto = new GetTranslationsRequestDto { Languages = new List<string> { "en" }, SportIds = new List<int> { 1 } };
        var mappedRequest = new GetTranslationsRequest 
        { 
            Languages = new List<string> { "en" }, 
            SportIds = new List<int> { 1 } 
        };

        _mockMapper.Setup(x => x.Map<GetTranslationsRequest>(requestDto)).Returns(mappedRequest);
        SetupHttpResponse(expectedResponse);

        var result = await _client.GetTranslationsAsync(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Sports.Should().ContainKey("en");
        result.Leagues.Should().ContainKey("en");
        result.Markets.Should().ContainKey("en");
        _mockMapper.Verify(x => x.Map<GetTranslationsRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetCompetitionsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new CompetitionCollectionResponse
        {
            Competitions = new List<Competition> 
            { 
                new Competition { Id = 1, Name = "World Cup" },
                new Competition { Id = 2, Name = "Champions League" }
            }
        };
        var requestDto = new GetCompetitionsRequestDto { SportIds = new List<int> { 1 }, LanguageId = 1 };
        var mappedRequest = new GetCompetitionsRequest();

        _mockMapper.Setup(x => x.Map<GetCompetitionsRequest>(requestDto)).Returns(mappedRequest);
        SetupHttpResponse(expectedResponse);

        var result = await _client.GetCompetitionsAsync(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Competitions.Should().HaveCount(2);
        result.Competitions.First().Name.Should().Be("World Cup");
        _mockMapper.Verify(x => x.Map<GetCompetitionsRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetIncidentsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetIncidentsResponse
        {
            Data = new List<Incident> 
            { 
                new Incident { IncidentId = 1, IncidentName = "Goal" },
                new Incident { IncidentId = 2, IncidentName = "Yellow Card" }
            }
        };
        var requestDto = new GetIncidentsRequestDto();
        var mappedRequest = new GetIncidentsRequest();

        _mockMapper.Setup(x => x.Map<GetIncidentsRequest>(requestDto)).Returns(mappedRequest);
        SetupHttpResponse(expectedResponse);

        var result = await _client.GetIncidentsAsync(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().IncidentName.Should().Be("Goal");
        _mockMapper.Verify(x => x.Map<GetIncidentsRequest>(requestDto), Times.Once);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetSportsAsync_WithHttpErrors_ShouldHandleErrorResponse(HttpStatusCode statusCode)
    {
        var errorResponse = new SportsCollectionResponse
        {
            Sports = new List<Sport>()
        };

        SetupHttpResponse(errorResponse, statusCode);

        var act = async () => await _client.GetSportsAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task GetSportsAsync_WithLargeDataSet_ShouldHandleCorrectly()
    {
        var largeSportsList = Enumerable.Range(1, 1000)
            .Select(i => new Sport { Id = i, Name = $"Sport {i}" })
            .ToList();

        var expectedResponse = new SportsCollectionResponse
        {
            Sports = largeSportsList
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetSportsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1000);
        result.Last().Name.Should().Be("Sport 1000");
    }

    [Fact]
    public async Task GetTranslationsAsync_WithMultipleLanguages_ShouldHandleCorrectly()
    {
        var expectedResponse = new TranslationResponse
        {
            Sports = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Football" } } },
                { "es", new List<LocalizedValue> { new LocalizedValue { LanguageId = 2, Value = "Fútbol" } } }
            },
            Leagues = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Premier League" } } },
                { "es", new List<LocalizedValue> { new LocalizedValue { LanguageId = 2, Value = "Liga Premier" } } }
            }
        };
        var requestDto = new GetTranslationsRequestDto 
        { 
            Languages = new List<string> { "en", "es" },
            SportIds = new List<int> { 1, 2 },
            LeagueIds = new List<int> { 1, 2 }
        };
        var mappedRequest = new GetTranslationsRequest 
        { 
            Languages = new List<string> { "en", "es" },
            SportIds = new List<int> { 1, 2 },
            LeagueIds = new List<int> { 1, 2 }
        };

        _mockMapper.Setup(x => x.Map<GetTranslationsRequest>(requestDto)).Returns(mappedRequest);
        SetupHttpResponse(expectedResponse);

        var result = await _client.GetTranslationsAsync(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Sports.Should().HaveCount(2);
        result.Leagues.Should().HaveCount(2);
        _mockMapper.Verify(x => x.Map<GetTranslationsRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetSportsAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.Is<CancellationToken>(ct => ct.IsCancellationRequested))
            .ThrowsAsync(new OperationCanceledException());

        var act = async () => await _client.GetSportsAsync(cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetLocationsAsync_WithEmptyResponse_ShouldHandleCorrectly()
    {
        var expectedResponse = new LocationsCollectionResponse
        {
            Locations = new List<Location>()
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetLocationsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<T>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                Errors = new List<Trade360SDK.CustomersApi.Entities.Base.Error>()
            },
            Body = responseObject
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);
    }
}
