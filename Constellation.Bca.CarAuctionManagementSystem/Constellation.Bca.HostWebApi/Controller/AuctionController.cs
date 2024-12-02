using Constellation.Bca.Application.Commands;
using Constellation.Bca.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Constellation.Bca.HostWebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController(IMediator mediator) : ControllerBase
    {
        [HttpPost("startauction")]
        public async Task<IActionResult> StartBidAsync([FromBody]AuctionDto auctionDto, CancellationToken cancellationToken)
        {
           var notification =  await mediator.Send(new StartAuctionCommand() { Auction = auctionDto });
            if (!notification.IsValid())
                BadRequest(notification);

            return Ok(notification);
        }

        [HttpPost("stopauction")]
        public async Task<IActionResult> StopBidAsync(int auctionId, DateTime closed_at, CancellationToken cancellationToken)
        {
            var notification = await mediator.Send(new StopAuctionCommand() { AuctionId = auctionId, Closedd_At = closed_at });
            if (!notification.IsValid())
                BadRequest(notification);

            return Ok(notification);
        }
    }
}
