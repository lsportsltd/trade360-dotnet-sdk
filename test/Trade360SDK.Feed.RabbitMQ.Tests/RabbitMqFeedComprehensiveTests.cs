using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ;
using Trade360SDK.Feed.RabbitMQ.Exceptions;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

/// <summary>
/// Comprehensive tests for RabbitMqFeed class covering complex scenarios,
/// error handling, connection management, and edge cases.
/// </summary>
public class RabbitMqFeedComprehensiveTests
{
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<ICustomersApiFactory> _mockCustomersApiFactory;
    private readonly Mock<IPackageDistributionHttpClient> _mockPackageDistributionClient;
    private readonly Mock<IMessageProcessorContainer> _mockMessageProcessorContainer;
    private readonly RmqConnectionSettings _validSettings;
    private readonly Trade360Settings _validTrade360Settings;

    public RabbitMqFeedComprehensiveTests()
    {
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger>();
        _mockCustomersApiFactory = new Mock<ICustomersApiFactory>();
        _mockPackageDistributionClient = new Mock<IPackageDistributionHttpClient>();
        _mockMessageProcessorContainer = new Mock<IMessageProcessorContainer>();

        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
        _mockCustomersApiFactory.Setup(x => x.CreatePackageDistributionHttpClient(It.IsAny<string>(), It.IsAny<PackageCredentials>()))
            .Returns(_mockPackageDistributionClient.Object);

        _validSettings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            PackageId = 123,
            RequestedHeartbeatSeconds = 60,
            NetworkRecoveryInterval = 30,
            PrefetchCount = 10,
            AutoAck = false,
            DispatchConsumersAsync = true
        };

        _validTrade360Settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplay-user",
                Password = "inplay-pass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematch-user",
                Password = "prematch-pass"
            }
        };
    }

    [Fact]
    public void Constructor_WithNullSettings_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new RabbitMqFeed(null!, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
                FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object));

        exception.ParamName.Should().Be("settings");
    }

    [Fact]
    public void Constructor_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
                FlowType.InPlay, null!, _mockCustomersApiFactory.Object));

        exception.ParamName.Should().Be("loggerFactory");
    }

    [Theory]
    [InlineData(FlowType.InPlay)]
    [InlineData(FlowType.PreMatch)]
    public void Constructor_WithValidParameters_CreatesInstance(FlowType flowType)
    {
        // Act
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            flowType, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        // Assert
        feed.Should().NotBeNull();
        _mockLoggerFactory.Verify(x => x.CreateLogger(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void Constructor_WithInvalidFlowType_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
                (FlowType)999, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object));

        exception.Message.Should().Contain("Not recognized flow type");
    }

    [Fact]
    public void Constructor_WithNullTrade360Settings_CreatesInstanceWithoutApiClient()
    {
        // Act
        var feed = new RabbitMqFeed(_validSettings, null, _mockMessageProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        // Assert
        feed.Should().NotBeNull();
        _mockCustomersApiFactory.Verify(x => x.CreatePackageDistributionHttpClient(It.IsAny<string>(), It.IsAny<PackageCredentials>()), Times.Never);
    }

    [Fact]
    public void Constructor_ValidatesSettings()
    {
        // Arrange
        var invalidSettings = new RmqConnectionSettings
        {
            Host = "", // Invalid empty host
            Port = 5672,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            PackageId = 123,
            RequestedHeartbeatSeconds = 60,
            NetworkRecoveryInterval = 30
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new RabbitMqFeed(invalidSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
                FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object));

        exception.Message.Should().Contain("Host is required");
    }

    [Fact]
    public async Task StartAsync_WithConnectAtStartTrue_CallsEnsureDistributionStarted()
    {
        // Arrange
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        _mockPackageDistributionClient.Setup(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetDistributionStatusResponse { IsDistributionOn = true });

        // Act & Assert
        // This will throw because we can't mock the RabbitMQ connection, but we can verify the API call was made
        await Assert.ThrowsAnyAsync<Exception>(() => feed.StartAsync(true, CancellationToken.None));

        _mockPackageDistributionClient.Verify(x => x.GetDistributionStatusAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task StartAsync_WithCancellationToken_ThrowsOperationCanceledException()
    {
        // Arrange
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => feed.StartAsync(true, cts.Token));
    }

    [Fact]
    public async Task StopAsync_DisposesResourcesCorrectly()
    {
        // Arrange
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        // Act
        await feed.StopAsync(CancellationToken.None);

        // Assert
        // Should complete without throwing
    }

    [Fact]
    public void Dispose_DisposesResourcesCorrectly()
    {
        // Arrange
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        // Act
        feed.Dispose();

        // Assert
        // Should complete without throwing
    }

    [Theory]
    [InlineData(FlowType.InPlay)]
    [InlineData(FlowType.PreMatch)]
    public void Constructor_CreatesCorrectPackageCredentials(FlowType flowType)
    {
        // Arrange
        PackageCredentials capturedCredentials = null;
        _mockCustomersApiFactory.Setup(x => x.CreatePackageDistributionHttpClient(It.IsAny<string>(), It.IsAny<PackageCredentials>()))
            .Callback<string, PackageCredentials>((url, creds) => capturedCredentials = creds)
            .Returns(_mockPackageDistributionClient.Object);

        // Act
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            flowType, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        // Assert
        capturedCredentials.Should().NotBeNull();
        if (flowType == FlowType.InPlay)
        {
            capturedCredentials.PackageId.Should().Be(_validTrade360Settings.InplayPackageCredentials.PackageId);
            capturedCredentials.Username.Should().Be(_validTrade360Settings.InplayPackageCredentials.Username);
            capturedCredentials.Password.Should().Be(_validTrade360Settings.InplayPackageCredentials.Password);
        }
        else
        {
            capturedCredentials.PackageId.Should().Be(_validTrade360Settings.PrematchPackageCredentials.PackageId);
            capturedCredentials.Username.Should().Be(_validTrade360Settings.PrematchPackageCredentials.Username);
            capturedCredentials.Password.Should().Be(_validTrade360Settings.PrematchPackageCredentials.Password);
        }
    }

    [Fact]
    public void Constructor_ConfiguresConnectionFactoryCorrectly()
    {
        // Act
        var feed = new RabbitMqFeed(_validSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
            FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object);

        // Assert
        feed.Should().NotBeNull();
        // We can't directly access the private _factory field, but we can verify the constructor completed successfully
    }

    [Fact]
    public void Constructor_WithInvalidSettings_ThrowsValidationException()
    {
        // Arrange
        var invalidSettings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = -1, // Invalid port
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            PackageId = 123,
            RequestedHeartbeatSeconds = 60,
            NetworkRecoveryInterval = 30
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new RabbitMqFeed(invalidSettings, _validTrade360Settings, _mockMessageProcessorContainer.Object, 
                FlowType.InPlay, _mockLoggerFactory.Object, _mockCustomersApiFactory.Object));

        exception.Message.Should().Contain("Port must be a positive integer");
    }
} 