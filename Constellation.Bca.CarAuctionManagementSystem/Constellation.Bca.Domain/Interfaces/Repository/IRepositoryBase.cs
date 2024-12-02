

namespace Constellation.Bca.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<bool> AddEntityAsync(T entity, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetAllAsync(string keyName, CancellationToken cancellationToken);
    }
}
