using System;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed
{
    public interface IEntityHandler<in TEntity, TFlow> : IHandler where TFlow : IFlow 
    {
        Task ProcessAsync(TEntity entity);
    }
}
