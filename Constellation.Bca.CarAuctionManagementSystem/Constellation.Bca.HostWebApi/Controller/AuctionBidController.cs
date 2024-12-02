using Constellation.Bca.Application.Commands;
using Constellation.Bca.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Constellation.Bca.HostWebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionBidController(IMediator mediator) : ControllerBase
    {
        [HttpPost("placeabid")]
        public async Task<IActionResult> PlaceAnAuctionBid(AuctionBidDto auctionBidDto, CancellationToken cancellationToken)
        {
            var notification = await mediator.Send(new PlaceAuctionBidCommand() { auctionBid =  auctionBidDto });
            if (!notification.IsValid())
                return BadRequest(notification);

            return Ok(notification);

        }
    }
}
