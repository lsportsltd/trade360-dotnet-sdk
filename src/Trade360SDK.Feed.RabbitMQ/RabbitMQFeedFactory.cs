using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed.RabbitMQ
{
    public class RabbitMqFeedFactory : IFeedFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqFeedFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFeed CreateFeed(RmqConnectionSettings settings)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            return new RabbitMqFeed(settings, loggerFactory);
        }
    }
}
