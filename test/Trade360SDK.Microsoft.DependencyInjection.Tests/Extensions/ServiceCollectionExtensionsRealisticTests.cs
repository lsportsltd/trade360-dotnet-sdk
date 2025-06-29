using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;
using FluentAssertions;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.Common.Configuration;
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions
{
    public class ServiceCollectionExtensionsRealisticTests
    {
        [Fact]
        public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                services.AddTrade360CustomerApiClient(null!));
        }

        [Fact]
        public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            // Act
            var result = services.AddTrade360CustomerApiClient(configuration);

            // Assert
            result.Should().BeSameAs(services, "method should return the same service collection for chaining");
            
            // Verify service registrations
            var serviceProvider = services.BuildServiceProvider();
            
            // Should be able to resolve the factory
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            factory.Should().NotBeNull("CustomersApiFactory should be registered");
            
            // Should be able to resolve AutoMapper
            var mapper = serviceProvider.GetService<IMapper>();
            mapper.Should().NotBeNull("AutoMapper should be registered");
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterHttpClients()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull("HttpClientFactory should be available");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // Add required Trade360Settings
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
            });

            // Act
            var result = services.AddTrade360PrematchSnapshotClient();

            // Assert
            result.Should().BeSameAs(services, "method should return the same service collection for chaining");
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Should be able to resolve AutoMapper
            var mapper = serviceProvider.GetService<IMapper>();
            mapper.Should().NotBeNull("AutoMapper should be registered");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // Add required Trade360Settings
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.InplayPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
            });

            // Act
            var result = services.AddTrade360InplaySnapshotClient();

            // Assert
            result.Should().BeSameAs(services, "method should return the same service collection for chaining");
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Should be able to resolve AutoMapper
            var mapper = serviceProvider.GetService<IMapper>();
            mapper.Should().NotBeNull("AutoMapper should be registered");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithMissingSettings_ShouldThrowWhenResolvingClient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTrade360PrematchSnapshotClient();

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Should throw when trying to resolve client without proper settings
            Assert.Throws<InvalidOperationException>(() => 
                serviceProvider.GetService<ISnapshotPrematchApiClient>());
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithMissingSettings_ShouldThrowWhenResolvingClient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTrade360InplaySnapshotClient();

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Should throw when trying to resolve client without proper settings
            Assert.Throws<InvalidOperationException>(() => 
                serviceProvider.GetService<ISnapshotInplayApiClient>());
        }

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterMultipleHttpClients()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            
            // Verify we can get HttpClientFactory which indicates HTTP clients were registered
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
            
            // The specific HTTP clients should be available through the factory
            var metadataClient = httpClientFactory?.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
            metadataClient.Should().NotBeNull();
        }

        [Fact]
        public void ServiceRegistration_ShouldHandleMultipleRegistrations()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
                options.InplayPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
            });

            // Act - Register all services
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            
            // All services should be resolvable
            var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
            customersFactory.Should().NotBeNull("Customers API factory should be registered");
            
            var mapper = serviceProvider.GetService<IMapper>();
            mapper.Should().NotBeNull("AutoMapper should be registered");
            
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull("HttpClientFactory should be registered");
        }

        [Fact]
        public void ServiceRegistration_ShouldAllowChaining()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
                options.InplayPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
            });

            // Act - Test method chaining
            var result = services
                .AddTrade360CustomerApiClient(configuration)
                .AddTrade360PrematchSnapshotClient()
                .AddTrade360InplaySnapshotClient();

            // Assert
            result.Should().BeSameAs(services, "chaining should return the original service collection");
        }

        [Theory]
        [InlineData("https://api.example.com/")]
        [InlineData("https://snapshot-api.example.com/v1/")]
        [InlineData("http://localhost:8080/")]
        public void AddTrade360PrematchSnapshotClient_WithDifferentBaseUrls_ShouldAcceptValidUrls(string baseUrl)
        {
            // Arrange
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = baseUrl;
                options.PrematchPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
            });

            // Act
            services.AddTrade360PrematchSnapshotClient();

            // Assert - Should not throw during registration
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.Should().NotBeNull("Service provider should be created successfully");
        }

        [Fact]
        public void ServiceRegistration_WithEmptyServiceCollection_ShouldWork()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            // Act
            services.AddTrade360CustomerApiClient(configuration);

            // Assert
            services.Should().NotBeEmpty("Services should be registered");
            services.Count.Should().BeGreaterThan(0, "At least one service should be registered");
        }

        [Fact]
        public void AutoMapperRegistration_ShouldBeRepeatable()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = new PackageCredentials 
                { 
                    PackageId = 123, 
                    Username = "test-user", 
                    Password = "test-password" 
                };
            });

            // Act - Register services multiple times (which adds AutoMapper multiple times)
            services.AddTrade360CustomerApiClient(configuration);
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360CustomerApiClient(configuration); // Duplicate registration

            // Assert - Should not throw
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            mapper.Should().NotBeNull("AutoMapper should still be resolvable despite multiple registrations");
        }
    }
} 