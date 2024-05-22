using System.Threading.Tasks;

namespace Trade360SDK.Feed
{
    public interface IEntityHandler<T>
    {
        Task ProcessAsync(T entity);
    }
}
