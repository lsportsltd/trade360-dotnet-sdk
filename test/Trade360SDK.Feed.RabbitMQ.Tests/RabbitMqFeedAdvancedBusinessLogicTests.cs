using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ;
using Trade360SDK.Feed.RabbitMQ.Exceptions;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class RabbitMqFeedAdvancedBusinessLogicTests
{
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IMessageProcessorContainer> _mockProcessorContainer;
    private readonly Mock<ICustomersApiFactory> _mockCustomersApiFactory;
    private readonly Mock<IPackageDistributionHttpClient> _mockDistributionClient;
    private readonly RmqConnectionSettings _settings;
    private readonly Trade360Settings _trade360Settings;

    public RabbitMqFeedAdvancedBusinessLogicTests()
    {
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger>();
        _mockProcessorContainer = new Mock<IMessageProcessorContainer>();
        _mockCustomersApiFactory = new Mock<ICustomersApiFactory>();
        _mockDistributionClient = new Mock<IPackageDistributionHttpClient>();

        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
        _mockCustomersApiFactory.Setup(x => x.CreatePackageDistributionHttpClient(It.IsAny<string>(), It.IsAny<PackageCredentials>()))
                                .Returns(_mockDistributionClient.Object);

        _settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20,
            AutoAck = false,
            DispatchConsumersAsync = true,
            PrefetchCount = 100
        };

        _trade360Settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplay_user",
                Password = "inplay_pass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematch_user",
                Password = "prematch_pass"
            }
        };
    }

    [Fact]
    public void Constructor_WithInvalidSettings_ShouldThrowArgumentException()
    {
        var invalidSettings = new RmqConnectionSettings
        {
            Host = "",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            RequestedHeartbeatSeconds = 5,
            NetworkRecoveryInterval = 10
        };

        Action act = () => new RabbitMqFeed(invalidSettings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithInPlayFlow_ShouldConfigureInPlayCredentials()
    {
        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        _mockCustomersApiFactory.Verify(x => x.CreatePackageDistributionHttpClient(
            "https://api.example.com",
            It.Is<PackageCredentials>(c => 
                c.PackageId == 123 && 
                c.Username == "inplay_user" && 
                c.Password == "inplay_pass")), Times.Once);
    }

    [Fact]
    public void Constructor_WithPreMatchFlow_ShouldConfigurePreMatchCredentials()
    {
        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.PreMatch, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        _mockCustomersApiFactory.Verify(x => x.CreatePackageDistributionHttpClient(
            "https://api.example.com",
            It.Is<PackageCredentials>(c => 
                c.PackageId == 456 && 
                c.Username == "prematch_user" && 
                c.Password == "prematch_pass")), Times.Once);
    }

    [Fact]
    public void Constructor_WithInvalidFlowType_ShouldThrowArgumentException()
    {
        Action act = () => new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            (FlowType)999, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        act.Should().Throw<ArgumentException>().WithMessage("*Not recognized flow type*");
    }

    [Fact]
    public async Task StartAsync_WithConnectAtStartFalse_ShouldSkipDistributionCheck()
    {
        var feed = new RabbitMqFeed(_settings, null, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(false, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();

        _mockDistributionClient.Verify(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task StartAsync_WithNoCustomersApiConfiguration_ShouldThrowArgumentException()
    {
        var feed = new RabbitMqFeed(_settings, null, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, CancellationToken.None);

        await act.Should().ThrowAsync<RabbitMqFeedException>();
    }

    [Fact]
    public async Task EnsureDistributionStartedAsync_WithDistributionAlreadyOn_ShouldReturnImmediately()
    {
        _mockDistributionClient.Setup(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = true });

        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();

        _mockDistributionClient.Verify(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockDistributionClient.Verify(x => x.StartDistributionAsync(It.IsAny<CancellationToken>()), Times.Never);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Distribution is already on")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task EnsureDistributionStartedAsync_WithDistributionOff_ShouldAttemptToStart()
    {
        _mockDistributionClient.SetupSequence(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = false })
                              .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = true });

        _mockDistributionClient.Setup(x => x.StartDistributionAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new StartDistributionResponse());

        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();

        _mockDistributionClient.Verify(x => x.StartDistributionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockDistributionClient.Verify(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()), Times.AtLeast(2));

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Distribution is off. Attempting to start")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task EnsureDistributionStartedAsync_WithMaxRetriesExceeded_ShouldThrowInvalidOperationException()
    {
        _mockDistributionClient.Setup(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = false });

        _mockDistributionClient.Setup(x => x.StartDistributionAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new StartDistributionResponse());

        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, CancellationToken.None);

        await act.Should().ThrowAsync<RabbitMqFeedException>();

        _mockDistributionClient.Verify(x => x.StartDistributionAsync(It.IsAny<CancellationToken>()), Times.Exactly(5));
    }

    [Fact]
    public async Task EnsureDistributionStartedAsync_WithCancellationRequested_ShouldThrowOperationCanceledException()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();

        _mockDistributionClient.Setup(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = false });

        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Distribution start operation was canceled")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task StartDistribution_WithException_ShouldLogError()
    {
        _mockDistributionClient.Setup(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = false });

        _mockDistributionClient.Setup(x => x.StartDistributionAsync(It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new HttpRequestException("Network error"));

        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, CancellationToken.None);

        await act.Should().ThrowAsync<RabbitMqFeedException>();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed StartDistribution")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeast(1));
    }

    [Fact]
    public async Task GetDistributionEnabled_WithException_ShouldLogErrorAndReturnFalse()
    {
        _mockDistributionClient.Setup(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new HttpRequestException("API error"));

        var feed = new RabbitMqFeed(_settings, _trade360Settings, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var act = async () => await feed.StartAsync(true, CancellationToken.None);

        await act.Should().ThrowAsync<RabbitMqFeedException>();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Got inappropriate GetDistributionEnabled response")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeast(1));
    }

    [Fact]
    public async Task StopAsync_WithActiveConnection_ShouldCloseGracefully()
    {
        var feed = new RabbitMqFeed(_settings, null, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        await feed.StopAsync(CancellationToken.None);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("RabbitMQ connection closed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task StopAsync_WithException_ShouldThrowRabbitMqFeedException()
    {
        var feed = new RabbitMqFeed(_settings, null, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        await feed.StopAsync(CancellationToken.None);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("RabbitMQ connection closed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void Dispose_ShouldLogDisposalAndCleanupResources()
    {
        var feed = new RabbitMqFeed(_settings, null, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        feed.Dispose();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Disposing RabbitMQ resources")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("RabbitMQFeed disposed successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void Dispose_WithException_ShouldThrowRabbitMqFeedException()
    {
        var feed = new RabbitMqFeed(_settings, null, _mockProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        feed.Dispose();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeast(1));
    }
}
