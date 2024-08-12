using System.Threading.Tasks;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.RabbitMQ.Handlers
{
    internal interface IMessageTypeHandler
    {
        Task ProcessAsync(string? body, MessageHeader header);
    }
}