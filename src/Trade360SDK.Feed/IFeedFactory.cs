namespace Trade360SDK.Feed.RabbitMQ
{
    public interface IFeedFactory
    {
        public IFeed CreateFeed(RmqConnectionSettings settings);
    }
}
