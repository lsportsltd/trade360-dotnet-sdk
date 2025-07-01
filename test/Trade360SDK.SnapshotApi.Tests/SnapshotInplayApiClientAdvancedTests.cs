using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Models;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.SnapshotApi.Http;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotInplayApiClientAdvancedTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IOptions<Trade360Settings>> _mockOptions;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Trade360Settings _validSettings;
    private readonly SnapshotInplayApiClient _client;
    private bool _disposed;

    public SnapshotInplayApiClientAdvancedTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockOptions = new Mock<IOptions<Trade360Settings>>();
        _mockMapper = new Mock<IMapper>();

        _validSettings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://inplay.test.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "inplayuser",
                Password = "inplaypass"
            }
        };

        _mockOptions.Setup(x => x.Value).Returns(_validSettings);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_validSettings.SnapshotApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
    }

    [Fact]
    public void Constructor_WithValidCredentials_ShouldInitializeSuccessfully()
    {
        // Act & Assert
        _client.Should().NotBeNull();
        _mockHttpClientFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public void Constructor_WithNullInplayCredentials_ShouldStillInitialize()
    {
        // Arrange
        var settingsWithoutCredentials = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://inplay.test.com/",
            InplayPackageCredentials = null
        };

        var mockOptions = new Mock<IOptions<Trade360Settings>>();
        mockOptions.Setup(x => x.Value).Returns(settingsWithoutCredentials);

        // Act
        Action act = () => new SnapshotInplayApiClient(_mockHttpClientFactory.Object, mockOptions.Object, _mockMapper.Object);

        // Assert - Should not throw during construction
        act.Should().NotThrow();
    }

    [Fact]
    public async Task GetFixtures_WithValidResponse_ShouldDeserializeCorrectly()
    {
        // Arrange
        var expectedFixtures = new[]
        {
            new FixtureEvent 
            { 
                FixtureId = 1, 
                Fixture = new Fixture() 
            },
            new FixtureEvent 
            { 
                FixtureId = 2, 
                Fixture = new Fixture() 
            }
        };

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedFixtures);

        // Act
        var result = await _client.GetFixtures(new GetFixturesRequestDto(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().FixtureId.Should().Be(1);
        result.Last().FixtureId.Should().Be(2);
    }

    [Fact]
    public async Task GetLivescore_WithEmptyResponse_ShouldReturnEmptyCollection()
    {
        // Arrange
        var emptyLivescores = new GetLiveScoreResponse[0];
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(emptyLivescores);

        // Act
        var result = await _client.GetLivescore(new GetLivescoreRequestDto(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFixtureMarkets_WithComplexMarketData_ShouldHandleCorrectly()
    {
        // Arrange
        var expectedMarkets = new[]
        {
            new MarketEvent 
            { 
                FixtureId = 123, 
                Markets = new[] 
                { 
                    new Market { Id = 1, Name = "Match Winner" },
                    new Market { Id = 2, Name = "Total Goals" }
                }
            }
        };

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(expectedMarkets);

        // Act
        var result = await _client.GetFixtureMarkets(new GetMarketRequestDto(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().FixtureId.Should().Be(123);
        result.First().Markets.Should().HaveCount(2);
        result.First().Markets!.First().Name.Should().Be("Match Winner");
    }

    [Fact]
    public async Task GetEvents_WithNullBodyInResponse_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(baseRequest);

        SetupHttpResponseWithNullBody<IEnumerable<GetEventsResponse>>();

        // Act
        Func<Task> act = async () => await _client.GetEvents(new GetMarketRequestDto(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Body*property is missed*");
    }

    [Fact]
    public async Task GetFixtures_WithNullHeaderInResponse_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        SetupHttpResponseWithNullHeader<IEnumerable<FixtureEvent>>();

        // Act
        Func<Task> act = async () => await _client.GetFixtures(new GetFixturesRequestDto(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Header*property is missed*");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task GetLivescore_WithHttpErrorStatus_ShouldHandleGracefully(HttpStatusCode errorCode)
    {
        // Arrange
        var emptyLivescores = new GetLiveScoreResponse[0];
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(emptyLivescores, errorCode);

        // Act
        var result = await _client.GetLivescore(new GetLivescoreRequestDto(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFixtureMarkets_WithMalformedJsonResponse_ShouldThrowJsonException()
    {
        // Arrange
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(baseRequest);

        SetupHttpResponseWithMalformedJson();

        // Act
        Func<Task> act = async () => await _client.GetFixtureMarkets(new GetMarketRequestDto(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<JsonException>();
    }

    [Fact]
    public async Task GetEvents_WithVeryLargeResponse_ShouldHandleEfficiently()
    {
        // Arrange
        var largeEventSet = Enumerable.Range(1, 5000)
            .Select(i => new GetEventsResponse 
            { 
                FixtureId = i,
                Fixture = new Fixture(),
                Markets = new[] { new Market { Id = i * 10 } }
            })
            .ToArray();

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(baseRequest);

        SetupHttpResponse(largeEventSet);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await _client.GetEvents(new GetMarketRequestDto(), CancellationToken.None);
        stopwatch.Stop();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5000);
        result.Last().FixtureId.Should().Be(5000);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete within 5 seconds
    }

    [Fact]
    public async Task GetFixtures_WithCancellationRequested_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(baseRequest);

        // Setup mock to properly handle cancellation
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>((request, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        // Act
        Func<Task> act = async () => await _client.GetFixtures(new GetFixturesRequestDto(), cancellationTokenSource.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetLivescore_WithCustomTimeout_ShouldHandleTimeoutGracefully()
    {
        // Arrange
        var baseRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(baseRequest);

        // Setup delayed response
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns(async () =>
            {
                await Task.Delay(100); // Simulate slow response
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                };
            });

        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

        // Act
        Func<Task> act = async () => await _client.GetLivescore(new GetLivescoreRequestDto(), cancellationTokenSource.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void Dispose_ShouldDisposeResourcesProperly()
    {
        // Act
        _client.Dispose();

        // Assert
        // Verify disposal completed without exception
        Action act = () => _client.Dispose();
        act.Should().NotThrow();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new BaseResponse<T>
        {
            Header = new MessageHeader
            {
                CreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Type = 1,
                MsgSeq = 1,
                MsgGuid = Guid.NewGuid().ToString(),
                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                MessageBrokerTimestamp = DateTime.UtcNow
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
            .ReturnsAsync(httpResponse);
    }

    private void SetupHttpResponseWithNullBody<T>() where T : class
    {
        var baseResponse = new BaseResponse<T>
        {
            Header = new MessageHeader
            {
                CreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Type = 1,
                MsgSeq = 1,
                MsgGuid = Guid.NewGuid().ToString(),
                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                MessageBrokerTimestamp = DateTime.UtcNow
            },
            Body = null
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
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

    private void SetupHttpResponseWithNullHeader<T>() where T : class
    {
        var baseResponse = new BaseResponse<T>
        {
            Header = null,
            Body = default(T)
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
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

    private void SetupHttpResponseWithMalformedJson()
    {
        var malformedJson = "{ invalid json content }";
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(malformedJson, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _client?.Dispose();
            _disposed = true;
        }
    }
} 