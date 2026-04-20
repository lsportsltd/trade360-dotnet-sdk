using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ;
using Trade360SDK.Feed.RabbitMQ.Validators;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Validators;

public class RmqConnectionSettingsQueueValidationTests
{
    private static RmqConnectionSettings ValidBaseline()
    {
        return new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 123,
            UserName = "u",
            Password = "p",
            RequestedHeartbeatSeconds = 15,
            NetworkRecoveryInterval = 20
        };
    }

    [Fact]
    public void Validate_CustomQueueNameTooLong_ThrowsArgumentException()
    {
        var s = ValidBaseline();
        s.CustomQueueName = new string('a', RabbitMqFeed.ConsumeQueueNameMaxLength + 1);

        var act = () => RmqConnectionSettingsValidator.Validate(s);

        act.Should().Throw<ArgumentException>().WithMessage("*CustomQueueName*");
    }

    [Fact]
    public void Validate_ZeroPackageIdWithCustomQueueName_DoesNotThrow()
    {
        var s = ValidBaseline();
        s.PackageId = 0;
        s.CustomQueueName = "my-queue";

        var act = () => RmqConnectionSettingsValidator.Validate(s);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_ZeroPackageIdWithoutCustomQueueName_ThrowsArgumentException()
    {
        var s = ValidBaseline();
        s.PackageId = 0;

        var act = () => RmqConnectionSettingsValidator.Validate(s);

        act.Should().Throw<ArgumentException>().WithMessage("*PackageId is required when CustomQueueName*");
    }
}
