using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Livescore;
using AutoMapper;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotPrematchApiClientAsyncMethodsTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SnapshotPrematchApiClient _client;

    public SnapshotPrematchApiClientAsyncMethodsTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockOptions = new Mock<IOptions<Trade360Settings>>();
        _mockMapper = new Mock<IMapper>();
        
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://snapshot.example.com/",
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };

        mockOptions.Setup(x => x.Value).Returns(settings);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(settings.SnapshotApiBaseUrl)
        };

        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _client = new SnapshotPrematchApiClient(mockHttpClientFactory.Object, mockOptions.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetFixtures_WithValidRequest_ShouldReturnFixtures()
    {
        var expectedFixtures = new[]
        {
            new FixtureEvent { FixtureId = 1 },
            new FixtureEvent { FixtureId = 2 }
        };

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedFixtures);

        var result = await _client.GetFixtures(new GetFixturesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().FixtureId.Should().Be(1);
    }

    [Fact]
    public async Task GetFixtureMarkets_WithValidRequest_ShouldReturnMarkets()
    {
        var expectedMarkets = new[]
        {
            new MarketEvent { FixtureId = 1, Markets = new[] { new Market { Id = 100 } } },
            new MarketEvent { FixtureId = 1, Markets = new[] { new Market { Id = 200 } } }
        };

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedMarkets);

        var result = await _client.GetFixtureMarkets(new GetMarketRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Markets!.First().Id.Should().Be(100);
    }

    [Fact]
    public async Task GetEvents_WithValidRequest_ShouldReturnEvents()
    {
        var expectedEvents = new[]
        {
            new GetEventsResponse { FixtureId = 1 },
            new GetEventsResponse { FixtureId = 2 }
        };

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedEvents);

        var result = await _client.GetEvents(new GetMarketRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().FixtureId.Should().Be(1);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetFixtures_WithHttpErrors_ShouldHandleGracefully(HttpStatusCode statusCode)
    {
        var errorFixtures = new FixtureEvent[0];
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(errorFixtures, statusCode);

        var result = await _client.GetFixtures(new GetFixturesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFixtures_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException())
            .Verifiable();

        var act = async () => await _client.GetFixtures(new GetFixturesRequestDto(), cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetLivescore_WithLargeDataset_ShouldHandleEfficiently()
    {
        var largeLivescoreSet = Enumerable.Range(1, 1000)
            .Select(i => new Trade360SDK.Common.Entities.Livescore.LivescoreEvent { FixtureId = i })
            .ToArray();

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(largeLivescoreSet);

        var result = await _client.GetLivescore(new GetLivescoreRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1000);
        result.Last().FixtureId.Should().Be(1000);
    }

    [Fact]
    public async Task GetOutrightLeagues_WithNetworkTimeout_ShouldHandleTimeout()
    {
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("Request timeout"))
            .Verifiable();

        var act = async () => await _client.GetOutrightLeagues(new GetFixturesRequestDto(), CancellationToken.None);

        await act.Should().ThrowAsync<TaskCanceledException>();
    }

    [Fact]
    public async Task GetFixtures_WithEmptyResponse_ShouldReturnEmptyCollection()
    {
        var emptyFixtures = new FixtureEvent[0];
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(emptyFixtures);

        var result = await _client.GetFixtures(new GetFixturesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetOutrightFixtureMarkets_WithSpecificRequest_ShouldReturnCorrectData()
    {
        var expectedMarkets = new[]
        {
            new GetOutrightMarketsResponse { Id = 1, Name = "Test Market", Type = 1 }
        };

        var baseRequest = new BaseOutrightRequest();
        _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(It.IsAny<GetOutrightMarketsRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedMarkets);

        var result = await _client.GetOutrightFixtureMarkets(new GetOutrightMarketsRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Id.Should().Be(1);
    }

    [Fact]
    public async Task GetOutrightLeagueEvents_WithValidRequest_ShouldReturnEvents()
    {
        var eventsWrapper = new Trade360SDK.Common.Entities.OutrightLeague.OutrightLeagueEventsWrapper<Trade360SDK.Common.Entities.OutrightLeague.OutrightLeagueEvent>
        {
            Id = 1,
            Name = "Test Events Wrapper",
            Events = new[] { new Trade360SDK.Common.Entities.OutrightLeague.OutrightLeagueEvent { FixtureId = 1 } }
        };
        
        var expectedEvents = new[]
        {
            new GetOutrightLeagueEventsResponse 
            { 
                Competition = new[] 
                { 
                    new Trade360SDK.Common.Entities.OutrightLeague.OutrightLeagueCompetitionWrapper<Trade360SDK.Common.Entities.OutrightLeague.OutrightLeagueEvent>
                    {
                        Id = 100,
                        Name = "Test Competition",
                        Competitions = new[] { eventsWrapper }
                    }
                }
            }
        };

        var baseRequest = new BaseOutrightRequest();
        _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(It.IsAny<GetOutrightFixturesRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedEvents);

        var result = await _client.GetOutrightLeagueEvents(new GetOutrightFixturesRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Competition.Should().HaveCount(1);
        result.First().Competition.First().Competitions.Should().HaveCount(1);
        result.First().Competition.First().Competitions.First().Events.Should().HaveCount(1);
        result.First().Competition.First().Competitions.First().Events.First().FixtureId.Should().Be(1);
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new Trade360SDK.SnapshotApi.Http.BaseResponse<T>
        {
            Header = new Trade360SDK.Common.Models.MessageHeader
            {
                CreationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Type = 1,
                MsgSeq = 1,
                MsgGuid = Guid.NewGuid().ToString(),
                ServerTimestamp = DateTime.UtcNow.Ticks,
                MessageTimestamp = DateTime.UtcNow
            },
            Body = responseObject
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }
}
