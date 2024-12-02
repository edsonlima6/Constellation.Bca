
using Constellation.Bca.Domain.Entites;

namespace Constellation.Bca.Domain.Interfaces.Repository
{
    public interface IAuctionRepository : IRepositoryBase<Auction>
    {
        Task<bool> HasAnyActiveAuctionAsync(int vehicleId, CancellationToken cancellationToken);
        Task<Auction?> GetAuctionByIdAsync(int auctionId, CancellationToken cancellationToken);

        Task<bool> StopAuctionAsync(Auction entity, CancellationToken cancellationToken);
    }
}
