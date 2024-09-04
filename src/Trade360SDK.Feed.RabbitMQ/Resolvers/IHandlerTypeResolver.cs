using System;

namespace Trade360SDK.Feed.RabbitMQ.Resolvers
{
    public interface IHandlerTypeResolver
    {
        Type ResolveMessageType(int entityType);

        IHandler GetHandler(Type type);
    }
}