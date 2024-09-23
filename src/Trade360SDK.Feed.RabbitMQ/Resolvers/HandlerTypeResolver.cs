using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.RabbitMQ.Resolvers
{
    public class HandlerTypeResolver<T> : IHandlerTypeResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<int, Type> _messageUpdateTypes;

        public HandlerTypeResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _messageUpdateTypes = Trade360AttributeHelper.GetAttributes()
                .ToDictionary(t => t.GetCustomAttribute<Trade360EntityAttribute>().EntityKey, t => t);
        }

        public IHandler GetHandler(Type type)
        {
            var handlerInterfaceType = typeof(IEntityHandler<,>).MakeGenericType(type, typeof(T));
            IHandler handler = _serviceProvider.GetRequiredService(handlerInterfaceType) as IHandler;
            
            if (handler == null)
            {
                throw new ArgumentException("No handler found for type", nameof(type));
            }

            return handler;
        }

        public Type ResolveMessageType(int entityType)
        {
            _messageUpdateTypes.TryGetValue(entityType, out var messageType);
            if (messageType == null)
            {
                throw new ArgumentException($"Not found Message type for type {entityType}");
            }

            return messageType;
        }
    }
}