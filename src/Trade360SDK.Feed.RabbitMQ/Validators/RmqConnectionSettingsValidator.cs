using System;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ;

namespace Trade360SDK.Feed.RabbitMQ.Validators
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

            if (settings.PackageId < 0)
                throw new ArgumentException("PackageId cannot be negative.", nameof(settings.PackageId));

            if (settings.PackageId == 0 && string.IsNullOrWhiteSpace(settings.CustomQueueName))
                throw new ArgumentException(
                    "PackageId is required when CustomQueueName is not set, or set CustomQueueName when PackageId is omitted.",
                    nameof(settings.PackageId));

            if (string.IsNullOrWhiteSpace(settings.UserName))
                throw new ArgumentException("UserName is required.", nameof(settings.UserName));

            if (string.IsNullOrWhiteSpace(settings.Password))
                throw new ArgumentException("Password is required.", nameof(settings.Password));

            if (settings.RequestedHeartbeatSeconds <= 10)
                throw new ArgumentException("RequestedHeartbeatSeconds must be a positive integer - Larger then 10.", nameof(settings.RequestedHeartbeatSeconds));

            if (!string.IsNullOrWhiteSpace(settings.CustomQueueName) && settings.CustomQueueName.Trim().Length > RabbitMqFeed.ConsumeQueueNameMaxLength)
                throw new ArgumentException(
                    $"CustomQueueName must be at most {RabbitMqFeed.ConsumeQueueNameMaxLength} characters.",
                    nameof(settings.CustomQueueName));

            var queueName = RabbitMqFeed.ResolveConsumeQueueName(settings);
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("The effective queue name is empty. Check CustomQueueName and PackageId.", nameof(settings.CustomQueueName));

            if (queueName.Length > RabbitMqFeed.ConsumeQueueNameMaxLength)
            {
                throw new ArgumentException(
                    $"The effective queue name exceeds {RabbitMqFeed.ConsumeQueueNameMaxLength} characters. Shorten CustomQueueName.",
                    nameof(settings.CustomQueueName));
            }
        }
    }
}
