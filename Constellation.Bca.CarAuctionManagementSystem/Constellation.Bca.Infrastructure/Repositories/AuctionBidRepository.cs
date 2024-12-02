
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Constellation.Bca.Infrastructure.Repositories
{
    public class AuctionBidRepository(DatabaseContext databaseContext, IMemoryCache memoryCache) : RepositoryBase<AuctionBid>(databaseContext, memoryCache), IAuctionBidRepository
    {

        public async Task<AuctionBid?> GetLastAuctionBidAsync(int auctionId, int vehicleId, CancellationToken cancellationToken)
        {
            return await _dbSet.OrderByDescending(x => x.Price.ToString())
                               .FirstOrDefaultAsync(x => x.AuctionId == auctionId && x.VehicleId == vehicleId, cancellationToken);
        }
    }
}
