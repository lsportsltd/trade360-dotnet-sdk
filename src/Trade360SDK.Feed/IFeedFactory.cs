using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed
{
    public interface IFeedFactory
    {
        IFeed CreateFeed(RmqConnectionSettings settings, FlowType flowType);
    }
}
