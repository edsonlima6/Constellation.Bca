using Constellation.Bca.Application.Commands;
using Constellation.Bca.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Constellation.Bca.HostWebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IMediator mediator) : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> GetVehicleFiltered([FromQuery]AllVehicleQueryCommand allVehicleQuery)
        {
            var queryList = await mediator.Send(allVehicleQuery);
            return Ok(queryList);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle(VehicleDto createVehicleCommand)
        {
            var result = await mediator.Send(new CreateVehicleCommand() { Vehicle = createVehicleCommand });
            if (!result.IsValid())
                return BadRequest(result);

            return CreatedAtAction(nameof(AddVehicle), result);
        }
    }
}
