using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class RabbitMqFeedFactoryTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ICustomersApiFactory> _mockCustomersApiFactory;
    private readonly IMessageProcessorContainer _mockInPlayContainer;
    private readonly IMessageProcessorContainer _mockPreMatchContainer;

    public RabbitMqFeedFactoryTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockCustomersApiFactory = new Mock<ICustomersApiFactory>();
        var mockProcessors = new List<IMessageProcessor>();
        _mockInPlayContainer = new MessageProcessorContainer<InPlay>(mockProcessors);
        _mockPreMatchContainer = new MessageProcessorContainer<PreMatch>(mockProcessors);

        var mockLogger = new Mock<ILogger>();
        _mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILoggerFactory)))
                           .Returns(_mockLoggerFactory.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ICustomersApiFactory)))
                           .Returns(_mockCustomersApiFactory.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);

        factory.Should().NotBeNull();
    }

    [Fact]
    public void CreateFeed_WithNullServiceProvider_ShouldThrowWhenAccessingServices()
    {
        var factory = new RabbitMqFeedFactory(null!);
        var settings = new RmqConnectionSettings();
        var trade360Settings = new Trade360Settings
        {
            InplayPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            PrematchPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            CustomersApiBaseUrl = "http://test.com"
        };

        Action act = () => factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreateFeed_WithInPlayFlowType_ShouldReturnRabbitMqFeed()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)))
                           .Returns(_mockInPlayContainer);

        var factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "pass"
        };
        var trade360Settings = new Trade360Settings
        {
            InplayPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            PrematchPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            CustomersApiBaseUrl = "http://test.com"
        };

        var result = factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        result.Should().NotBeNull();
        result.Should().BeOfType<RabbitMqFeed>();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILoggerFactory)), Times.Once);
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(ICustomersApiFactory)), Times.Once);
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Once);
    }

    [Fact]
    public void CreateFeed_WithPreMatchFlowType_ShouldReturnRabbitMqFeed()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)))
                           .Returns(_mockPreMatchContainer);

        var factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "pass"
        };
        var trade360Settings = new Trade360Settings
        {
            InplayPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            PrematchPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            CustomersApiBaseUrl = "http://test.com"
        };

        var result = factory.CreateFeed(settings, trade360Settings, FlowType.PreMatch);

        result.Should().NotBeNull();
        result.Should().BeOfType<RabbitMqFeed>();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Once);
    }

    [Fact]
    public void CreateFeed_WithMissingLoggerFactory_ShouldThrowInvalidOperationException()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILoggerFactory)))
                           .Returns((ILoggerFactory?)null);

        var factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);
        var settings = new RmqConnectionSettings();
        var trade360Settings = new Trade360Settings
        {
            InplayPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            PrematchPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            CustomersApiBaseUrl = "http://test.com"
        };

        Action act = () => factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("No service for type*");
    }

    [Fact]
    public void CreateFeed_WithMissingCustomersApiFactory_ShouldThrowInvalidOperationException()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ICustomersApiFactory)))
                           .Returns((ICustomersApiFactory?)null);

        var factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);
        var settings = new RmqConnectionSettings();
        var trade360Settings = new Trade360Settings
        {
            InplayPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            PrematchPackageCredentials = new PackageCredentials { PackageId = 1, Username = "test", Password = "test" },
            CustomersApiBaseUrl = "http://test.com"
        };

        Action act = () => factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("No service for type*");
    }
}
