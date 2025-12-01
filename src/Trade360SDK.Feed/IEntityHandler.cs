using System.Threading.Tasks;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed
{
    public interface IEntityHandler<in TType, TFlow> where TFlow : IFlow where TType : class
    {
        Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, TType? entity);
    }
}
