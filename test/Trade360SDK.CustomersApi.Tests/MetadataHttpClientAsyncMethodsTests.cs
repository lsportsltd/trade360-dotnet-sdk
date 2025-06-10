using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Linq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using AutoMapper;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.Common.Entities.Incidents;

namespace Trade360SDK.CustomersApi.Tests;

public class MetadataHttpClientAsyncMethodsTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Trade360Settings _settings;
    private readonly MetadataHttpClient _client;

    public MetadataHttpClientAsyncMethodsTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
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

        var mockMapper = new Mock<IMapper>();
        SetupAutoMapperDefaults(mockMapper);
        _client = new MetadataHttpClient(_mockHttpClientFactory.Object, _settings.CustomersApiBaseUrl, packageCredentials, mockMapper.Object);
    }

    [Fact]
    public async Task GetSportsAsync_WithValidRequest_ShouldReturnSports()
    {
        var expectedResponse = new SportsCollectionResponse
        {
            Sports = new[]
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
    public async Task GetLocationsAsync_WithValidRequest_ShouldReturnLocations()
    {
        var expectedResponse = new LocationsCollectionResponse
        {
            Locations = new[]
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
    public async Task GetLeaguesAsync_WithValidRequest_ShouldReturnLeagues()
    {
        var expectedResponse = new LeaguesCollectionResponse
        {
            Leagues = new[]
            {
                new League { Id = 1, Name = "Premier League" },
                new League { Id = 2, Name = "La Liga" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetLeaguesAsync(new GetLeaguesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Premier League");
    }

    [Fact]
    public async Task GetMarketsAsync_WithValidRequest_ShouldReturnMarkets()
    {
        var expectedResponse = new MarketsCollectionResponse
        {
            Markets = new[]
            {
                new Market { Id = 1, Name = "Match Winner" },
                new Market { Id = 2, Name = "Over/Under" }
            }
        };

        var mockMapper = new Mock<IMapper>();
        var request = new GetMarketsRequest();
        mockMapper.Setup(m => m.Map<GetMarketsRequest>(It.IsAny<GetMarketsRequestDto>())).Returns(request);

        var client = new MetadataHttpClient(_mockHttpClientFactory.Object, _settings.CustomersApiBaseUrl, 
            new PackageCredentials { Username = "test", Password = "test", PackageId = 123 }, mockMapper.Object);

        SetupHttpResponse(expectedResponse);

        var result = await client.GetMarketsAsync(new GetMarketsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Match Winner");
    }

    [Fact]
    public async Task GetTranslationsAsync_WithValidRequest_ShouldReturnTranslations()
    {
        var expectedResponse = new TranslationResponse
        {
            Sports = new Dictionary<string, List<LocalizedValue>>
            {
                { "1", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Football" } } }
            },
            Leagues = new Dictionary<string, List<LocalizedValue>>
            {
                { "1", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Premier League" } } }
            },
            Locations = new Dictionary<string, List<LocalizedValue>>
            {
                { "1", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "England" } } }
            },
            Markets = new Dictionary<string, List<LocalizedValue>>
            {
                { "1", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Match Winner" } } }
            },
            Participants = new Dictionary<string, List<LocalizedValue>>
            {
                { "1", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Team A" } } }
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetTranslationsAsync(new GetTranslationsRequestDto 
        { 
            Languages = new[] { "en" },
            SportIds = new[] { 1 }
        }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Sports.Should().HaveCount(1);
        result.Leagues.Should().HaveCount(1);
        result.Sports["1"].First().Value.Should().Be("Football");
    }

    [Fact]
    public async Task GetCompetitionsAsync_WithValidRequest_ShouldReturnCompetitions()
    {
        var expectedCompetitions = new CompetitionCollectionResponse
        {
            Competitions = new[]
            {
                new Competition { Id = 1, Name = "World Cup" },
                new Competition { Id = 2, Name = "Champions League" }
            }
        };

        SetupHttpResponse(expectedCompetitions);

        var result = await _client.GetCompetitionsAsync(new GetCompetitionsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Competitions.Should().HaveCount(2);
        result.Competitions.First().Name.Should().Be("World Cup");
    }

    [Fact]
    public async Task GetIncidentsAsync_WithValidRequest_ShouldReturnIncidents()
    {
        var expectedIncidents = new GetIncidentsResponse
        {
            Data = new[]
            {
                new Incident { IncidentId = 1 },
                new Incident { IncidentId = 2 }
            }
        };

        SetupHttpResponse(expectedIncidents);

        var result = await _client.GetIncidentsAsync(new GetIncidentsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().IncidentId.Should().Be(1);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetSportsAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
    {
        var errorResponse = new SportsCollectionResponse { Sports = new Sport[0] };
        SetupHttpResponse(errorResponse, statusCode);

        var act = async () => await _client.GetSportsAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task GetTranslationsAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns((HttpRequestMessage request, CancellationToken token) =>
            {
                token.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        cancellationTokenSource.Cancel();

        var act = async () => await _client.GetTranslationsAsync(new GetTranslationsRequestDto 
        { 
            Languages = new[] { "en" },
            SportIds = new[] { 1 }
        }, cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetLeaguesAsync_WithLargeDataset_ShouldHandleEfficiently()
    {
        var expectedResponse = new LeaguesCollectionResponse
        {
            Leagues = Enumerable.Range(1, 1000)
                .Select(i => new League { Id = i, Name = $"League {i}" })
                .ToArray()
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetLeaguesAsync(new GetLeaguesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1000);
        result.Last().Name.Should().Be("League 1000");
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
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }

    private void SetupAutoMapperDefaults(Mock<IMapper> mockMapper)
    {
        mockMapper.Setup(x => x.Map<GetTranslationsRequest>(It.IsAny<GetTranslationsRequestDto>()))
            .Returns((GetTranslationsRequestDto dto) => new GetTranslationsRequest 
            { 
                Languages = dto?.Languages ?? new List<string> { "en" },
                SportIds = dto?.SportIds ?? new List<int> { 1 },
                LocationIds = dto?.LocationIds,
                LeagueIds = dto?.LeagueIds,
                MarketIds = dto?.MarketIds,
                ParticipantIds = dto?.ParticipantIds
            });

        mockMapper.Setup(x => x.Map<GetLeaguesRequest>(It.IsAny<GetLeaguesRequestDto>()))
            .Returns((GetLeaguesRequestDto dto) => new GetLeaguesRequest());

        mockMapper.Setup(x => x.Map<GetMarketsRequest>(It.IsAny<GetMarketsRequestDto>()))
            .Returns((GetMarketsRequestDto dto) => new GetMarketsRequest());

        mockMapper.Setup(x => x.Map<GetCompetitionsRequest>(It.IsAny<GetCompetitionsRequestDto>()))
            .Returns((GetCompetitionsRequestDto dto) => new GetCompetitionsRequest());

        mockMapper.Setup(x => x.Map<GetIncidentsRequest>(It.IsAny<GetIncidentsRequestDto>()))
            .Returns((GetIncidentsRequestDto dto) => new GetIncidentsRequest());
    }
}
