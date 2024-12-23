using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ
{
    public class RabbitMqFeedFactory : IFeedFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqFeedFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFeed CreateFeed(RmqConnectionSettings settings, Trade360Settings trade360Settings, FlowType flowType)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var customerApiFactory = _serviceProvider.GetRequiredService<ICustomersApiFactory>();
            IMessageProcessorContainer messageProcessorContainer = null;
            if (flowType == FlowType.InPlay)
            {
                messageProcessorContainer = _serviceProvider.GetRequiredService<MessageProcessorContainer<InPlay>>();
            } 
            else if (flowType == FlowType.PreMatch)
            {
                messageProcessorContainer = _serviceProvider.GetRequiredService<MessageProcessorContainer<PreMatch>>();
            }
            return new RabbitMqFeed(settings, trade360Settings, messageProcessorContainer, flowType, loggerFactory, customerApiFactory);
        }
    }
}
