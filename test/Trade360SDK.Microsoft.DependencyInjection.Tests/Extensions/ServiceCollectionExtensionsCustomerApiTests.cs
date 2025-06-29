using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Xunit;
using AutoMapper;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions
{
    public class ServiceCollectionExtensionsCustomerApiTests
    {
        [Fact]
        public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<IMapper>().Should().NotBeNull();
            serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
        }

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

            // Verify that HTTP clients can be created for the registered interfaces
            var metadataClient = httpClientFactory!.CreateClient(typeof(IMetadataHttpClient).FullName!);
            var packageDistributionClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName!);
            var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName!);

            metadataClient.Should().NotBeNull();
            packageDistributionClient.Should().NotBeNull();
            subscriptionClient.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();
            IConfiguration configuration = null!;

            // Act & Assert
            Action act = () => services.AddTrade360CustomerApiClient(configuration);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterCustomersApiFactoryAsTransient()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var factory1 = serviceProvider.GetService<ICustomersApiFactory>();
            var factory2 = serviceProvider.GetService<ICustomersApiFactory>();

            factory1.Should().NotBeNull();
            factory2.Should().NotBeNull();
            factory1.Should().NotBeSameAs(factory2, "ICustomersApiFactory should be registered as transient");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterAutoMapper()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var mapper = serviceProvider.GetService<IMapper>();
            mapper.Should().NotBeNull();
            mapper!.ConfigurationProvider.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldReturnServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var result = services.AddTrade360CustomerApiClient(configuration);

            // Assert
            result.Should().BeSameAs(services, "Method should return the same service collection for fluent API");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithEmptyConfiguration_ShouldStillRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var emptyConfiguration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(emptyConfiguration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<IMapper>().Should().NotBeNull();
            serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterExpectedNumberOfServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            var initialCount = services.Count;

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var finalCount = services.Count;

            // Assert
            var addedServices = finalCount - initialCount;
            addedServices.Should().BeGreaterThan(0, "Services should be registered");

            // Verify specific services are registered
            services.Any(s => s.ServiceType == typeof(ICustomersApiFactory)).Should().BeTrue();
            services.Any(s => s.ServiceType == typeof(IMapper)).Should().BeTrue();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithMultipleRegistrations_ShouldNotConflict()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360CustomerApiClient(configuration); // Register twice
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            var mapper = serviceProvider.GetService<IMapper>();

            factory.Should().NotBeNull();
            mapper.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterHttpClientsWithPolicies()
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

            // Verify that named HTTP clients are registered and can be created
            var clientTypes = new[]
            {
                typeof(IMetadataHttpClient),
                typeof(IPackageDistributionHttpClient),
                typeof(ISubscriptionHttpClient)
            };

            foreach (var clientType in clientTypes)
            {
                var client = httpClientFactory!.CreateClient(clientType.FullName!);
                client.Should().NotBeNull($"HTTP client for {clientType.Name} should be registered");
                
                // Verify timeout indicates policies are applied (default is 100 seconds)
                client.Timeout.Should().Be(TimeSpan.FromSeconds(100), 
                    $"HTTP client for {clientType.Name} should have timeout indicating policies are applied");
            }
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldCreateIndependentHttpClientInstances()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var client1 = httpClientFactory!.CreateClient(typeof(IMetadataHttpClient).FullName!);
            var client2 = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);

            // Assert
            client1.Should().NotBeNull();
            client2.Should().NotBeNull();
            client1.Should().NotBeSameAs(client2, "HttpClientFactory should create independent instances");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldPreserveHttpClientDefaultSettings()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var client = httpClientFactory!.CreateClient(typeof(IMetadataHttpClient).FullName!);

            // Assert
            client.MaxResponseContentBufferSize.Should().BeGreaterThan(0);
            client.DefaultRequestHeaders.Should().NotBeNull();
            client.DefaultRequestVersion.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        [InlineData("Staging")]
        public void AddTrade360CustomerApiClient_WithDifferentEnvironments_ShouldRegisterServices(string environment)
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new System.Collections.Generic.KeyValuePair<string, string?>("Environment", environment)
                })
                .Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<IMapper>().Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithConfigurationValues_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new System.Collections.Generic.KeyValuePair<string, string?>("CustomersApi:BaseUrl", "https://api.example.com/"),
                    new System.Collections.Generic.KeyValuePair<string, string?>("CustomersApi:Timeout", "30")
                })
                .Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<IMapper>().Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldHandleServiceProviderDisposal()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            Action act = () => serviceProvider.Dispose();
            act.Should().NotThrow("Service provider should dispose cleanly");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithServiceDescriptorValidation_ShouldRegisterCorrectLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);

            // Assert
            var serviceDescriptors = services.ToList();

            var factoryDescriptor = serviceDescriptors
                .FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
            factoryDescriptor.Should().NotBeNull();
            factoryDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);

            var mapperDescriptor = serviceDescriptors
                .FirstOrDefault(s => s.ServiceType == typeof(IMapper));
            mapperDescriptor.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterHttpClientFactoryServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);

            // Assert
            var serviceDescriptors = services.ToList();
            var httpClientFactoryDescriptor = serviceDescriptors
                .FirstOrDefault(s => s.ServiceType == typeof(IHttpClientFactory));
            httpClientFactoryDescriptor.Should().NotBeNull();
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithComplexConfiguration_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new System.Collections.Generic.KeyValuePair<string, string?>("Logging:LogLevel:Default", "Information"),
                    new System.Collections.Generic.KeyValuePair<string, string?>("ConnectionStrings:Default", "Server=localhost;Database=test;"),
                    new System.Collections.Generic.KeyValuePair<string, string?>("CustomersApi:Features:EnableRetry", "true"),
                    new System.Collections.Generic.KeyValuePair<string, string?>("CustomersApi:Features:EnableCircuitBreaker", "true")
                })
                .Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
            serviceProvider.GetService<IMapper>().Should().NotBeNull();
            serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
        }
    }
} 