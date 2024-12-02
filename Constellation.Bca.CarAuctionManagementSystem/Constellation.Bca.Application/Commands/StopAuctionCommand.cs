using Constellation.Bca.Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.Commands
{
    public class StopAuctionCommand : IRequest<NotificationDomain>
    {
        [Required(ErrorMessage = "Auction Id is required")]
        public int AuctionId { get; set; }

        [Required(ErrorMessage = "Closed date is required")]
        public DateTime Closedd_At { get; set; }
    }
}
