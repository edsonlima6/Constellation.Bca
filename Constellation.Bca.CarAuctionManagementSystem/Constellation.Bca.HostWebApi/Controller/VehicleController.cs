using Constellation.Bca.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Constellation.Bca.HostWebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IMediator mediator) : ControllerBase
    {
        [HttpGet()]
        public IActionResult GetVehicleById()
        {
            var ano = new { auth = "olá" };
            return Ok(ano);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle(CreateVehicleCommand createVehicleCommand)
        {
            var result = await mediator.Send(createVehicleCommand);
            if (result.IsValid())
                return CreatedAtAction(nameof(AddVehicle), result);

            return BadRequest(result);
        }
    }
}
