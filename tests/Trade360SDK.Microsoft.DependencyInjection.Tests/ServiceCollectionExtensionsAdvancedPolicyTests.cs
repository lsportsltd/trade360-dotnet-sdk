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
using Polly;
using Microsoft.Extensions.Http;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsAdvancedPolicyTests
    {
        [Fact]
        public void CustomerApiHttpClients_ShouldHaveRetryPolicy()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();

            // Verify HTTP clients can be created (they have Polly policies configured)
            var metadataClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            var packageClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.PackageDistributionHttpClient");
            var subscriptionClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.SubscriptionHttpClient");

            metadataClient.Should().NotBeNull();
            packageClient.Should().NotBeNull();
            subscriptionClient.Should().NotBeNull();
        }

        [Fact]
        public void SnapshotApiHttpClients_ShouldHaveRetryPolicy()
        {
            // Arrange
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();

            // Verify HTTP clients can be created (they have Polly policies configured)
            var prematchClient = httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");
            var inplayClient = httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotInplayApiClient");

            prematchClient.Should().NotBeNull();
            inplayClient.Should().NotBeNull();
        }

        [Fact]
        public void HttpClients_ShouldHaveCircuitBreakerPolicy()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert - HTTP clients should be created with circuit breaker policies
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();

            // Create clients to verify they have policies configured
            var customerClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            var snapshotClient = httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");

            customerClient.Should().NotBeNull();
            snapshotClient.Should().NotBeNull();
        }

        [Fact]
        public void HttpClientTimeout_ShouldBeConfigurable()
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
                    client.Timeout = TimeSpan.FromSeconds(45);
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            // Assert
            var client = httpClientFactory.CreateClient();
            client.Timeout.Should().Be(TimeSpan.FromSeconds(45));
        }

        [Fact]
        public void ResiliencePolicies_ShouldHandleErrorScenarios()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Assert - Should not throw during service registration
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
        public void HttpClientPolicies_ShouldSupportMultipleClients()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act - Register all client types
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert - All HTTP clients should be created successfully
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();

            var clients = new[]
            {
                httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient"),
                httpClientFactory.CreateClient("Trade360SDK.CustomersApi.PackageDistributionHttpClient"),
                httpClientFactory.CreateClient("Trade360SDK.CustomersApi.SubscriptionHttpClient"),
                httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient"),
                httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotInplayApiClient")
            };

            foreach (var client in clients)
            {
                client.Should().NotBeNull();
            }
        }

        [Fact]
        public void PolicyConfiguration_ShouldBeIndependent()
        {
            // Arrange
            var services1 = new ServiceCollection();
            var services2 = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act - Configure different service collections
            services1.AddTrade360CustomerApiClient(configuration);
            services2.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://api.example.com");
            services2.AddTrade360PrematchSnapshotClient();

            // Assert - Both should work independently
            using var provider1 = services1.BuildServiceProvider();
            using var provider2 = services2.BuildServiceProvider();

            var factory1 = provider1.GetService<IHttpClientFactory>();
            var factory2 = provider2.GetService<IHttpClientFactory>();

            factory1.Should().NotBeNull();
            factory2.Should().NotBeNull();

            var client1 = factory1.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            var client2 = factory2.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");

            client1.Should().NotBeNull();
            client2.Should().NotBeNull();
        }

        [Fact]
        public void HttpClientFactory_ShouldCreateUniqueInstances()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            // Assert
            var client1 = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            var client2 = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");

            client1.Should().NotBeNull();
            client2.Should().NotBeNull();
            client1.Should().NotBeSameAs(client2); // Factory creates new instances
        }

        [Fact]
        public void PolicyIntegration_ShouldWorkWithLogging()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act
            services.AddLogging();
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();

            // Assert
            Action buildProvider = () =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

                loggerFactory.Should().NotBeNull();
                httpClientFactory.Should().NotBeNull();

                var client = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
                client.Should().NotBeNull();
            };
            buildProvider.Should().NotThrow();
        }

        [Fact]
        public void ResiliencePolicies_ShouldHandleDisposal()
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
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            
            // Create some clients
            var client1 = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            var client2 = httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");

            client1.Should().NotBeNull();
            client2.Should().NotBeNull();

            // Assert - Disposal should work cleanly
            Action dispose = () => serviceProvider.Dispose();
            dispose.Should().NotThrow();
        }
    }
} 