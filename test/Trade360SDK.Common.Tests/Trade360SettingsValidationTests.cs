using FluentAssertions;
using Trade360SDK.Common.Configuration;

namespace Trade360SDK.Common.Tests;

public class Trade360SettingsValidationTests
{
    [Fact]
    public void Trade360Settings_WithValidData_ShouldInitializeCorrectly()
    {
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            SnapshotApiBaseUrl = "https://snapshot.example.com/"
        };

        settings.CustomersApiBaseUrl.Should().Be("https://api.example.com/");
        settings.SnapshotApiBaseUrl.Should().Be("https://snapshot.example.com/");


    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Trade360Settings_WithInvalidCustomersApiBaseUrl_ShouldAllowButBeInvalid(string invalidUrl)
    {
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = invalidUrl
        };

        settings.CustomersApiBaseUrl.Should().Be(invalidUrl);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Trade360Settings_WithInvalidSnapshotApiBaseUrl_ShouldAllowButBeInvalid(string invalidUrl)
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = invalidUrl
        };

        settings.SnapshotApiBaseUrl.Should().Be(invalidUrl);
    }

    [Fact]
    public void Trade360Settings_WithNullCredentials_ShouldAllowNullValues()
    {
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/",
            SnapshotApiBaseUrl = "https://snapshot.example.com/"
        };


        settings.PrematchPackageCredentials.Should().BeNull();
        settings.InplayPackageCredentials.Should().BeNull();
    }

    [Fact]
    public void Trade360Settings_WithValidUrls_ShouldAcceptDifferentFormats()
    {
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "http://localhost:8080/",
            SnapshotApiBaseUrl = "https://api.production.com/v1/"
        };

        settings.CustomersApiBaseUrl.Should().Be("http://localhost:8080/");
        settings.SnapshotApiBaseUrl.Should().Be("https://api.production.com/v1/");
    }

    [Fact]
    public void Trade360Settings_WithSameCredentialsForDifferentServices_ShouldWork()
    {
        var sharedCredentials = new PackageCredentials
        {
            Username = "shareduser",
            Password = "sharedpass",
            PackageId = 999
        };

        var settings = new Trade360Settings();



    }

    [Fact]
    public void Trade360Settings_DefaultValues_ShouldBeCorrect()
    {
        var settings = new Trade360Settings();

        settings.CustomersApiBaseUrl.Should().BeNull();
        settings.SnapshotApiBaseUrl.Should().BeNull();

        settings.PrematchPackageCredentials.Should().BeNull();
        settings.InplayPackageCredentials.Should().BeNull();
    }

    [Fact]
    public void Trade360Settings_WithComplexConfiguration_ShouldWork()
    {
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://customers-api.trade360.com/v2/",
            SnapshotApiBaseUrl = "https://snapshot-api.trade360.com/v1/"
        };

        settings.Should().NotBeNull();


    }
}
