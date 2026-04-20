using RabbitMQ.Client;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed.RabbitMQ
{
    internal static class RabbitMqSslConfigurator
    {
        internal static void Apply(ConnectionFactory factory, RmqConnectionSettings settings)
        {
            if (!settings.SslEnabled)
                return;

            factory.Ssl.Enabled = true;
            factory.Ssl.ServerName = settings.Host!.Trim();
        }
    }
}
