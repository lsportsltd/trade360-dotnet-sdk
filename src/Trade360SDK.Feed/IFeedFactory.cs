using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed
{
    public interface IFeedFactory
    {
        public IFeed CreateFeed(RmqConnectionSettings settings);
    }
}
