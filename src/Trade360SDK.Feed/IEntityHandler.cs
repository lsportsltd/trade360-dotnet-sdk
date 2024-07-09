using System.Threading.Tasks;

namespace Trade360SDK.Feed
{
    public interface IEntityHandler<in T>
    {
        Task ProcessAsync(T entity);
    }
}
