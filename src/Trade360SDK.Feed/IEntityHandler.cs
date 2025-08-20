using System;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed
{
    public interface IEntityHandler<in TType, TFlow> where TFlow : IFlow where TType : class
    {
        Task ProcessAsync(RabbitMessageProperties? rabbitHeaders, MessageHeader? header, TType? entity);
    }
}
