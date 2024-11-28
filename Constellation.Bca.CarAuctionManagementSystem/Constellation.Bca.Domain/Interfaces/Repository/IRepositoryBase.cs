

namespace Constellation.Bca.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        public Task<bool> AddEntityAsync(T entity, CancellationToken cancellationToken);
    }
}
