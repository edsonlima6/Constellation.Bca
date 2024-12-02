
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Constellation.Bca.Infrastructure.Repositories
{
    public class AuctionRepository(DatabaseContext databaseContext,
                                   IMemoryCache memoryCache) : RepositoryBase<Auction>(databaseContext, memoryCache), IAuctionRepository
    {
        public async Task<Auction?> GetAuctionByIdAsync(int auctionId, CancellationToken cancellationToken) => await _dbSet.FirstOrDefaultAsync(x => x.Id == auctionId, cancellationToken);

        public async Task<bool> HasAnyActiveAuctionAsync(int vehicleId, CancellationToken cancellationToken) 
            => await _dbSet.AnyAsync(x => x.VehicleId == vehicleId && x.IsActive, cancellationToken);

        public async Task<bool> StopAuctionAsync(Auction entity, CancellationToken cancellationToken)
        {
            _dbSet.Entry(entity).Property(x => x.Closedd_At).IsModified = true;
            _dbSet.Entry(entity).Property(x => x.IsActive).IsModified = true;
            var isEntityUpdate = await _databaseContext.SaveChangesAsync();

            return isEntityUpdate > 0;
        }
    }
}
