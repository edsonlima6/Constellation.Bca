
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Interfaces.Services;
using FluentValidation;
using System.Net;

namespace Constellation.Bca.Domain.Services
{
    public class AuctionService(IAuctionRepository auctionRepository, 
                                IValidator<Auction> auctionValidator,
                                IVehicleRepository vehicleRepository) : ServiceBase, IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));

        public async Task<NotificationDomain> StopAuctionAsync(int id, DateTime closedd_At, CancellationToken cancellationToken)
        {
            var notification = new NotificationDomain() { StatusCode = HttpStatusCode.OK };
            if (id <= 0)
                return GetNotificationDomain(HttpStatusCode.BadRequest, [ConstantProvider.GetInvalidAuctionIdMessage()], id);

            if (closedd_At.Date < DateTime.Now.Date)
                return GetNotificationDomain(HttpStatusCode.BadGateway, [ConstantProvider.GetClosed_atMustBeGreanterThanCurrentDayMessage()], closedd_At);

            var auctionFromDb = await auctionRepository.GetAuctionByIdAsync(id, cancellationToken);
            if (auctionFromDb is null)
                return GetNotificationDomain(HttpStatusCode.NotFound, [ConstantProvider.GetNotFoundAuctionIdMessage()], closedd_At);

            auctionFromDb.IsActive = false;
            await auctionRepository.StopAuctionAsync(auctionFromDb, cancellationToken);

            return notification;
        }

        public async Task<NotificationDomain> StartAuctionAsync(Auction auction, CancellationToken cancellationToken)
        {
            var notification = new NotificationDomain() { StatusCode = HttpStatusCode.Created, Data = auction };
            var result = auctionValidator.Validate(auction);
            if (!result.IsValid)
                return GetNotificationDomain(HttpStatusCode.BadRequest, result.Errors.Select(c => c.ErrorMessage).ToList(), auction);

            if (!await _vehicleRepository.ExistsById(auction.VehicleId))
                return GetNotificationDomain(HttpStatusCode.BadGateway, [ConstantProvider.GetNonExistingVehicleMessage()], auction);

            if (await _auctionRepository.HasAnyActiveAuctionAsync(auction.VehicleId, cancellationToken))
                return GetNotificationDomain(HttpStatusCode.BadGateway, [ConstantProvider.GetExistingActiveAuctionMessage()], auction);

            if (string.IsNullOrEmpty(auction.UserName))
                return GetNotificationDomain(HttpStatusCode.BadGateway, [ConstantProvider.GetUserDoesNotExistsMessage()], auction);

            auction.IsActive = true;
            var entity = await _auctionRepository.AddEntityAsync(auction, cancellationToken);
            if (!entity)
                return GetNotificationDomain(HttpStatusCode.BadRequest, ["Error to add the auction"], auction);

            
            return notification;
        }
    }
}
