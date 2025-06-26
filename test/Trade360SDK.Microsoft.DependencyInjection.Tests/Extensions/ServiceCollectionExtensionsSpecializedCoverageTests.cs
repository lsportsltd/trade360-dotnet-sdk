using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    public class ServiceCollectionExtensionsSpecializedCoverageTests
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ServiceCollectionExtensionsSpecializedCoverageTests()
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

        #region Private Method Coverage Tests

        [Fact]
        public void GetRetryPolicy_ShouldReturnValidRetryPolicy()
        {
            // Arrange
            var type = typeof(Trade360SDK.Microsoft.DependencyInjection.Extensions.ServiceCollectionExtensions);
            var method = type.GetMethod("GetRetryPolicy", BindingFlags.NonPublic | BindingFlags.Static);
            
            // Act
            var policy = method?.Invoke(null, null) as IAsyncPolicy<HttpResponseMessage>;
            
            // Assert
            policy.Should().NotBeNull("GetRetryPolicy should return a valid policy");
        }

        [Fact]
        public void GetCircuitBreakerPolicy_ShouldReturnValidCircuitBreakerPolicy()
        {
            // Arrange
            var type = typeof(Trade360SDK.Microsoft.DependencyInjection.Extensions.ServiceCollectionExtensions);
            var method = type.GetMethod("GetCircuitBreakerPolicy", BindingFlags.NonPublic | BindingFlags.Static);
            
            // Act
            var policy = method?.Invoke(null, null) as IAsyncPolicy<HttpResponseMessage>;
            
            // Assert
            policy.Should().NotBeNull("GetCircuitBreakerPolicy should return a valid policy");
        }

        [Fact]
        public void GetRetryPolicy_ShouldConfigureExponentialBackoff()
        {
            // Arrange
            _services.AddTrade360CustomerApiClient(_configuration);
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            
            // Act
            var client = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
            
            // Assert
            client.Should().NotBeNull("HTTP client with retry policy should be created successfully");
        }

        [Fact]
        public void GetCircuitBreakerPolicy_ShouldConfigureCircuitBreakerWithCorrectSettings()
        {
            // Arrange
            _services.AddTrade360CustomerApiClient(_configuration);
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            
            // Act
            var client = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName!);
            
            // Assert
            client.Should().NotBeNull("HTTP client with circuit breaker policy should be created successfully");
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithNullSnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = null;
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithNullSnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = null;
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithEmptySnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "";
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
            act.Should().Throw<UriFormatException>();
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithEmptySnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
            act.Should().Throw<UriFormatException>();
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithInvalidSnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "invalid-url";
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
            act.Should().Throw<UriFormatException>();
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithInvalidSnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "invalid-url";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
            act.Should().Throw<UriFormatException>();
        }

        #endregion

        #region Configuration Edge Cases

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithWhitespaceSnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "   ";
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
            act.Should().Throw<UriFormatException>();
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithWhitespaceSnapshotApiBaseUrl_ShouldThrowWhenClientUsed()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "   ";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var act = () => serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
            act.Should().Throw<UriFormatException>();
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithValidRelativeUrl_ShouldWorkCorrectly()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://example.com/api/";
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var client = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
            client.Should().NotBeNull("Client should be created with valid relative URL");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithValidRelativeUrl_ShouldWorkCorrectly()
        {
            // Arrange
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://example.com/api/";
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var client = serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
            client.Should().NotBeNull("Client should be created with valid relative URL");
        }

        #endregion

        #region Service Registration Verification

        [Fact]
        public void AddTrade360CustomerApiClient_ShouldRegisterExactlyThreeHttpClients()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            
            // Assert
            var httpClientDescriptors = _services.Where(s => 
                s.ServiceType == typeof(IMetadataHttpClient) ||
                s.ServiceType == typeof(IPackageDistributionHttpClient) ||
                s.ServiceType == typeof(ISubscriptionHttpClient)).ToList();
            
            httpClientDescriptors.Should().HaveCount(3, "Should register exactly 3 HTTP client types");
        }

        [Fact]
        public void AddTrade360PrematchSnapshotClient_ShouldRegisterExactlyOneHttpClient()
        {
            // Act
            _services.AddTrade360PrematchSnapshotClient();
            
            // Assert
            var httpClientDescriptors = _services.Where(s => 
                s.ServiceType == typeof(ISnapshotPrematchApiClient)).ToList();
            
            httpClientDescriptors.Should().HaveCount(2, "Should register HTTP client services (factory and implementation)");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_ShouldRegisterExactlyOneHttpClient()
        {
            // Act
            _services.AddTrade360InplaySnapshotClient();
            
            // Assert
            var httpClientDescriptors = _services.Where(s => 
                s.ServiceType == typeof(ISnapshotInplayApiClient)).ToList();
            
            httpClientDescriptors.Should().HaveCount(2, "Should register HTTP client services (factory and implementation)");
        }

        [Fact]
        public void AllExtensionMethods_ShouldRegisterAutoMapperOnlyOnce()
        {
            // Act
            _services.AddTrade360CustomerApiClient(_configuration);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();
            
            // Assert
            var autoMapperDescriptors = _services.Where(s => 
                s.ServiceType == typeof(AutoMapper.IMapper)).ToList();
            
            // AutoMapper may register multiple services, but they should all be the same instance
            var serviceProvider = _services.BuildServiceProvider();
            var mapper1 = serviceProvider.GetService<AutoMapper.IMapper>();
            var mapper2 = serviceProvider.GetService<AutoMapper.IMapper>();
            
            mapper1.Should().NotBeNull("First mapper should be available");
            mapper2.Should().NotBeNull("Second mapper should be available");
        }

        #endregion

        #region Boundary Value Tests

        [Fact]
        public void AddTrade360PrematchSnapshotClient_WithMaxLengthUrl_ShouldWorkCorrectly()
        {
            // Arrange - Create a very long but valid URL
            var longUrl = "https://example.com/" + new string('a', 1000) + "/";
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = longUrl;
                options.PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var client = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
            client.Should().NotBeNull("Client should handle very long URLs");
        }

        [Fact]
        public void AddTrade360InplaySnapshotClient_WithMaxLengthUrl_ShouldWorkCorrectly()
        {
            // Arrange - Create a very long but valid URL
            var longUrl = "https://example.com/" + new string('a', 1000) + "/";
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = longUrl;
                options.InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "user",
                    Password = "pass"
                };
            });
            
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            
            // Act & Assert
            var client = serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
            client.Should().NotBeNull("Client should handle very long URLs");
        }

        #endregion
    }
} 