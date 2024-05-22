using System.Threading.Tasks;

namespace Trade360SDK.Feed.Interfaces
{
    internal interface IBodyHandler
    {
        Task Process(string? body);
    }
}
