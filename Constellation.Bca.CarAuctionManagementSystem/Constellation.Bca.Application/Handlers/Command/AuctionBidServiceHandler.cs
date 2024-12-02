using Constellation.Bca.Application.Commands;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Services;
using MapsterMapper;
using MediatR;

namespace Constellation.Bca.Application.Handlers.Command
{
    public class AuctionBidServiceHandler(IAuctionBidService auctionBidService, IMapper mapper) : IRequestHandler<PlaceAuctionBidCommand, NotificationDomain>
    {
        public async Task<NotificationDomain> Handle(PlaceAuctionBidCommand request, CancellationToken cancellationToken)
        {
            var auctionBid = mapper.Map<AuctionBid>(request.auctionBid);
            return await auctionBidService.PlaceABidAsync(auctionBid, cancellationToken);
        }
    }
}
