using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.RabbitMQ.Resolvers
{
    public class MessageProcessor<TType, TFlow> : IMessageProcessor where TFlow : IFlow where TType : class
    {
        private readonly IServiceProvider _serviceProvider;
       
        private readonly ILogger<MessageProcessor<TType, TFlow>> _logger;

        public MessageProcessor(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MessageProcessor<TType, TFlow>>();
            _serviceProvider = serviceProvider;
        }
        
        

        public async Task ProcessAsync(Type type, MessageHeader? header, string? body)
        {
            var handler = _serviceProvider.GetRequiredService<IEntityHandler<TType, TFlow>>();
            
            if (handler == null)
            {
                throw new ArgumentException("No handler found for type", nameof(type));
            }

            TType? message;

            try
            {
                message = JsonSerializer.Deserialize<TType>(body);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(body))
                {
                    _logger.LogWarning($"Failed to deserialize message body {body}.");
                }
                message = null;
            }
            
            await handler.ProcessAsync(header, message);
        }
        
        public Type GetTypeOfTType()
        {
            return typeof(TType);
        }

        public Type GetTypeOfTFlow()
        {
            return typeof(TFlow);
        }
    }
}