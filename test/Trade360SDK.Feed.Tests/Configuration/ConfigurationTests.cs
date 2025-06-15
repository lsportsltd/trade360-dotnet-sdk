using FluentAssertions;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed.Tests;

public class ConfigurationTests
{
    [Fact]
    public void RmqConnectionSettings_ShouldInitializeWithDefaults()
    {
        var settings = new RmqConnectionSettings();

        settings.Host.Should().BeNull();
        settings.Port.Should().Be(0);
        settings.VirtualHost.Should().BeNull();
        settings.PackageId.Should().Be(0);
        settings.UserName.Should().BeNull();
        settings.Password.Should().BeNull();
    }

    [Fact]
    public void RmqConnectionSettings_ShouldAllowSettingProperties()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "testuser",
            Password = "testpass"
        };

        settings.Host.Should().Be("localhost");
        settings.Port.Should().Be(5672);
        settings.VirtualHost.Should().Be("/");
        settings.PackageId.Should().Be(123);
        settings.UserName.Should().Be("testuser");
        settings.Password.Should().Be("testpass");
    }

    [Theory]
    [InlineData("localhost", 5672, "/", 1, "user", "pass")]
    [InlineData("rabbitmq.example.com", 5673, "/vhost", 999, "admin", "secret")]
    public void RmqConnectionSettings_ShouldAcceptDifferentValues(string host, int port, string vhost, int packageId, string username, string password)
    {
        var settings = new RmqConnectionSettings
        {
            Host = host,
            Port = port,
            VirtualHost = vhost,
            PackageId = packageId,
            UserName = username,
            Password = password
        };

        settings.Host.Should().Be(host);
        settings.Port.Should().Be(port);
        settings.VirtualHost.Should().Be(vhost);
        settings.PackageId.Should().Be(packageId);
        settings.UserName.Should().Be(username);
        settings.Password.Should().Be(password);
    }
}
