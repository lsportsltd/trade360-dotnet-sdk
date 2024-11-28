using System;
using System.Threading.Tasks;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.RabbitMQ.Resolvers
{
    public interface IMessageProcessor
    {
        Task ProcessAsync(Type type, MessageHeader? header, string? body);
        Type GetTypeOfTType();
        Type GetTypeOfTFlow();
    }

}