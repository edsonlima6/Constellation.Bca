
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Constellation.Bca.Infrastructure.Repositories
{
    public class VehicleRepository(DatabaseContext databaseContext, IMemoryCache memoryCache) 
        : RepositoryBase<Vehicle>(databaseContext, memoryCache), IVehicleRepository
    {
        public async Task<bool> IsUniqueIdentifierInUseAsync(string uniqueIdentifier, CancellationToken cancellationToken) => await _dbSet.AnyAsync(x => x.UniqueIdentifier == uniqueIdentifier, cancellationToken);

        public async Task<IEnumerable<Vehicle>> GetAllAsQueryableAsync(CancellationToken cancellationToken) => await GetAllAsync("AllVehicles", cancellationToken);

        public async Task<bool> ExistsById(int vehicleId) => await _dbSet.AnyAsync(x => x.Id == vehicleId);
    }
}
