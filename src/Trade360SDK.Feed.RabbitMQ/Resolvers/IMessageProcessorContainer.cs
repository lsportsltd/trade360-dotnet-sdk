namespace Trade360SDK.Feed.RabbitMQ.Resolvers
{
    public interface IMessageProcessorContainer
    {
        IMessageProcessor GetMessageProcessor(int messageType);
    }
}