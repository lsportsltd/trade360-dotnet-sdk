using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Validators;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class RmqConnectionSettingsValidatorTests
{
    [Fact]
    public void Validate_WithValidSettings_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 30
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidHost_ShouldThrowArgumentException(string? host)
    {
        var settings = new RmqConnectionSettings
        {
            Host = host,
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Host is required.*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidPort_ShouldThrowArgumentException(int port)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = port,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Port must be a positive integer.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidVirtualHost_ShouldThrowArgumentException(string? virtualHost)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = virtualHost,
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("VirtualHost is required.*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidPackageId_ShouldThrowArgumentException(int packageId)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = packageId,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("PackageId must be a positive integer.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidUserName_ShouldThrowArgumentException(string? userName)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = userName,
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("UserName is required.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidPassword_ShouldThrowArgumentException(string? password)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = password
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Password is required.*");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(5)]
    [InlineData(0)]
    public void Validate_WithInvalidHeartbeatSeconds_ShouldThrowArgumentException(int heartbeatSeconds)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            RequestedHeartbeatSeconds = heartbeatSeconds
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("RequestedHeartbeatSeconds must be a positive integer - Larger then 10.*");
    }

    [Theory]
    [InlineData(15)]
    [InlineData(10)]
    [InlineData(0)]
    public void Validate_WithInvalidNetworkRecoveryInterval_ShouldThrowArgumentException(int networkRecoveryInterval)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            NetworkRecoveryInterval = networkRecoveryInterval
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("NetworkRecoveryInterval must be a positive integer.*");
    }
}
