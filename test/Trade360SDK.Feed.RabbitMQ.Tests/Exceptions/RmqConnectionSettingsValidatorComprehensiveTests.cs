using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Validators;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class RmqConnectionSettingsValidatorComprehensiveTests
{
    [Fact]
    public void Validate_WithValidSettings_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Validate_WithInvalidHost_ShouldThrowArgumentException(string invalidHost)
    {
        var settings = new RmqConnectionSettings
        {
            Host = invalidHost,
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Host is required.*")
           .And.ParamName.Should().Be("Host");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_WithInvalidPort_ShouldThrowArgumentException(int invalidPort)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = invalidPort,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Port must be a positive integer.*")
           .And.ParamName.Should().Be("Port");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5672)]
    [InlineData(15672)]
    [InlineData(65535)]
    public void Validate_WithValidPort_ShouldNotThrow(int validPort)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = validPort,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Validate_WithInvalidUserName_ShouldThrowArgumentException(string invalidUserName)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = invalidUserName,
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("UserName is required.*")
           .And.ParamName.Should().Be("UserName");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Validate_WithInvalidPassword_ShouldThrowArgumentException(string invalidPassword)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = invalidPassword,
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Password is required.*")
           .And.ParamName.Should().Be("Password");
    }

    [Fact]
    public void Validate_WithNullSettings_ShouldThrowNullReferenceException()
    {
        var act = () => RmqConnectionSettingsValidator.Validate(null);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Validate_WithNullSettings_ShouldThrowNullReferenceException_Alternative()
    {
        RmqConnectionSettings? nullSettings = null;

        var act = () => RmqConnectionSettingsValidator.Validate(nullSettings!);
        
        act.Should().Throw<NullReferenceException>();
    }

    [Theory]
    [InlineData("production-host")]
    [InlineData("192.168.1.100")]
    [InlineData("rabbitmq.example.com")]
    [InlineData("localhost")]
    public void Validate_WithValidHosts_ShouldNotThrow(string validHost)
    {
        var settings = new RmqConnectionSettings
        {
            Host = validHost,
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("user123")]
    [InlineData("test-user")]
    [InlineData("guest")]
    public void Validate_WithValidUserNames_ShouldNotThrow(string validUserName)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = validUserName,
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("complex!@#$%")]
    [InlineData("guest")]
    [InlineData("p")]
    public void Validate_WithValidPasswords_ShouldNotThrow(string validPassword)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = validPassword,
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithAllRequiredProperties_ShouldNotThrow()
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(65536)]
    [InlineData(100000)]
    [InlineData(int.MaxValue)]
    public void Validate_WithPortAboveValidRange_ShouldNotThrow(int highPort)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = highPort,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Validate_WithInvalidVirtualHost_ShouldThrowArgumentException(string invalidVirtualHost)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = invalidVirtualHost,
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("VirtualHost is required.*")
           .And.ParamName.Should().Be("VirtualHost");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_WithInvalidPackageId_ShouldThrowArgumentException(int invalidPackageId)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = invalidPackageId,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("PackageId must be a positive integer.*")
           .And.ParamName.Should().Be("PackageId");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(5)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidRequestedHeartbeatSeconds_ShouldThrowArgumentException(int invalidHeartbeat)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = invalidHeartbeat,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("RequestedHeartbeatSeconds must be a positive integer - Larger then 10.*")
           .And.ParamName.Should().Be("RequestedHeartbeatSeconds");
    }

    [Theory]
    [InlineData(15)]
    [InlineData(10)]
    [InlineData(5)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidNetworkRecoveryInterval_ShouldThrowArgumentException(int invalidInterval)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 20,
            NetworkRecoveryInterval = invalidInterval
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().Throw<ArgumentException>()
           .WithMessage("NetworkRecoveryInterval must be a positive integer.*")
           .And.ParamName.Should().Be("NetworkRecoveryInterval");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/production")]
    [InlineData("/test")]
    [InlineData("/custom-vhost")]
    public void Validate_WithValidVirtualHosts_ShouldNotThrow(string validVirtualHost)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = validVirtualHost,
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void Validate_WithValidPackageIds_ShouldNotThrow(int validPackageId)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = validPackageId,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(11)]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(int.MaxValue)]
    public void Validate_WithValidRequestedHeartbeatSeconds_ShouldNotThrow(int validHeartbeat)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = validHeartbeat,
            NetworkRecoveryInterval = 20
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(16)]
    [InlineData(20)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(int.MaxValue)]
    public void Validate_WithValidNetworkRecoveryInterval_ShouldNotThrow(int validInterval)
    {
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "guest",
            Password = "guest",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = validInterval
        };

        var act = () => RmqConnectionSettingsValidator.Validate(settings);

        act.Should().NotThrow();
    }
}
