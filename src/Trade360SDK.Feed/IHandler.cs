using System.Threading.Tasks;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed
{
    public interface IHandler
    {
        Task ProcessAsync(object entity, MessageHeader header);
    }
}