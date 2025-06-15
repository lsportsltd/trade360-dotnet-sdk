using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Validators;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class RmqConnectionSettingsValidatorBusinessLogicTests
{
    [Fact]
    public void Validate_WithValidSettings_ShouldNotThrow()
    {
        var validSettings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20,
            PrefetchCount = 100
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(validSettings);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNullHost_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = null,
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Host*");
    }

    [Fact]
    public void Validate_WithEmptyHost_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Host*");
    }

    [Fact]
    public void Validate_WithWhitespaceHost_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "   ",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Host*");
    }

    [Fact]
    public void Validate_WithZeroPort_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 0,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Port*");
    }

    [Fact]
    public void Validate_WithNegativePort_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = -1,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Port*");
    }

    [Fact]
    public void Validate_WithPortTooHigh_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 70000,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            RequestedHeartbeatSeconds = 30
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNullVirtualHost_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = null,
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*VirtualHost*");
    }

    [Fact]
    public void Validate_WithEmptyVirtualHost_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "",
            PackageId = 1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*VirtualHost*");
    }

    [Fact]
    public void Validate_WithZeroPackageId_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 0,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*PackageId*");
    }

    [Fact]
    public void Validate_WithNegativePackageId_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = -1,
            UserName = "user",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*PackageId*");
    }

    [Fact]
    public void Validate_WithNullUserName_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = null,
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*UserName*");
    }

    [Fact]
    public void Validate_WithEmptyUserName_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "",
            Password = "password"
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*UserName*");
    }

    [Fact]
    public void Validate_WithNullPassword_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = null
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Password*");
    }

    [Fact]
    public void Validate_WithEmptyPassword_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = ""
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*Password*");
    }

    [Fact]
    public void Validate_WithNegativeHeartbeat_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            RequestedHeartbeatSeconds = -1
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*RequestedHeartbeatSeconds*");
    }

    [Fact]
    public void Validate_WithNegativeNetworkRecoveryInterval_ShouldThrowArgumentException()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            NetworkRecoveryInterval = -1
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>().WithMessage("*NetworkRecoveryInterval*");
    }

    [Fact]
    public void Validate_WithZeroPrefetchCount_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "password",
            PrefetchCount = 0,
            RequestedHeartbeatSeconds = 30
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithBoundaryValues_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 1,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "u",
            Password = "p",
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20,
            PrefetchCount = 0
        };

        Action act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }
}
