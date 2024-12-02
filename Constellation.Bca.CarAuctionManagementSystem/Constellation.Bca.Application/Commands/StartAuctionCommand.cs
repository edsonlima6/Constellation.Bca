using Constellation.Bca.Application.DTOs;
using Constellation.Bca.Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.Commands
{
    public class StartAuctionCommand : IRequest<NotificationDomain>
    {
        [Required(ErrorMessage = "Auction must be send")]
        public AuctionDto Auction { get; set; }
    }
}
