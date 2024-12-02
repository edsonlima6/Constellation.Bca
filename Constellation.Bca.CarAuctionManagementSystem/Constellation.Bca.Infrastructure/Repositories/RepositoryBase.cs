

using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Constellation.Bca.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly DatabaseContext _databaseContext;
        protected readonly IMemoryCache _memoryCache;
        protected DbSet<T> _dbSet;
        public RepositoryBase(DatabaseContext databaseContext, IMemoryCache memoryCache)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _dbSet = _databaseContext.Set<T>();
            _dbSet = _databaseContext.Set<T>();
        }

        public async Task<bool> AddEntityAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity is null)
                return false;

            await _dbSet.AddAsync(entity, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string keyName, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreateAsync(keyName, x => _databaseContext.Set<T>().AsNoTracking().ToListAsync());
        }
    }
}
