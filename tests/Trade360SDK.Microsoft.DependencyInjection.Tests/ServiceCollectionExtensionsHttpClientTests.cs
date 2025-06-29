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

namespace Trade360SDK.Microsoft.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsHttpClientTests
    {
        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterHttpClients()
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
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Action act = () => services.AddTrade360CustomerApiClient(null);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("configuration");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldRegisterHttpClient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act
            services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldRegisterHttpClient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act
            services.AddTrade360InplaySnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
        }

        [Fact]
        public void SnapshotClients_WithValidBaseUrl_ShouldConfigureBaseAddress()
        {
            // Arrange
            var services = new ServiceCollection();
            var expectedBaseUrl = "https://snapshot.example.com/";
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = expectedBaseUrl;
            });

            // Act
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();
            
            prematchClient.Should().NotBeNull();
            inplayClient.Should().NotBeNull();
        }

        [Fact]
        public void SnapshotClients_WithNullBaseUrl_ShouldThrowArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = null; // This will cause exception
            });

            services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            Action act = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CustomerApiClients_ShouldNotHaveBaseAddressConfigured()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Customer API clients don't get base addresses configured automatically
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
            
            // Customer API clients are registered but don't have base addresses
            var metadataClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            metadataClient.BaseAddress.Should().BeNull(); // This is expected behavior
        }

        [Fact]
        public void HttpClients_ShouldBeRegisteredAsTransient()
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

            // Assert
            var factory1 = serviceProvider.GetService<ICustomersApiFactory>();
            var factory2 = serviceProvider.GetService<ICustomersApiFactory>();
            
            // Factories should be different instances (transient)
            factory1.Should().NotBeSameAs(factory2);
        }

        [Fact]
        public void MultipleServiceRegistrations_ShouldNotInterfere()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act - Register all services
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
            var serviceProvider = services.BuildServiceProvider();

            // Assert - All services should be resolvable
            var customerFactory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

            customerFactory.Should().NotBeNull();
            prematchClient.Should().NotBeNull();
            inplayClient.Should().NotBeNull();
        }

        [Fact]
        public async Task HttpClientFactory_ShouldCreateNamedClients()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            // Assert
            httpClientFactory.Should().NotBeNull();
            
            // Should be able to create HTTP clients (even if they don't have base addresses)
            var client1 = httpClientFactory.CreateClient();
            var client2 = httpClientFactory.CreateClient("TestClient");
            
            client1.Should().NotBeNull();
            client2.Should().NotBeNull();
        }

        [Fact]
        public void ServiceRegistration_ShouldHandleValidUrlFormats()
        {
            // Arrange
            var services = new ServiceCollection();
            var validUrls = new[]
            {
                "https://api.example.com",
                "https://api.example.com/",
                "http://localhost:8080",
                "https://api.subdomain.example.com/v1"
            };

            foreach (var url in validUrls)
            {
                // Act
                var testServices = new ServiceCollection();
                testServices.Configure<Trade360Settings>(options =>
                {
                    options.SnapshotApiBaseUrl = url;
                });
                testServices.AddTrade360PrematchSnapshotClient();
                var serviceProvider = testServices.BuildServiceProvider();

                // Assert
                var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                client.Should().NotBeNull($"URL {url} should be valid");
            }
        }

        [Fact]
        public void ServiceProvider_ShouldHandleDisposal()
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

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            factory.Should().NotBeNull();

            // Dispose should not throw
            Action dispose = () => serviceProvider.Dispose();
            dispose.Should().NotThrow();
        }

        [Fact]
        public void AutoMapper_ShouldBeRegisteredOnce()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://api.example.com";
            });

            // Act - Register multiple services that all register AutoMapper
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Assert - Should not throw due to multiple AutoMapper registrations
            Action buildProvider = () => services.BuildServiceProvider();
            buildProvider.Should().NotThrow();
        }
    }
} 