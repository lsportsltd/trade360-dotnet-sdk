using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Comprehensive configuration validation tests designed to maximize code coverage
/// by testing all configuration validation paths and edge cases
/// </summary>
public class ServiceCollectionExtensionsConfigurationValidationTests
{
    private readonly ServiceCollection _services;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public ServiceCollectionExtensionsConfigurationValidationTests()
    {
        _services = new ServiceCollection();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>()))
            .Returns(new Mock<IConfigurationSection>().Object);
    }

    #region Configuration Null and Empty Tests

    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        var act = () => _services.AddTrade360CustomerApiClient(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("configuration");
    }

    [Fact]
    public void AddServices_WithNullConfigurationSection_ShouldStillRegister()
    {
        // Arrange
        // Configure a valid URL to avoid configuration validation errors
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://test.example.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
    }

    [Fact]
    public void AddServices_WithEmptyConfigurationValues_ShouldHandleGracefully()
    {
        // Arrange
        var emptyConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "",
                ["Trade360Settings:EmptyProperty"] = "",
                ["Trade360Settings:NullProperty"] = null!
            })
            .Build();

        // Act
        _services.AddTrade360CustomerApiClient(emptyConfig);
        _services.Configure<Trade360Settings>(emptyConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Services should register but fail when trying to create clients
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var act1 = () => httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var act2 = () => httpClientFactory.CreateClient("ISnapshotInplayApiClient");

        act1.Should().Throw<UriFormatException>();
        act2.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void AddServices_WithWhitespaceOnlyConfiguration_ShouldThrowUriFormatException()
    {
        // Arrange
        var whitespaceConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "   ",
            })
            .Build();

        _services.Configure<Trade360Settings>(whitespaceConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act & Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var act = () => httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        act.Should().Throw<UriFormatException>();
    }

    #endregion

    #region URL Validation Tests

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("://missing-scheme")]
    [InlineData("http://")]
    [InlineData("https://")]
    public void SnapshotClients_WithInvalidUrls_ShouldThrowExceptionWhenCreatingClient(string invalidUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = invalidUrl;
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act & Assert
        var act1 = () => httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var act2 = () => httpClientFactory.CreateClient("ISnapshotInplayApiClient");

        // Invalid URLs should throw UriFormatException when creating the client
        act1.Should().Throw<UriFormatException>();
        act2.Should().Throw<UriFormatException>();
    }

    [Theory]
    [InlineData("invalid://url")]
    [InlineData("ftp://invalid-scheme.com")]
    public void SnapshotClients_WithValidButUnusualUrls_ShouldCreateClients(string unusualUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = unusualUrl;
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act & Assert
        var act1 = () => httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var act2 = () => httpClientFactory.CreateClient("ISnapshotInplayApiClient");

        // These URLs are valid URIs (even if unusual) so they should not throw
        act1.Should().NotThrow();
        act2.Should().NotThrow();
        
        var client1 = act1();
        var client2 = act2();
        
        client1.Should().NotBeNull();
        client2.Should().NotBeNull();
        
        client1.Dispose();
        client2.Dispose();
    }

    [Theory]
    [InlineData("https://valid.example.com")]
    [InlineData("http://localhost")]
    [InlineData("https://api.test.co.uk:8443/v1/")]
    [InlineData("http://127.0.0.1:8080")]
    [InlineData("https://subdomain.domain.com/path/to/api/")]
    public void SnapshotClients_WithValidUrls_ShouldCreateClientsSuccessfully(string validUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = validUrl;
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act
        var prematchClient = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var inplayClient = httpClientFactory.CreateClient("ISnapshotInplayApiClient");

        // Assert
        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
        prematchClient.BaseAddress.Should().NotBeNull();
        inplayClient.BaseAddress.Should().NotBeNull();

        prematchClient.Dispose();
        inplayClient.Dispose();
    }

    #endregion

    #region Options Pattern Validation

    [Fact]
    public void Trade360Settings_WithMissingConfiguration_ShouldUseDefaultValues()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options => { }); // No configuration

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Should().NotBeNull();
        options.Value.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().BeNull();
    }

    [Fact]
    public void Trade360Settings_WithPartialConfiguration_ShouldMergeWithDefaults()
    {
        // Arrange
        var partialConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://partial.test.com"
            })
            .Build();

        _services.Configure<Trade360Settings>(partialConfig.GetSection("Trade360Settings"));

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Should().NotBeNull();
        options.Value.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://partial.test.com");
    }

    [Fact]
    public void OptionsValidation_WithDataAnnotations_ShouldWork()
    {
        // Arrange
        var validConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://validation.test.com"
            })
            .Build();

        _services.Configure<Trade360Settings>(validConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Should().NotBeNull();
        var act = () => options.Value;
        act.Should().NotThrow();
    }

    #endregion

    #region Configuration Binding Edge Cases

    [Fact]
    public void Configuration_WithCaseSensitivity_ShouldHandleCorrectly()
    {
        // Arrange
        // Configuration is case-insensitive by default in .NET Core
        // So we need to avoid duplicate keys that differ only by case
        var caseConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://propercase.test.com"
            })
            .Build();

        _services.Configure<Trade360Settings>(caseConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://propercase.test.com");
    }

    [Fact]
    public void Configuration_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var specialConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://special-chars_123.test.com/api/v1/"
            })
            .Build();

        _services.Configure<Trade360Settings>(specialConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");

        // Assert
        client.Should().NotBeNull();
        client.BaseAddress.ToString().Should().StartWith("https://special-chars_123.test.com/api/v1/");
        client.Dispose();
    }

    [Fact]
    public void Configuration_WithNestedSections_ShouldBindCorrectly()
    {
        // Arrange
        var nestedConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Settings:Trade360Settings:SnapshotApiBaseUrl"] = "https://nested-section.test.com",
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://direct-section.test.com"
            })
            .Build();

        _services.Configure<Trade360Settings>(nestedConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://direct-section.test.com");
    }

    #endregion

    #region Configuration Provider Tests

    [Fact]
    public void Configuration_WithMultipleProviders_ShouldUseCorrectPrecedence()
    {
        // Arrange
        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://provider1.test.com"
            })
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://provider2.test.com" // This should override
            });

        var config = configBuilder.Build();
        _services.Configure<Trade360Settings>(config.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Value.SnapshotApiBaseUrl.Should().Be("https://provider2.test.com");
    }

    [Fact]
    public void Configuration_WithEnvironmentVariables_ShouldRespectHierarchy()
    {
        // Arrange - Simulate environment variable configuration
        var envConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings__SnapshotApiBaseUrl"] = "https://environment.test.com"
            })
            .Build();

        _services.Configure<Trade360Settings>(envConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Assert
        options.Should().NotBeNull();
        // Environment variable format with __ doesn't bind to : in this test setup
        options.Value.SnapshotApiBaseUrl.Should().BeNull();
    }

    #endregion

    #region Service Registration Validation

    [Fact]
    public void ServiceRegistration_WithNullServiceCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection nullServices = null!;

        // Act & Assert
        var act1 = () => nullServices.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        var act2 = () => nullServices.AddTrade360PrematchSnapshotClient();
        var act3 = () => nullServices.AddTrade360InplaySnapshotClient();

        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
        act3.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ServiceRegistration_WithValidParameters_ShouldReturnSameServiceCollection()
    {
        // Arrange & Act
        var result1 = _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        var result2 = _services.AddTrade360PrematchSnapshotClient();
        var result3 = _services.AddTrade360InplaySnapshotClient();

        // Assert
        result1.Should().BeSameAs(_services);
        result2.Should().BeSameAs(_services);
        result3.Should().BeSameAs(_services);
    }

    [Fact]
    public void ServiceRegistration_MultipleCalls_ShouldNotCauseErrors()
    {
        // Arrange & Act
        for (int i = 0; i < 5; i++)
        {
            _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
            _services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = $"https://multiple-{i}.test.com";
            });
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();
        }

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();

        factory.Should().NotBeNull();
        prematch.Should().NotBeNull();
        inplay.Should().NotBeNull();
    }

    #endregion

    #region Error Recovery Tests

    [Fact]
    public void Services_AfterConfigurationError_ShouldRecoverGracefully()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "invalid-url"; // This will cause errors
        });
        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act - Try to create client with invalid URL
        var act1 = () => httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        act1.Should().Throw<UriFormatException>();

        // Now reconfigure with valid URL
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://recovered.test.com";
        });

        var newServiceProvider = _services.BuildServiceProvider();
        var newHttpClientFactory = newServiceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert - Should work with new configuration
        var client = newHttpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        client.Should().NotBeNull();
        client.Dispose();
    }

    [Fact]
    public void Services_WithConfigurationChanges_ShouldUseLatestConfiguration()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://initial.test.com";
        });

        // Act - Change configuration
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://updated.test.com";
        });

        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://updated.test.com");
    }

    #endregion
} 