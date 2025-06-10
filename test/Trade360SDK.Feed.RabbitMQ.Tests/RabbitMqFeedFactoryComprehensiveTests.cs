using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.RabbitMQ;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Feed;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class RabbitMqFeedFactoryComprehensiveTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IMessageProcessorContainer> _mockProcessorContainer;
    private readonly RabbitMqFeedFactory _factory;

    public RabbitMqFeedFactoryComprehensiveTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger>();
        _mockProcessorContainer = new Mock<IMessageProcessorContainer>();

        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
        var inplayContainer = new MessageProcessorContainer<Trade360SDK.Feed.FeedType.InPlay>(new List<IMessageProcessor>());
        var prematchContainer = new MessageProcessorContainer<Trade360SDK.Feed.FeedType.PreMatch>(new List<IMessageProcessor>());
        var mockCustomersApiFactory = new Mock<Trade360SDK.CustomersApi.Interfaces.ICustomersApiFactory>();
        var mockPackageDistributionClient = new Mock<Trade360SDK.CustomersApi.Interfaces.IPackageDistributionHttpClient>();
        
        mockCustomersApiFactory.Setup(x => x.CreatePackageDistributionHttpClient(It.IsAny<string>(), It.IsAny<Trade360SDK.Common.Configuration.PackageCredentials>()))
            .Returns(mockPackageDistributionClient.Object);

        _mockServiceProvider.Setup(x => x.GetService(typeof(ILoggerFactory))).Returns(_mockLoggerFactory.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(MessageProcessorContainer<Trade360SDK.Feed.FeedType.InPlay>))).Returns(inplayContainer);
        _mockServiceProvider.Setup(x => x.GetService(typeof(MessageProcessorContainer<Trade360SDK.Feed.FeedType.PreMatch>))).Returns(prematchContainer);
        _mockServiceProvider.Setup(x => x.GetService(typeof(Trade360SDK.CustomersApi.Interfaces.ICustomersApiFactory))).Returns(mockCustomersApiFactory.Object);

        _factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);
    }

    [Fact]
    public void Constructor_WithValidServiceProvider_ShouldInitializeCorrectly()
    {
        _factory.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ShouldNotThrowException()
    {
        var factory = new RabbitMqFeedFactory(null);
        factory.Should().NotBeNull();
    }

    [Fact]
    public void CreateFeed_WithInplayFlowType_ShouldReturnMessageConsumer()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IFeed>();
    }

    [Fact]
    public void CreateFeed_WithPrematchFlowType_ShouldReturnMessageConsumer()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result = _factory.CreateFeed(settings, trade360Settings, FlowType.PreMatch);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IFeed>();
    }

    [Fact]
    public void CreateFeed_WithNullSettings_ShouldThrowArgumentNullException()
    {
        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var act = () => _factory.CreateFeed(null, trade360Settings, FlowType.InPlay);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(FlowType.InPlay)]
    [InlineData(FlowType.PreMatch)]
    public void CreateFeed_WithValidFlowTypes_ShouldReturnMessageConsumer(FlowType flowType)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result = _factory.CreateFeed(settings, trade360Settings, flowType);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IFeed>();
    }

    [Fact]
    public void CreateFeed_WithInvalidConnectionSettings_ShouldThrowArgumentException()
    {
        var invalidSettings = new RmqConnectionSettings
        {
            Host = "",
            Port = 0,
            VirtualHost = "",
            UserName = "",
            Password = "",
            PackageId = 0,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var act = () => _factory.CreateFeed(invalidSettings, trade360Settings, FlowType.InPlay);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateFeed_WithAutoAckTrue_ShouldReturnMessageConsumer()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = true
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IFeed>();
    }

    [Fact]
    public void CreateFeed_WithAutoAckFalse_ShouldReturnMessageConsumer()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IFeed>();
    }

    [Fact]
    public void CreateFeed_WithDifferentPackageIds_ShouldReturnMessageConsumer()
    {
        var settings1 = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var settings2 = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 456,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result1 = _factory.CreateFeed(settings1, trade360Settings, FlowType.InPlay);
        var result2 = _factory.CreateFeed(settings2, trade360Settings, FlowType.PreMatch);

        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().BeAssignableTo<IFeed>();
        result2.Should().BeAssignableTo<IFeed>();
    }

    [Fact]
    public void CreateFeed_WithServiceProviderMissingLoggerFactory_ShouldThrowInvalidOperationException()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(ILoggerFactory)))).Returns(null);
        var factory = new RabbitMqFeedFactory(mockServiceProvider.Object);

        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var act = () => factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CreateFeed_WithServiceProviderMissingProcessorContainer_ShouldThrowInvalidOperationException()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(ILoggerFactory)))).Returns(_mockLoggerFactory.Object);
        mockServiceProvider.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(Trade360SDK.CustomersApi.Interfaces.ICustomersApiFactory)))).Returns(new Mock<Trade360SDK.CustomersApi.Interfaces.ICustomersApiFactory>().Object);
        mockServiceProvider.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(MessageProcessorContainer<Trade360SDK.Feed.FeedType.InPlay>)))).Returns(null);
        var factory = new RabbitMqFeedFactory(mockServiceProvider.Object);

        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var act = () => factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CreateFeed_MultipleCalls_ShouldReturnDifferentInstances()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false
        };

        var trade360Settings = new Trade360SDK.Common.Configuration.Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new Trade360SDK.Common.Configuration.PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser", 
                Password = "prematchpass"
            }
        };
        
        var result1 = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
        var result2 = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().NotBeSameAs(result2);
    }
}
