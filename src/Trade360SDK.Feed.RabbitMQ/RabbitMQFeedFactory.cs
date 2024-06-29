using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Trade360SDK.Feed.RabbitMQ
{
    public class RabbitMQFeedFactory : IRabbitMQFeedFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public RabbitMQFeedFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public IRabbitMQFeed CreateFeed(RmqConnectionSettings settings)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            return new RabbitMQFeed(settings, loggerFactory);
        }
    }
}
