

using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Constellation.Bca.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly DatabaseContext _databaseContext;
        protected DbSet<T> _dbSet;
        public RepositoryBase(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
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
    }
}
