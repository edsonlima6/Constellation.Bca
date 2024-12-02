
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Interfaces.Services;
using FluentValidation;
using System.Net;

namespace Constellation.Bca.Domain.Services
{
    public class AuctionBidService(IAuctionBidRepository auctionBidRepository, 
                                   IValidator<AuctionBid> auctionBidValidator, 
                                   IAuctionRepository auctionRepository) : ServiceBase, IAuctionBidService
    {

        private readonly IAuctionBidRepository _auctionBidRepository = auctionBidRepository ?? throw new ArgumentNullException(nameof(auctionBidRepository));
        private readonly IValidator<AuctionBid> auctionBidValidator = auctionBidValidator ?? throw new ArgumentNullException(nameof(auctionBidValidator));
        private readonly IAuctionRepository _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        public async Task<NotificationDomain> PlaceABidAsync(AuctionBid auctionBid, CancellationToken cancellationToken)
        {
            var notification = new NotificationDomain() { StatusCode = System.Net.HttpStatusCode.Created };
            var resultValidator = auctionBidValidator.Validate(auctionBid);
            if (!resultValidator.IsValid)
                return GetNotificationDomain(HttpStatusCode.BadRequest, resultValidator.Errors.Select(x => x.ErrorMessage).ToList(), auctionBid);

            if (!await auctionRepository.HasAnyActiveAuctionAsync(auctionBid.VehicleId, cancellationToken))
                return GetNotificationDomain(HttpStatusCode.BadRequest, [ConstantProvider.GetNonExistingActiveAuctionMessage()], auctionBid);

            var auction = await auctionRepository.GetAuctionByIdAsync(auctionBid.AuctionId, cancellationToken);
            if (auction is null)
                return GetNotificationDomain(HttpStatusCode.NotFound, [ConstantProvider.GetNotFoundAuctionIdMessage()], auctionBid);

            var lastBidCreated = await _auctionBidRepository.GetLastAuctionBidAsync(auctionBid.AuctionId, auctionBid.VehicleId, cancellationToken);
            if (lastBidCreated is not null && auctionBid.Price <= lastBidCreated.Price)
                return GetNotificationDomain(HttpStatusCode.BadRequest, [ConstantProvider.GetAmountLesserThanLatestBidAuctiontionMessage(lastBidCreated.Price.ToString())], auctionBid);


            await _auctionBidRepository.AddEntityAsync(auctionBid, cancellationToken);
            return notification;
        }
    }
}
