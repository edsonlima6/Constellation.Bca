
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;

namespace Constellation.Bca.Domain.Interfaces.Services
{
    public interface IAuctionService
    {
        Task<NotificationDomain> StartAuctionAsync(Auction auction, CancellationToken cancellationToken);
        Task<NotificationDomain> StopAuctionAsync(int id, DateTime closedd_At, CancellationToken cancellationToken);
    }
}
