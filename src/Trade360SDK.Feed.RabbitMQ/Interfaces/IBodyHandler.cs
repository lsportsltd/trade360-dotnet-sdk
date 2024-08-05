using System.Threading.Tasks;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.RabbitMQ.Interfaces
{
    internal interface IBodyHandler
    {
        Task ProcessAsync(string? body, MessageHeader header);
    }
}