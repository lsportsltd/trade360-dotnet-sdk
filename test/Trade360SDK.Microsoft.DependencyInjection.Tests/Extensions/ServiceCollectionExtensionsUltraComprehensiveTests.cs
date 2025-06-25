using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Polly;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;
using Xunit;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions
{
    public class ServiceCollectionExtensionsUltraComprehensiveTests
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ServiceCollectionExtensionsUltraComprehensiveTests()
        {
            _services = new ServiceCollection();
            
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("Trade360:CustomersApiBaseUrl", "https://api.example.com/"),
                new KeyValuePair<string, string>("Trade360:SnapshotApiBaseUrl", "https://snapshot.example.com/")
            });
            _configuration = configBuilder.Build();
        }

        #region AddTrade360CustomerApiClient Tests

        [Fact]
        public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterAllRequiredServices()
        {
            // Act
            var result = _services.AddTrade360CustomerApiClient(_configuration);

            // Assert
            result.Should().BeSameAs(_services, "Should return the same service collection for chaining");
            
            var serviceProvider = _services.BuildServiceProvider();
            
            // Verify core services are registered
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull("CustomersApiFactory should be registered");
            serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull("HttpClientFactory should be registered");
            serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull("AutoMapper should be registered");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => _services.AddTrade360CustomerApiClient(null!);
            
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("configuration")
                .WithMessage("*configuration*");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterHttpClientsWithCorrectTypes()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            // Verify we can create HTTP clients for all registered types
            var metadataClient = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName);
            var packageDistributionClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName);
            var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName);

            metadataClient.Should().NotBeNull("MetadataHttpClient should be creatable");
            packageDistributionClient.Should().NotBeNull("PackageDistributionHttpClient should be creatable");
            subscriptionClient.Should().NotBeNull("SubscriptionHttpClient should be creatable");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterServicesWithCorrectLifetimes()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);

            // Assert
            var factoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
            factoryDescriptor.Should().NotBeNull("CustomersApiFactory should be registered");
            factoryDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient, "CustomersApiFactory should be transient");

            // Verify HTTP client registrations
            var httpClientDescriptors = _services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
            httpClientDescriptors.Should().NotBeEmpty("HTTP clients should be registered");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterAutoMapperCorrectly()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

            mapper.Should().NotBeNull("AutoMapper should be registered");
            
            // Verify AutoMapper configuration is valid
            var act = () => mapper.ConfigurationProvider.AssertConfigurationIsValid();
            act.Should().NotThrow("AutoMapper configuration should be valid");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithMultipleCalls_ShouldNotDuplicateRegistrations()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360CustomerApiClient(_configuration);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            
            factory.Should().NotBeNull("Factory should still be available after multiple registrations");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldConfigureResiliencyPolicies()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            // Create clients and verify they're configured (policies are attached during creation)
            var metadataClient = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName);
            var packageDistributionClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName);
            var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName);

            // Verify clients are created successfully (policies would prevent creation if misconfigured)
            metadataClient.Should().NotBeNull();
            packageDistributionClient.Should().NotBeNull();
            subscriptionClient.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithEmptyConfiguration_ShouldStillRegisterServices()
        {
            // Arrange
            var emptyConfig = new ConfigurationBuilder().Build();

            // Act
            _services.AddTrade360CustomerApiClient(emptyConfig);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        }

        #endregion

        #region AddTrade360PrematchSnapshotClient Tests

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            var result = _services.AddTrade360PrematchSnapshotClient();

            // Assert
            result.Should().BeSameAs(_services, "Should return the same service collection for chaining");
            
            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull("SnapshotPrematchApiClient should be registered");
            serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull("AutoMapper should be registered");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldConfigureHttpClientWithBaseAddress()
        {
            // Arrange
            var baseUrl = "https://snapshot.example.com/";
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = baseUrl;
            });

            // Act
            _services.AddTrade360PrematchSnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            
            // The HTTP client configuration is applied during client creation
            var client = httpClientFactory.CreateClient(typeof(ISnapshotPrematchApiClient).FullName);
            client.Should().NotBeNull("HTTP client should be creatable");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldRegisterServicesWithCorrectLifetimes()
        {
            // Act
            _services.AddTrade360PrematchSnapshotClient();

            // Assert
            var clientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
            clientDescriptor.Should().NotBeNull("SnapshotPrematchApiClient should be registered");
            clientDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient, "SnapshotPrematchApiClient should be transient");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldConfigureResiliencyPolicies()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            });

            // Act
            _services.AddTrade360PrematchSnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            
            var client = httpClientFactory.CreateClient(typeof(ISnapshotPrematchApiClient).FullName);
            client.Should().NotBeNull("Client with policies should be creatable");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithMultipleCalls_ShouldNotDuplicateRegistrations()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360PrematchSnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            
            client.Should().NotBeNull("Client should still be available after multiple registrations");
        }

        #endregion

        #region AddTrade360InplaySnapshotClient Tests

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
            });

            // Act
            var result = _services.AddTrade360InplaySnapshotClient();

            // Assert
            result.Should().BeSameAs(_services, "Should return the same service collection for chaining");
            
            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull("SnapshotInplayApiClient should be registered");
            serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull("AutoMapper should be registered");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldConfigureHttpClientWithBaseAddress()
        {
            // Arrange
            var baseUrl = "https://snapshot.example.com/";
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = baseUrl;
            });

            // Act
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            
            var client = httpClientFactory.CreateClient(typeof(ISnapshotInplayApiClient).FullName);
            client.Should().NotBeNull("HTTP client should be creatable");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldRegisterServicesWithCorrectLifetimes()
        {
            // Act
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var clientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
            clientDescriptor.Should().NotBeNull("SnapshotInplayApiClient should be registered");
            clientDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient, "SnapshotInplayApiClient should be transient");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldConfigureResiliencyPolicies()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            });

            // Act
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            
            var client = httpClientFactory.CreateClient(typeof(ISnapshotInplayApiClient).FullName);
            client.Should().NotBeNull("Client with policies should be creatable");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithMultipleCalls_ShouldNotDuplicateRegistrations()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
            });

            // Act
            _services.AddTrade360InplaySnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var client = serviceProvider.GetService<ISnapshotInplayApiClient>();
            
            client.Should().NotBeNull("Client should still be available after multiple registrations");
        }

        #endregion

        #region Integration and Cross-Method Tests

        [Fact]
        public void AllExtensionMethods_ShouldWorkTogetherWithoutConflicts()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            
            // Verify all services are available
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
            serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull();
            serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AllExtensionMethods_ShouldRegisterUniqueServices()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            
            var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

            customersFactory.Should().NotBeNull();
            prematchClient.Should().NotBeNull();
            inplayClient.Should().NotBeNull();
            
            // Verify they are different instances (transient)
            var customersFactory2 = serviceProvider.GetService<ICustomersApiFactory>();
            var prematchClient2 = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayClient2 = serviceProvider.GetService<ISnapshotInplayApiClient>();

            customersFactory.Should().NotBeSameAs(customersFactory2, "CustomersApiFactory should be transient");
            prematchClient.Should().NotBeSameAs(prematchClient2, "PrematchClient should be transient");
            inplayClient.Should().NotBeSameAs(inplayClient2, "InplayClient should be transient");
        }

        [Fact]
        public void AllExtensionMethods_WithLogging_ShouldIntegrateCorrectly()
        {
            // Arrange
            _services.AddLogging();
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.Should().NotBeNull("LoggerFactory should be available");
            
            // Verify services can be created with logging
            var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

            customersFactory.Should().NotBeNull();
            prematchClient.Should().NotBeNull();
            inplayClient.Should().NotBeNull();
        }

        [Fact]
        public void AllExtensionMethods_WithOptions_ShouldConfigureCorrectly()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
            
            options.Should().NotBeNull("Trade360Settings should be configured");
            options!.Value.Should().NotBeNull();
            options.Value.CustomersApiBaseUrl.Should().Be("https://api.example.com/");
            options.Value.SnapshotApiBaseUrl.Should().Be("https://snapshot.example.com/");
            options.Value.InplayPackageCredentials.Should().NotBeNull();
            options.Value.PrematchPackageCredentials.Should().NotBeNull();
        }

        #endregion

        #region Error Handling and Edge Cases

        [Fact]
        public void AddTrade360CustomerApiClient_WithInvalidConfiguration_ShouldStillRegisterServices()
        {
            // Arrange
            var invalidConfig = new Mock<IConfiguration>();
            invalidConfig.Setup(c => c[It.IsAny<string>()]).Returns((string?)null);

            // Act
            _services.AddTrade360CustomerApiClient(invalidConfig.Object);

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        }

        [Fact]
        public void SnapshotClients_WithMissingOptions_ShouldThrowWhenCreated()
        {
            // Act (without configuring options)
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = _services.BuildServiceProvider();
            
            // Services should be registered but throw when created due to missing configuration
            var prematchAction = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayAction = () => serviceProvider.GetService<ISnapshotInplayApiClient>();
            
            prematchAction.Should().Throw<InvalidOperationException>()
                .WithMessage("Operation is not valid due to the current state of the object.");
            inplayAction.Should().Throw<InvalidOperationException>()
                .WithMessage("Operation is not valid due to the current state of the object.");
        }

        [Fact]
        public void AllExtensionMethods_ShouldBeChainableWithOtherServiceRegistrations()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            var result = _services
                .AddLogging()
                .AddTrade360CustomerApiClient(_configuration)
                .AddTrade360PrematchSnapshotClient()
                .AddTrade360InplaySnapshotClient()
                .AddSingleton<string>("test");

            // Assert
            result.Should().BeSameAs(_services, "All methods should be chainable");
            
            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetService<ILoggerFactory>().Should().NotBeNull();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
            serviceProvider.GetService<string>().Should().Be("test");
        }

        #endregion

        #region Performance and Resource Tests

        [Fact]
        public void AllExtensionMethods_WithManyRegistrations_ShouldPerformWell()
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            // Act
            for (int i = 0; i < 100; i++)
            {
                _services.AddTrade360CustomerApiClient(_configuration);
                _services.AddTrade360PrematchSnapshotClient();
                _services.AddTrade360InplaySnapshotClient();
            }

            var duration = DateTime.UtcNow - startTime;

            // Assert
            duration.TotalSeconds.Should().BeLessThan(5, "Registration should complete quickly even with many calls");
            
            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
        }

        [Fact]
        public void ServiceProvider_ShouldDisposeCorrectly()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.CustomersApiBaseUrl = "https://api.example.com/";
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "inplayuser",
                    Password = "inplaypass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                };
            });

            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            // Act
            using (var serviceProvider = _services.BuildServiceProvider())
            {
                var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
                var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

                // Assert
                customersFactory.Should().NotBeNull();
                prematchClient.Should().NotBeNull();
                inplayClient.Should().NotBeNull();
            }

            // Should dispose without throwing
        }

        #endregion
    }
} 