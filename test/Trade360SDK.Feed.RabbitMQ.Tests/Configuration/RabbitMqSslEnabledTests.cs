using RabbitMQ.Client;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Configuration;

public class RabbitMqSslEnabledTests
{
    [Fact]
    public void Apply_WhenSslDisabled_DoesNotEnableSsl()
    {
        var factory = new ConnectionFactory();
        var settings = new RmqConnectionSettings { SslEnabled = false };

        RabbitMqSslConfigurator.Apply(factory, settings);

        Assert.False(factory.Ssl.Enabled);
    }

    [Fact]
    public void Apply_WhenSslEnabled_SetsEnabledAndServerNameToHost()
    {
        var factory = new ConnectionFactory();
        var settings = new RmqConnectionSettings
        {
            SslEnabled = true,
            Host = "rmq.example.com"
        };

        RabbitMqSslConfigurator.Apply(factory, settings);

        Assert.True(factory.Ssl.Enabled);
        Assert.Equal("rmq.example.com", factory.Ssl.ServerName);
    }
}
