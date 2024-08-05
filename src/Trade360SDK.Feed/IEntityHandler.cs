using System.Threading.Tasks;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed
{
    public interface IEntityHandler<in T>
    {
        Task ProcessAsync(T entity, MessageHeader header);
    }
}
