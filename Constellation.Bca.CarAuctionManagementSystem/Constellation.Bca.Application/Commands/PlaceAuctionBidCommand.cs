
using Constellation.Bca.Application.DTOs;
using Constellation.Bca.Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.Commands
{
    public class PlaceAuctionBidCommand : IRequest<NotificationDomain>
    {
        [Required(ErrorMessage = "Auction Bid must be send")]
        public AuctionBidDto auctionBid {  get; set; }
    }
}
