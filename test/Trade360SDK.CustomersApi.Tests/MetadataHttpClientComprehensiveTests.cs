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
using Trade360SDK.Common.Entities.Incidents;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class MetadataHttpClientComprehensiveTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<ILogger<MetadataHttpClient>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Trade360Settings _settings;
    private readonly MetadataHttpClient _client;

    public MetadataHttpClientComprehensiveTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<MetadataHttpClient>>();
        _mockMapper = new Mock<IMapper>();
        
        _mockMapper.Setup(m => m.Map<GetTranslationsRequest>(It.IsAny<GetTranslationsRequestDto>()))
            .Returns((GetTranslationsRequestDto dto) => new GetTranslationsRequest
            {
                Languages = dto.Languages ?? new List<string>(),
                SportIds = dto.SportIds ?? new List<int>(),
                LocationIds = dto.LocationIds ?? new List<int>(),
                LeagueIds = dto.LeagueIds ?? new List<int>(),
                MarketIds = dto.MarketIds ?? new List<int>(),
                ParticipantIds = dto.ParticipantIds ?? new List<int>()
            });
            
        _mockMapper.Setup(m => m.Map<GetLeaguesRequest>(It.IsAny<GetLeaguesRequestDto>()))
            .Returns((GetLeaguesRequestDto _) => new GetLeaguesRequest());
            
        _mockMapper.Setup(m => m.Map<GetMarketsRequest>(It.IsAny<GetMarketsRequestDto>()))
            .Returns((GetMarketsRequestDto _) => new GetMarketsRequest());
            
        _mockMapper.Setup(m => m.Map<GetCompetitionsRequest>(It.IsAny<GetCompetitionsRequestDto>()))
            .Returns((GetCompetitionsRequestDto _) => new GetCompetitionsRequest());
            
        _mockMapper.Setup(m => m.Map<GetIncidentsRequest>(It.IsAny<GetIncidentsRequestDto>()))
            .Returns((GetIncidentsRequestDto _) => new GetIncidentsRequest());
        
        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/"
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var packageCredentials = new PackageCredentials
        {
            Username = "test_user",
            Password = "test_password",
            PackageId = 123
        };

        SetupDefaultHttpResponse();
        
        _client = new MetadataHttpClient(_mockHttpClientFactory.Object, _settings.CustomersApiBaseUrl, packageCredentials, _mockMapper.Object);
    }

    private void SetupDefaultHttpResponse()
    {
        var defaultResponse = new SportsCollectionResponse
        {
            Sports = new List<Sport>()
        };

        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<SportsCollectionResponse>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                HttpStatusCode = HttpStatusCode.OK,
                Errors = new List<Trade360SDK.CustomersApi.Entities.Base.Error>()
            },
            Body = defaultResponse
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
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
                new League { Id = 1, Name = "Premier League", SportId = 1 },
                new League { Id = 2, Name = "La Liga", SportId = 1 }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetLeaguesRequestDto();
        var result = await _client.GetLeaguesAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Premier League");
    }

    [Fact]
    public async Task GetMarketsAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new MarketsCollectionResponse
        {
            Markets = new List<Market> 
            { 
                new Market { Id = 1, Name = "Match Winner" },
                new Market { Id = 2, Name = "Over/Under 2.5" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetMarketsRequestDto();
        var result = await _client.GetMarketsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Match Winner");
    }

    [Fact]
    public async Task GetTranslationsAsync_WithValidRequest_ShouldReturnExpectedResponse()
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

        SetupHttpResponse(expectedResponse);

        var request = new GetTranslationsRequestDto 
        { 
            Languages = new[] { "en", "es" },
            SportIds = new[] { 1, 2 }
        };
        var result = await _client.GetTranslationsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Sports.Should().HaveCount(2);
        result.Leagues.Should().HaveCount(2);
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

        SetupHttpResponse(expectedResponse);

        var request = new GetCompetitionsRequestDto();
        var result = await _client.GetCompetitionsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Competitions.Should().HaveCount(2);
        result.Competitions.First().Name.Should().Be("World Cup");
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

        SetupHttpResponse(expectedResponse);

        var request = new GetIncidentsRequestDto();
        var result = await _client.GetIncidentsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().IncidentName.Should().Be("Goal");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetSportsAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
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
    public async Task GetTranslationsAsync_WithLargeDataSet_ShouldHandleCorrectly()
    {
        var largeTranslationSet = new TranslationResponse
        {
            Sports = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", Enumerable.Range(1, 100).Select(i => new LocalizedValue { LanguageId = 1, Value = $"Sport {i}" }).ToList() },
                { "es", Enumerable.Range(1, 100).Select(i => new LocalizedValue { LanguageId = 2, Value = $"Deporte {i}" }).ToList() }
            },
            Leagues = new Dictionary<string, List<LocalizedValue>>
            {
                { "en", Enumerable.Range(1, 500).Select(i => new LocalizedValue { LanguageId = 1, Value = $"League {i}" }).ToList() },
                { "es", Enumerable.Range(1, 500).Select(i => new LocalizedValue { LanguageId = 2, Value = $"Liga {i}" }).ToList() }
            }
        };

        SetupHttpResponse(largeTranslationSet);

        var request = new GetTranslationsRequestDto 
        { 
            Languages = new[] { "en", "es" },
            SportIds = new[] { 1, 2 }
        };

        var result = await _client.GetTranslationsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Sports.Should().HaveCount(2);
        result.Leagues.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMarketsAsync_WithEmptyResponse_ShouldReturnEmptyCollection()
    {
        var emptyResponse = new MarketsCollectionResponse
        {
            Markets = new List<Market>()
        };

        SetupHttpResponse(emptyResponse);

        var request = new GetMarketsRequestDto();
        var result = await _client.GetMarketsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCompetitionsAsync_WithFilteredRequest_ShouldReturnFilteredResults()
    {
        var expectedResponse = new CompetitionCollectionResponse
        {
            Competitions = new List<Competition> 
            { 
                new Competition { Id = 1, Name = "World Cup" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetCompetitionsRequestDto();

        var result = await _client.GetCompetitionsAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Competitions.Should().HaveCount(1);
        result.Competitions.First().Name.Should().Be("World Cup");
    }

    [Fact]
    public async Task GetIncidentsAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var request = new GetIncidentsRequestDto();

        var act = async () => await _client.GetIncidentsAsync(request, cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<T>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                HttpStatusCode = statusCode,
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
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post || req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }
}
