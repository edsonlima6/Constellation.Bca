
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Constellation.Bca.Infrastructure.Repositories
{
    public class VehicleRepository(DatabaseContext databaseContext, IMemoryCache memoryCache) : RepositoryBase<Vehicle>(databaseContext), IVehicleRepository 
    {
        private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        public async Task<bool> IsUniqueIdentifierInUseAsync(string uniqueIdentifier, CancellationToken cancellationToken) => await _dbSet.AnyAsync(x => x.UniqueIdentifier == uniqueIdentifier, cancellationToken);

        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken) => await memoryCache.GetOrCreateAsync("AllVehicle", x => _dbSet.AsNoTracking().ToListAsync(cancellationToken));
    }
}
