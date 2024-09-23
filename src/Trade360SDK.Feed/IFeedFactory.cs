using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed
{
    public interface IFeedFactory
    {
        IFeed CreateFeed(RmqConnectionSettings settings, Trade360Settings trade360Settings, FlowType flowType);
    }
}
