namespace Trade360SDK.Feed.RabbitMQ
{
    public interface IRabbitMQFeedFactory
    {
        public IRabbitMQFeed CreateFeed(RmqConnectionSettings settings);
    }
}
