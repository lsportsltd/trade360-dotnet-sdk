using System;
using System.Collections.Generic;
using System.Reflection;
using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Feed.RabbitMQ.Resolvers
{
    public class MessageProcessorContainer<TFlow> : IMessageProcessorContainer
    {
        private readonly Dictionary<int, IMessageProcessor> _messageProcessorsInfo;

        public MessageProcessorContainer(IEnumerable<IMessageProcessor> messageProcessors)
        {
            _messageProcessorsInfo = new Dictionary<int, IMessageProcessor>();
            
            foreach (var messageProcessor in messageProcessors)
            {
                if (messageProcessor.GetTypeOfTFlow() == typeof(TFlow))
                {
                    var type = messageProcessor.GetTypeOfTType();
                    var id = type.GetCustomAttribute<Trade360EntityAttribute>().EntityKey;

                    if (!_messageProcessorsInfo.TryAdd(id, messageProcessor))
                    {
                        throw new ArgumentException("Failed to add resolver as the resolver already registered.");
                    }
                }
            }
        }
        
        public IMessageProcessor GetMessageProcessor(int messageType)
        {
            return _messageProcessorsInfo[messageType];
        }
    }
}