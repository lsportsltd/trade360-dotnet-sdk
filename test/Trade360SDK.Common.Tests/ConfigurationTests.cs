using FluentAssertions;
using Trade360SDK.Common.Configuration;

namespace Trade360SDK.Common.Tests;

public class ConfigurationTests
{
    [Fact]
    public void Trade360Settings_ShouldInitializeWithDefaults()
    {
        var settings = new Trade360Settings();

        settings.CustomersApiBaseUrl.Should().BeNull();
        settings.SnapshotApiBaseUrl.Should().BeNull();
        settings.InplayPackageCredentials.Should().BeNull();
        settings.PrematchPackageCredentials.Should().BeNull();
    }

    [Fact]
    public void Trade360Settings_ShouldAllowSettingProperties()
    {
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://customers.api.com",
            SnapshotApiBaseUrl = "https://snapshot.api.com",
            InplayPackageCredentials = new PackageCredentials { PackageId = 1 },
            PrematchPackageCredentials = new PackageCredentials { PackageId = 2 }
        };

        settings.CustomersApiBaseUrl.Should().Be("https://customers.api.com");
        settings.SnapshotApiBaseUrl.Should().Be("https://snapshot.api.com");
        settings.InplayPackageCredentials.Should().NotBeNull();
        settings.InplayPackageCredentials.PackageId.Should().Be(1);
        settings.PrematchPackageCredentials.Should().NotBeNull();
        settings.PrematchPackageCredentials.PackageId.Should().Be(2);
    }

    [Fact]
    public void PackageCredentials_ShouldInitializeWithDefaults()
    {
        var credentials = new PackageCredentials();

        credentials.PackageId.Should().Be(0);
        credentials.Username.Should().BeNull();
        credentials.Password.Should().BeNull();
        credentials.MessageFormat.Should().Be("json");
    }

    [Fact]
    public void PackageCredentials_ShouldAllowSettingProperties()
    {
        var credentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "testuser",
            Password = "testpass",
            MessageFormat = "xml"
        };

        credentials.PackageId.Should().Be(123);
        credentials.Username.Should().Be("testuser");
        credentials.Password.Should().Be("testpass");
        credentials.MessageFormat.Should().Be("xml");
    }

    [Theory]
    [InlineData("json")]
    [InlineData("xml")]
    [InlineData("protobuf")]
    public void PackageCredentials_MessageFormat_ShouldAcceptDifferentFormats(string format)
    {
        var credentials = new PackageCredentials
        {
            MessageFormat = format
        };

        credentials.MessageFormat.Should().Be(format);
    }
}
