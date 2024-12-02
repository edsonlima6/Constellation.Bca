
using Constellation.Bca.Application.Commands;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Services;
using MapsterMapper;
using MediatR;

namespace Constellation.Bca.Application.Handlers.Command
{
    public class AuctionServiceHandler(IAuctionService auctionService, IMapper mapper) 
        : IRequestHandler<StartAuctionCommand, NotificationDomain>, 
          IRequestHandler<StopAuctionCommand, NotificationDomain>
    {
        public async Task<NotificationDomain> Handle(StartAuctionCommand request, CancellationToken cancellationToken)
        {
            if (request.Auction is null)
                throw new ArgumentNullException(nameof(request.Auction));

            var auction = mapper.Map<Auction>(request.Auction);
            return await auctionService.StartAuctionAsync(auction, cancellationToken);
        }

        public async Task<NotificationDomain> Handle(StopAuctionCommand request, CancellationToken cancellationToken) => 
            await auctionService.StopAuctionAsync(request.AuctionId, request.Closedd_At, cancellationToken);
    }
}
