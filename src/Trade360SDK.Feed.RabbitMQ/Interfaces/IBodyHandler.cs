using System.Threading.Tasks;

namespace Trade360SDK.Feed.RabbitMQ.Interfaces
{
    internal interface IBodyHandler
    {
        Task ProcessAsync(string? body);
    }
}
