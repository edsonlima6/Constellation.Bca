using Constellation.Bca.Domain.Entites;

namespace Constellation.Bca.Domain.Interfaces.Repository
{
    public interface IAuctionBidRepository : IRepositoryBase<AuctionBid>
    {
        Task<AuctionBid?> GetLastAuctionBidAsync(int auctionId, int vehicleId, CancellationToken cancellationToken);
    }
}
