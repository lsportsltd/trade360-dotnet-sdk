using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Linq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Entities.Base;
using Trade360SDK.Common.Entities.Incidents;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class MetadataHttpClientAsyncMethodsComprehensiveTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IOptions<Trade360Settings>> _mockOptions;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Trade360Settings _settings;
    private readonly MetadataHttpClient _client;

    public MetadataHttpClientAsyncMethodsComprehensiveTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockOptions = new Mock<IOptions<Trade360Settings>>();
        _mockMapper = new Mock<IMapper>();
        
        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://customers.example.com/"
        };

        _mockOptions.Setup(x => x.Value).Returns(_settings);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var packageCredentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "testuser",
            Password = "testpass"
        };

        SetupAutoMapperDefaults();
        _client = new MetadataHttpClient(_mockHttpClientFactory.Object, _settings.CustomersApiBaseUrl, packageCredentials, _mockMapper.Object);
    }

    [Fact]
    public async Task GetSportsAsync_WithValidRequest_ShouldReturnSports()
    {
        var expectedSports = new SportsCollectionResponse
        {
            Sports = new[] { new Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Sport { Id = 1, Name = "Football" } }
        };

        SetupHttpResponse(expectedSports);

        var result = await _client.GetSportsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Football");
    }

    [Fact]
    public async Task GetLocationsAsync_WithValidRequest_ShouldReturnLocations()
    {
        var expectedLocations = new LocationsCollectionResponse
        {
            Locations = new[] { new Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Location { Id = 1, Name = "England" } }
        };

        SetupHttpResponse(expectedLocations);

        var result = await _client.GetLocationsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("England");
    }

    [Fact]
    public async Task GetLeaguesAsync_WithValidRequest_ShouldReturnLeagues()
    {
        var expectedLeagues = new LeaguesCollectionResponse
        {
            Leagues = new[] { new Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.League { Id = 1, Name = "Premier League" } }
        };

        SetupHttpResponse(expectedLeagues);

        var result = await _client.GetLeaguesAsync(new GetLeaguesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Premier League");
    }

    [Fact]
    public async Task GetMarketsAsync_WithValidRequest_ShouldReturnMarkets()
    {
        var expectedMarkets = new MarketsCollectionResponse
        {
            Markets = new[] { new Market { Id = 1, Name = "Match Winner" } }
        };

        SetupHttpResponse(expectedMarkets);

        var result = await _client.GetMarketsAsync(new GetMarketsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Match Winner");
    }

    [Fact]
    public async Task GetTranslationsAsync_WithValidRequest_ShouldReturnTranslations()
    {
        var expectedTranslations = new TranslationResponse
        {
            Sports = new Dictionary<string, List<LocalizedValue>>
            {
                { "1", new List<LocalizedValue> { new LocalizedValue { LanguageId = 1, Value = "Football" } } }
            }
        };

        SetupHttpResponse(expectedTranslations);

        var result = await _client.GetTranslationsAsync(new GetTranslationsRequestDto 
        { 
            Languages = new[] { "en" },
            SportIds = new[] { 1 }
        }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Sports.Should().HaveCount(1);
        result.Sports.First().Value.First().Value.Should().Be("Football");
    }

    [Fact]
    public async Task GetCompetitionsAsync_WithValidRequest_ShouldReturnCompetitions()
    {
        var expectedCompetitions = new CompetitionCollectionResponse
        {
            Competitions = new[] { new Competition { Id = 1, Name = "World Cup" } }
        };

        SetupHttpResponse(expectedCompetitions);

        var result = await _client.GetCompetitionsAsync(new GetCompetitionsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Competitions.Should().HaveCount(1);
        result.Competitions.First().Name.Should().Be("World Cup");
    }

    [Fact]
    public async Task GetIncidentsAsync_WithValidRequest_ShouldReturnIncidents()
    {
        var expectedIncidents = new GetIncidentsResponse
        {
            Data = new[] { new Incident { IncidentId = 1 } }
        };

        SetupHttpResponse(expectedIncidents);

        var result = await _client.GetIncidentsAsync(new GetIncidentsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().IncidentId.Should().Be(1);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetSportsAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
    {
        var errorResponse = new SportsCollectionResponse { Sports = new Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Sport[0] };
        SetupHttpResponse(errorResponse, statusCode);

        var act = async () => await _client.GetSportsAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task GetSportsAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns((HttpRequestMessage _, CancellationToken token) =>
            {
                token.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        cancellationTokenSource.Cancel();

        var act = async () => await _client.GetSportsAsync(cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetLocationsAsync_WithLargeDataset_ShouldHandleEfficiently()
    {
        var largeLocationSet = new LocationsCollectionResponse
        {
            Locations = Enumerable.Range(1, 1000)
                .Select(i => new Trade360SDK.CustomersApi.Entities.MetadataApi.Responses.Location { Id = i, Name = $"Location {i}" })
                .ToArray()
        };

        SetupHttpResponse(largeLocationSet);

        var result = await _client.GetLocationsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1000);
        result.Last().Name.Should().Be("Location 1000");
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
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }

    private void SetupAutoMapperDefaults()
    {
        _mockMapper.Setup(x => x.Map<GetTranslationsRequest>(It.IsAny<GetTranslationsRequestDto>()))
            .Returns((GetTranslationsRequestDto dto) => new GetTranslationsRequest 
            { 
                Languages = dto?.Languages ?? new List<string> { "en" },
                SportIds = dto?.SportIds ?? new List<int> { 1 },
                LocationIds = dto?.LocationIds,
                LeagueIds = dto?.LeagueIds,
                MarketIds = dto?.MarketIds,
                ParticipantIds = dto?.ParticipantIds
            });

        _mockMapper.Setup(x => x.Map<GetLeaguesRequest>(It.IsAny<GetLeaguesRequestDto>()))
            .Returns((GetLeaguesRequestDto _) => new GetLeaguesRequest());

        _mockMapper.Setup(x => x.Map<GetMarketsRequest>(It.IsAny<GetMarketsRequestDto>()))
            .Returns((GetMarketsRequestDto _) => new GetMarketsRequest());

        _mockMapper.Setup(x => x.Map<GetCompetitionsRequest>(It.IsAny<GetCompetitionsRequestDto>()))
            .Returns((GetCompetitionsRequestDto _) => new GetCompetitionsRequest());

        _mockMapper.Setup(x => x.Map<GetIncidentsRequest>(It.IsAny<GetIncidentsRequestDto>()))
            .Returns((GetIncidentsRequestDto _) => new GetIncidentsRequest());
    }
}
