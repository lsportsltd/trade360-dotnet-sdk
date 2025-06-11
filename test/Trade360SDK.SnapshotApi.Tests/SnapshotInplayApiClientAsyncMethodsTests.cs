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
using AutoMapper;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotInplayApiClientAsyncMethodsTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SnapshotInplayApiClient _client;

    public SnapshotInplayApiClientAsyncMethodsTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockOptions = new Mock<IOptions<Trade360Settings>>();
        _mockMapper = new Mock<IMapper>();
        
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://snapshot.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            }
        };

        mockOptions.Setup(x => x.Value).Returns(settings);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(settings.SnapshotApiBaseUrl)
        };

        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _client = new SnapshotInplayApiClient(mockHttpClientFactory.Object, mockOptions.Object, _mockMapper.Object);
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
    public async Task GetLivescore_WithValidRequest_ShouldReturnLivescores()
    {
        var expectedLivescores = new[]
        {
            new GetLiveScoreResponse { FixtureId = 1 },
            new GetLiveScoreResponse { FixtureId = 2 }
        };

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedLivescores);

        var result = await _client.GetLivescore(new GetLivescoreRequestDto(), CancellationToken.None);

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

        var act = async () => await _client.GetFixtures(new GetFixturesRequestDto(), cancellationTokenSource.Token);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetLivescore_WithLargeDataset_ShouldHandleEfficiently()
    {
        var largeLivescoreSet = Enumerable.Range(1, 1000)
            .Select(i => new GetLiveScoreResponse { FixtureId = i })
            .ToArray();

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(largeLivescoreSet);

        var result = await _client.GetLivescore(new GetLivescoreRequestDto(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1000);
        result.Last().FixtureId.Should().Be(1000);
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new Trade360SDK.SnapshotApi.Http.BaseResponse<T>
        {
            Header = new Trade360SDK.Common.Models.MessageHeader
            {
                CreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Type = 1,
                MsgSeq = 1,
                MsgGuid = Guid.NewGuid().ToString(),
                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                MessageBrokerTimestamp = DateTime.UtcNow,
                MessageTimestamp = DateTime.UtcNow
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
}
