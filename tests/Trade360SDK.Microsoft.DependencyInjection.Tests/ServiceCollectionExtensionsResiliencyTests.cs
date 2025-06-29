using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using FluentAssertions;
using Xunit;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.Common.Configuration;
using Microsoft.Extensions.Logging;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsResiliencyTests
    {
        [Fact]
        public void ServiceRegistration_ShouldSupportLogging()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            
            // Act
            services.AddLogging();
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.Should().NotBeNull();
        }

        [Fact]
        public void MultipleServiceRegistrations_ShouldHandleMemoryPressure()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act - Register services multiple times
            for (int i = 0; i < 10; i++)
            {
                services.AddTrade360CustomerApiClient(configuration);
                services.AddTrade360PrematchSnapshotClient();
                services.AddTrade360InplaySnapshotClient();
            }

            // Assert - Should not throw due to memory pressure
            Action buildProvider = () =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var factory = serviceProvider.GetService<ICustomersApiFactory>();
                factory.Should().NotBeNull();
            };
            buildProvider.Should().NotThrow();
        }

        [Fact]
        public void ServiceRegistration_ShouldHandleUrlVariations()
        {
            // Arrange
            var testUrls = new[]
            {
                "https://api.example.com",
                "https://api.example.com/",
                "http://localhost:8080",
                "https://api-v2.example.com/v1/"
            };

            foreach (var baseUrl in testUrls)
            {
                // Act
                var services = new ServiceCollection();
                services.Configure<Trade360Settings>(options =>
                {
                    options.SnapshotApiBaseUrl = baseUrl;
                });
                services.AddTrade360PrematchSnapshotClient();

                // Assert
                Action buildProvider = () =>
                {
                    using var serviceProvider = services.BuildServiceProvider();
                    var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                    client.Should().NotBeNull();
                };
                buildProvider.Should().NotThrow($"URL {baseUrl} should be handled correctly");
            }
        }

        [Fact]
        public void ServiceProvider_ShouldHandleDisposalProperly()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Act
            var serviceProvider = services.BuildServiceProvider();
            
            // Get services to ensure they're created
            var customerFactory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

            customerFactory.Should().NotBeNull();
            prematchClient.Should().NotBeNull();
            inplayClient.Should().NotBeNull();

            // Assert - Disposal should not throw
            Action dispose = () => serviceProvider.Dispose();
            dispose.Should().NotThrow();
        }

        [Fact]
        public void HttpClientConfiguration_ShouldPreserveCustomSettings()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            services.ConfigureHttpClientDefaults(builder =>
            {
                builder.ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            // Assert
            httpClientFactory.Should().NotBeNull();
            var client = httpClientFactory.CreateClient();
            client.Timeout.Should().Be(TimeSpan.FromSeconds(30));
        }

        [Fact]
        public void PackageCredentials_ShouldHandleVariousScenarios()
        {
            // Arrange
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = "inplay-package",
                    UserName = "user",
                    Password = "pass"
                };
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = "prematch-package",
                    UserName = "user2",
                    Password = "pass2"
                };
            });

            // Act
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Assert
            Action buildProvider = () =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();
                
                prematchClient.Should().NotBeNull();
                inplayClient.Should().NotBeNull();
            };
            buildProvider.Should().NotThrow();
        }

        [Fact]
        public void AutoMapper_ShouldHandleConflictResolution()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act - Register multiple services that all add AutoMapper
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Assert - Should handle multiple AutoMapper registrations gracefully
            Action buildProvider = () =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                // AutoMapper should be registered and functional
                var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
                mapper.Should().NotBeNull();
            };
            buildProvider.Should().NotThrow();
        }

        [Fact]
        public async Task ConcurrentServiceCreation_ShouldBeThreadSafe()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Act - Create services concurrently
            var tasks = new Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var factory = serviceProvider.GetService<ICustomersApiFactory>();
                    var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                    
                    factory.Should().NotBeNull();
                    client.Should().NotBeNull();
                });
            }

            // Assert - All concurrent operations should complete successfully
            await Task.WhenAll(tasks);
        }

        [Fact]
        public void ServiceScope_ShouldValidateLifecycles()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            using (var scope1 = serviceProvider.CreateScope())
            using (var scope2 = serviceProvider.CreateScope())
            {
                var factory1 = scope1.ServiceProvider.GetService<ICustomersApiFactory>();
                var factory2 = scope2.ServiceProvider.GetService<ICustomersApiFactory>();
                var client1 = scope1.ServiceProvider.GetService<ISnapshotPrematchApiClient>();
                var client2 = scope2.ServiceProvider.GetService<ISnapshotPrematchApiClient>();

                factory1.Should().NotBeNull();
                factory2.Should().NotBeNull();
                client1.Should().NotBeNull();
                client2.Should().NotBeNull();

                // Transient services should be different instances
                factory1.Should().NotBeSameAs(factory2);
                client1.Should().NotBeSameAs(client2);
            }
        }

        [Fact]
        public void ComplexConfiguration_ShouldBeHandled()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationData = new Dictionary<string, string>
            {
                {"Trade360:SnapshotApiBaseUrl", "https://complex.api.example.com/v2/"},
                {"Trade360:InplayPackageCredentials:PackageId", "complex-inplay"},
                {"Trade360:InplayPackageCredentials:UserName", "complex-user"},
                {"Trade360:InplayPackageCredentials:Password", "complex-pass"},
                {"Trade360:PrematchPackageCredentials:PackageId", "complex-prematch"},
                {"Trade360:PrematchPackageCredentials:UserName", "complex-user2"},
                {"Trade360:PrematchPackageCredentials:Password", "complex-pass2"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            // Act
            services.Configure<Trade360Settings>(configuration.GetSection("Trade360"));
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Assert
            Action buildProvider = () =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var customerFactory = serviceProvider.GetService<ICustomersApiFactory>();
                var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

                customerFactory.Should().NotBeNull();
                prematchClient.Should().NotBeNull();
                inplayClient.Should().NotBeNull();
            };
            buildProvider.Should().NotThrow();
        }

        [Fact]
        public void ServiceRegistrationOrder_ShouldNotMatter()
        {
            // Arrange & Act - Register services in different orders
            var services1 = new ServiceCollection();
            var services2 = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            
            services1.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://api.example.com");
            services2.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://api.example.com");

            // Order 1: Customer -> Prematch -> Inplay
            services1.AddTrade360CustomerApiClient(configuration);
            services1.AddTrade360PrematchSnapshotClient();
            services1.AddTrade360InplaySnapshotClient();

            // Order 2: Inplay -> Customer -> Prematch
            services2.AddTrade360InplaySnapshotClient();
            services2.AddTrade360CustomerApiClient(configuration);
            services2.AddTrade360PrematchSnapshotClient();

            // Assert - Both orders should work
            using var provider1 = services1.BuildServiceProvider();
            using var provider2 = services2.BuildServiceProvider();

            var factory1 = provider1.GetService<ICustomersApiFactory>();
            var factory2 = provider2.GetService<ICustomersApiFactory>();

            factory1.Should().NotBeNull();
            factory2.Should().NotBeNull();
        }
    }
} 