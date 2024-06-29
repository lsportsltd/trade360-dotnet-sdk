using System;

namespace Trade360SDK.Feed.RabbitMQ
{
    public static class RmqConnectionSettingsValidator
    {
        public static void Validate(RmqConnectionSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Host))
                throw new ArgumentException("Host is required.", nameof(settings.Host));

            if (settings.Port <= 0)
                throw new ArgumentException("Port must be a positive integer.", nameof(settings.Port));

            if (string.IsNullOrWhiteSpace(settings.VirtualHost))
                throw new ArgumentException("VirtualHost is required.", nameof(settings.VirtualHost));

            if (settings.PackageId <= 0)
                throw new ArgumentException("PackageId must be a positive integer.", nameof(settings.PackageId));

            if (string.IsNullOrWhiteSpace(settings.UserName))
                throw new ArgumentException("UserName is required.", nameof(settings.UserName));

            if (string.IsNullOrWhiteSpace(settings.Password))
                throw new ArgumentException("Password is required.", nameof(settings.Password));

            if (settings.PrefetchCount < 0)
                throw new ArgumentException("PrefetchCount must be a positive integer.", nameof(settings.PrefetchCount));

            if (settings.RequestedHeartbeatSeconds <= 10)
                throw new ArgumentException("RequestedHeartbeatSeconds must be a positive integer - Larger then 10.", nameof(settings.RequestedHeartbeatSeconds));

            if (settings.NetworkRecoveryInterval <= 15)
                throw new ArgumentException("NetworkRecoveryInterval must be a positive integer.", nameof(settings.NetworkRecoveryInterval));
        }
    }
}
