using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Validators;

namespace Trade360SDK.Feed.Tests;

public class ValidationTests
{
    [Fact]
    public void RmqConnectionSettingsValidator_WithValidSettings_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "pass",
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null, 5672, "/", 1, "user", "pass", 30, 20)]
    [InlineData("", 5672, "/", 1, "user", "pass", 30, 20)]
    [InlineData("localhost", 0, "/", 1, "user", "pass", 30, 20)]
    [InlineData("localhost", -1, "/", 1, "user", "pass", 30, 20)]
    [InlineData("localhost", 5672, null, 1, "user", "pass", 30, 20)]
    [InlineData("localhost", 5672, "", 1, "user", "pass", 30, 20)]
    [InlineData("localhost", 5672, "/", 0, "user", "pass", 30, 20)]
    [InlineData("localhost", 5672, "/", -1, "user", "pass", 30, 20)]
    [InlineData("localhost", 5672, "/", 1, null, "pass", 30, 20)]
    [InlineData("localhost", 5672, "/", 1, "", "pass", 30, 20)]
    [InlineData("localhost", 5672, "/", 1, "user", null, 30, 20)]
    [InlineData("localhost", 5672, "/", 1, "user", "", 30, 20)]
    [InlineData("localhost", 5672, "/", 1, "user", "pass", 5, 20)]
    [InlineData("localhost", 5672, "/", 1, "user", "pass", 30, 10)]
    public void RmqConnectionSettingsValidator_WithInvalidSettings_ShouldThrowArgumentException(
        string host, int port, string virtualHost, int packageId, string userName, string password, int heartbeat, int recovery)
    {
        var settings = new RmqConnectionSettings
        {
            Host = host,
            Port = port,
            VirtualHost = virtualHost,
            PackageId = packageId,
            UserName = userName,
            Password = password,
            RequestedHeartbeatSeconds = heartbeat,
            NetworkRecoveryInterval = recovery
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>();
    }
}
