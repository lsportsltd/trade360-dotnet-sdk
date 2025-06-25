using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Feed;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests
{
        public class RabbitMqFeedFactoryComprehensiveTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly Mock<ICustomersApiFactory> _mockCustomersApiFactory;
        private readonly MessageProcessorContainer<InPlay> _inPlayContainer;
        private readonly MessageProcessorContainer<PreMatch> _preMatchContainer;
        private readonly RabbitMqFeedFactory _factory;

        /// <summary>
        /// Creates a complete valid RmqConnectionSettings for testing
        /// </summary>
        private static RmqConnectionSettings CreateValidRmqSettings()
        {
            return new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "test",
                Password = "test",
                RequestedHeartbeatSeconds = 60,
                NetworkRecoveryInterval = 30
            };
        }

        /// <summary>
        /// Creates a complete valid Trade360Settings with package credentials for testing
        /// </summary>
        private static Trade360Settings CreateValidTrade360Settings()
        {
            return new Trade360Settings
            {
                CustomersApiBaseUrl = "https://api.example.com/",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "testuser",
                    Password = "testpass",
                    MessageFormat = "json"
                },
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "testuser",
                    Password = "testpass",
                    MessageFormat = "json"
                }
            };
        }

    public RabbitMqFeedFactoryComprehensiveTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockCustomersApiFactory = new Mock<ICustomersApiFactory>();
        
        // Create real instances with empty processor collections for testing
        _inPlayContainer = new MessageProcessorContainer<InPlay>(new List<IMessageProcessor>());
        _preMatchContainer = new MessageProcessorContainer<PreMatch>(new List<IMessageProcessor>());

        // Setup service provider to return required services
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILoggerFactory)))
            .Returns(_mockLoggerFactory.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ICustomersApiFactory)))
            .Returns(_mockCustomersApiFactory.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)))
            .Returns(_inPlayContainer);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)))
            .Returns(_preMatchContainer);

        _factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);
    }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidServiceProvider_ShouldInitializeCorrectly()
        {
            // Act
            var factory = new RabbitMqFeedFactory(_mockServiceProvider.Object);

            // Assert
            factory.Should().NotBeNull();
            factory.Should().BeAssignableTo<IFeedFactory>();
        }

        [Fact]
        public void Constructor_WithNullServiceProvider_ShouldAcceptNullButFailOnCreateFeed()
        {
            // Act - Constructor should accept null without throwing
            var factory = new RabbitMqFeedFactory(null!);
            
            // Assert - Constructor succeeds but CreateFeed should fail
            factory.Should().NotBeNull();
            
            // Verify that using the factory with null service provider fails
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();
            
            var act = () => factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("provider");
        }

        #endregion

        #region CreateFeed Tests - InPlay Flow

        [Fact]
        public void CreateFeed_WithInPlayFlowType_ShouldReturnRabbitMqFeed()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            settings.AutoAck = true;
            
            var trade360Settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://api.example.com/",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "testuser",
                    Password = "testpass"
                }
            };

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RabbitMqFeed>();
            
            // Verify the service provider was called correctly
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILoggerFactory)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ICustomersApiFactory)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Never);
        }

        [Fact]
        public void CreateFeed_WithInPlayFlowType_ShouldUseInPlayMessageProcessorContainer()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Never);
        }

        #endregion

        #region CreateFeed Tests - PreMatch Flow

        [Fact]
        public void CreateFeed_WithPreMatchFlowType_ShouldReturnRabbitMqFeed()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            settings.AutoAck = false;
            
            var trade360Settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://api.example.com/",
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "prematchuser",
                    Password = "prematchpass"
                }
            };

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.PreMatch);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RabbitMqFeed>();
            
            // Verify the service provider was called correctly
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILoggerFactory)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ICustomersApiFactory)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Never);
        }

        [Fact]
        public void CreateFeed_WithPreMatchFlowType_ShouldUsePreMatchMessageProcessorContainer()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.PreMatch);

            // Assert
            result.Should().NotBeNull();
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Never);
        }

        #endregion

        #region Parameter Validation Tests

        [Fact]
        public void CreateFeed_WithNullSettings_ShouldThrowArgumentNullException()
        {
            // Arrange
            var trade360Settings = new Trade360Settings();

            // Act & Assert
            var act = () => _factory.CreateFeed(null!, trade360Settings, FlowType.InPlay);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateFeed_WithNullTrade360Settings_ShouldCreateFeedSuccessfully()
        {
            // Arrange - Provide complete valid RmqConnectionSettings to pass validation
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "test",
                Password = "test",
                RequestedHeartbeatSeconds = 60,
                NetworkRecoveryInterval = 30
            };

            // Act
            var result = _factory.CreateFeed(settings, null!, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RabbitMqFeed>();
        }

        [Theory]
        [InlineData(FlowType.InPlay)]
        [InlineData(FlowType.PreMatch)]
        public void CreateFeed_WithValidParameters_ShouldNotThrow(FlowType flowType)
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act & Assert
            var act = () => _factory.CreateFeed(settings, trade360Settings, flowType);
            act.Should().NotThrow();
        }

        #endregion

        #region Settings Configuration Tests

        [Fact]
        public void CreateFeed_WithCompleteRmqSettings_ShouldPassAllSettingsToFeed()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "rabbitmq.example.com",
                Port = 5673,
                VirtualHost = "/test",
                PackageId = 456,
                UserName = "testuser",
                Password = "testpassword",
                AutoAck = true,
                PrefetchCount = 50,
                RequestedHeartbeatSeconds = 60,
                NetworkRecoveryInterval = 30
            };
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RabbitMqFeed>();
        }

        [Fact]
        public void CreateFeed_WithCompleteTradeSettings_ShouldPassAllSettingsToFeed()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://api.example.com/",
                SnapshotApiBaseUrl = "https://snapshot.example.com/",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "inplayuser",
                    Password = "inplaypass",
                    MessageFormat = "json"
                },
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "prematchuser",
                    Password = "prematchpass",
                    MessageFormat = "xml"
                }
            };

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RabbitMqFeed>();
        }

        #endregion

        #region Service Provider Integration Tests

        [Fact]
        public void CreateFeed_ShouldRetrieveAllRequiredServicesFromProvider()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILoggerFactory)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ICustomersApiFactory)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Once);
        }

        [Fact]
        public void CreateFeed_WithMissingLoggerFactory_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILoggerFactory)))
                .Returns((ILoggerFactory?)null);
            
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act & Assert
            var act = () => _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("No service for type 'Microsoft.Extensions.Logging.ILoggerFactory' has been registered.");
        }

        [Fact]
        public void CreateFeed_WithMissingCustomersApiFactory_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockServiceProvider.Setup(sp => sp.GetService(typeof(ICustomersApiFactory)))
                .Returns((ICustomersApiFactory?)null);
            
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act & Assert
            var act = () => _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("No service for type 'Trade360SDK.CustomersApi.Interfaces.ICustomersApiFactory' has been registered.");
        }

        [Fact]
        public void CreateFeed_WithMissingMessageProcessorContainer_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockServiceProvider.Setup(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)))
                .Returns((MessageProcessorContainer<InPlay>?)null);
            
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act & Assert
            var act = () => _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("No service for type 'Trade360SDK.Feed.RabbitMQ.Resolvers.MessageProcessorContainer`1[Trade360SDK.Feed.FeedType.InPlay]' has been registered.");
        }

        #endregion

        #region Multiple Feed Creation Tests

        [Fact]
        public void CreateFeed_CalledMultipleTimes_ShouldCreateSeparateInstances()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var feed1 = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
            var feed2 = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            feed1.Should().NotBeNull();
            feed2.Should().NotBeNull();
            feed1.Should().NotBeSameAs(feed2, "Each call should create a new instance");
        }

        [Fact]
        public void CreateFeed_WithDifferentFlowTypes_ShouldCreateDifferentFeeds()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var inPlayFeed = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
            var preMatchFeed = _factory.CreateFeed(settings, trade360Settings, FlowType.PreMatch);

            // Assert
            inPlayFeed.Should().NotBeNull();
            preMatchFeed.Should().NotBeNull();
            inPlayFeed.Should().NotBeSameAs(preMatchFeed);
            
            // Verify different message processor containers were used
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Once);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Once);
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void CreateFeed_WithMinimalSettings_ShouldStillCreateFeed()
        {
            // Arrange
            var settings = CreateValidRmqSettings(); // Complete valid settings
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var result = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RabbitMqFeed>();
        }

        [Fact]
        public void CreateFeed_WithDefaultFlowType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();
            var defaultFlowType = default(FlowType);

            // Act & Assert
            var act = () => _factory.CreateFeed(settings, trade360Settings, defaultFlowType);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("messageProcessorContainer");
            
            // Default FlowType is 0, which should not match InPlay or PreMatch
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Never);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Never);
        }

        [Fact]
        public void CreateFeed_WithInvalidFlowType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();
            var invalidFlowType = (FlowType)999; // Invalid enum value

            // Act & Assert
            var act = () => _factory.CreateFeed(settings, trade360Settings, invalidFlowType);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("messageProcessorContainer");
            
            // Should not try to get any message processor container
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Never);
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<PreMatch>)), Times.Never);
        }

        #endregion

        #region Performance and Resource Tests

        [Fact]
        public void CreateFeed_CalledManyTimes_ShouldPerformWell()
        {
            // Arrange
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();
            var startTime = DateTime.UtcNow;

            // Act
            for (int i = 0; i < 100; i++)
            {
                var feed = _factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);
                feed.Should().NotBeNull();
            }

            var duration = DateTime.UtcNow - startTime;

            // Assert
            duration.TotalSeconds.Should().BeLessThan(5, "Creating 100 feeds should complete quickly");
            
            // Verify service provider was called the expected number of times
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILoggerFactory)), Times.Exactly(100));
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(ICustomersApiFactory)), Times.Exactly(100));
            _mockServiceProvider.Verify(sp => sp.GetService(typeof(MessageProcessorContainer<InPlay>)), Times.Exactly(100));
        }

        #endregion

        #region Interface Implementation Tests

        [Fact]
        public void RabbitMqFeedFactory_ShouldImplementIFeedFactory()
        {
            // Assert
            _factory.Should().BeAssignableTo<IFeedFactory>();
        }

        [Fact]
        public void IFeedFactory_CreateFeed_ShouldReturnIFeed()
        {
            // Arrange
            IFeedFactory factory = _factory;
            var settings = CreateValidRmqSettings();
            var trade360Settings = CreateValidTrade360Settings();

            // Act
            var result = factory.CreateFeed(settings, trade360Settings, FlowType.InPlay);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IFeed>();
        }

        #endregion
    }
} 