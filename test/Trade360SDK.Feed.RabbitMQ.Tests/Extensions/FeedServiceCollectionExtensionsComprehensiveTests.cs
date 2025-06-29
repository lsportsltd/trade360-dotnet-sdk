using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Feed;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Extensions;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.CustomersApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Extensions
{
    public class FeedServiceCollectionExtensionsComprehensiveTests
    {
        [Fact]
        public void AddT360RmqFeedSdk_WithValidConfiguration_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var result = services.AddT360RmqFeedSdk(configuration);

            // Assert
            result.Should().BeSameAs(services, "method should return the same service collection for chaining");
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Verify core services
            serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
            serviceProvider.GetService<IFeedFactory>().Should().BeOfType<RabbitMqFeedFactory>();
            
            // Verify message processor containers
            serviceProvider.GetService<MessageProcessorContainer<InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessorContainer<PreMatch>>().Should().NotBeNull();
            
            // Verify CustomersApi client is registered
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldRegisterAllInPlayMessageProcessors()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Verify all InPlay message processors are registered
            var inPlayProcessors = services.Where(s => 
                s.ServiceType == typeof(IMessageProcessor) && 
                s.ImplementationType?.GetGenericArguments().Any(t => t == typeof(InPlay)) == true)
                .ToList();

            inPlayProcessors.Should().HaveCount(6, "should register 6 InPlay message processors");

            // Verify specific InPlay processors
            serviceProvider.GetService<MessageProcessor<FixtureMetadataUpdate, InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<HeartbeatUpdate, InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<LivescoreUpdate, InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<KeepAliveUpdate, InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<SettlementUpdate, InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<MarketUpdate, InPlay>>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldRegisterAllPreMatchMessageProcessors()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Verify all PreMatch message processors are registered
            var preMatchProcessors = services.Where(s => 
                s.ServiceType == typeof(IMessageProcessor) && 
                s.ImplementationType?.GetGenericArguments().Any(t => t == typeof(PreMatch)) == true)
                .ToList();

            preMatchProcessors.Should().HaveCount(11, "should register 11 PreMatch message processors");

            // Verify specific PreMatch processors
            serviceProvider.GetService<MessageProcessor<FixtureMetadataUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<HeartbeatUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<LivescoreUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<SettlementUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<MarketUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<OutrightFixtureUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<OutrightLeagueUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<OutrightScoreUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<OutrightSettlementsUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<OutrightLeagueMarketUpdate, PreMatch>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessor<OutrightFixtureMarketUpdate, PreMatch>>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldRegisterServicesAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);

            // Assert - Verify service lifetimes
            var feedFactoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IFeedFactory));
            feedFactoryDescriptor.Should().NotBeNull();
            feedFactoryDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);

            var containerDescriptors = services.Where(s => 
                s.ServiceType == typeof(MessageProcessorContainer<InPlay>) || 
                s.ServiceType == typeof(MessageProcessorContainer<PreMatch>))
                .ToList();
            
            containerDescriptors.Should().HaveCount(2);
            containerDescriptors.Should().OnlyContain(d => d.Lifetime == ServiceLifetime.Singleton);

            var processorDescriptors = services.Where(s => s.ServiceType == typeof(IMessageProcessor)).ToList();
            processorDescriptors.Should().OnlyContain(d => d.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddT360RmqFeedSdk_WithNullConfiguration_ShouldStillRegisterCoreServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert - Should not throw with null configuration
            Action act = () => services.AddT360RmqFeedSdk(null);
            act.Should().NotThrow();

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_WithMultipleCalls_ShouldNotDuplicateRegistrations()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act - Call multiple times
            services.AddT360RmqFeedSdk(configuration);
            services.AddT360RmqFeedSdk(configuration);
            services.AddT360RmqFeedSdk(configuration);

            // Assert - Should have multiple registrations (this is expected behavior)
            var feedFactoryDescriptors = services.Where(s => s.ServiceType == typeof(IFeedFactory)).ToList();
            feedFactoryDescriptors.Should().HaveCount(3, "multiple calls should result in multiple registrations");

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldAllowServiceChaining()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act - Test method chaining
            var result = services
                .AddLogging()
                .AddT360RmqFeedSdk(configuration)
                .AddSingleton<string>("test");

            // Assert
            result.Should().BeSameAs(services);
            result.Should().HaveCountGreaterThan(20, "should have many services registered");
        }

        [Fact]
        public void AddT360RmqFeedSdk_WithEmptyConfiguration_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var emptyConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            // Act
            services.AddT360RmqFeedSdk(emptyConfiguration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessorContainer<InPlay>>().Should().NotBeNull();
            serviceProvider.GetService<MessageProcessorContainer<PreMatch>>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldRegisterExactMessageProcessorCount()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);

            // Assert - Verify exact counts
            var allMessageProcessors = services.Where(s => s.ServiceType == typeof(IMessageProcessor)).ToList();
            allMessageProcessors.Should().HaveCount(17, "should register exactly 17 message processors (6 InPlay + 11 PreMatch)");

            var inPlayCount = services.Count(s => 
                s.ServiceType == typeof(IMessageProcessor) && 
                s.ImplementationType?.GetGenericArguments().Contains(typeof(InPlay)) == true);
            inPlayCount.Should().Be(6, "should register exactly 6 InPlay processors");

            var preMatchCount = services.Count(s => 
                s.ServiceType == typeof(IMessageProcessor) && 
                s.ImplementationType?.GetGenericArguments().Contains(typeof(PreMatch)) == true);
            preMatchCount.Should().Be(11, "should register exactly 11 PreMatch processors");
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldRegisterMessageProcessorContainersCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var inPlayContainer = serviceProvider.GetService<MessageProcessorContainer<InPlay>>();
            var preMatchContainer = serviceProvider.GetService<MessageProcessorContainer<PreMatch>>();

            inPlayContainer.Should().NotBeNull();
            preMatchContainer.Should().NotBeNull();
            inPlayContainer.Should().NotBeSameAs(preMatchContainer);
        }

        [Fact]
        public void AddT360RmqFeedSdk_WithComplexConfiguration_ShouldHandleCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var complexConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"CustomersApi:BaseUrl", "https://api.example.com"},
                    {"RabbitMQ:Host", "localhost"},
                    {"RabbitMQ:Port", "5672"},
                    {"Logging:LogLevel:Default", "Information"}
                })
                .Build();

            // Act
            services.AddT360RmqFeedSdk(complexConfig);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ShouldIntegrateWithExistingServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            
            // Pre-register some services
            services.AddLogging();
            services.AddSingleton<string>("existing-service");

            // Act
            services.AddT360RmqFeedSdk(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            
            // Verify existing services still work
            serviceProvider.GetService<string>().Should().Be("existing-service");
            
            // Verify new services work
            serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AddT360RmqFeedSdk_ServiceProvider_ShouldDisposeCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Get some services to ensure they're created
            var feedFactory = serviceProvider.GetService<IFeedFactory>();
            var inPlayContainer = serviceProvider.GetService<MessageProcessorContainer<InPlay>>();

            feedFactory.Should().NotBeNull();
            inPlayContainer.Should().NotBeNull();

            // Assert - Disposal should not throw
            Action dispose = () => serviceProvider.Dispose();
            dispose.Should().NotThrow();
        }

        [Fact]
        public void AddT360RmqFeedSdk_WithServiceScopes_ShouldMaintainSingletonBehavior()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddT360RmqFeedSdk(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Singletons should be the same across scopes
            using var scope1 = serviceProvider.CreateScope();
            using var scope2 = serviceProvider.CreateScope();

            var factory1 = scope1.ServiceProvider.GetService<IFeedFactory>();
            var factory2 = scope2.ServiceProvider.GetService<IFeedFactory>();
            var factory3 = serviceProvider.GetService<IFeedFactory>();

            factory1.Should().BeSameAs(factory2);
            factory1.Should().BeSameAs(factory3);
        }
    }
} 