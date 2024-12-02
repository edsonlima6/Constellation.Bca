
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;

namespace Constellation.Bca.Domain.Interfaces.Services
{
    public interface IAuctionBidService
    {
        Task<NotificationDomain> PlaceABidAsync(AuctionBid auctionBid, CancellationToken cancellationToken);
    }
}
