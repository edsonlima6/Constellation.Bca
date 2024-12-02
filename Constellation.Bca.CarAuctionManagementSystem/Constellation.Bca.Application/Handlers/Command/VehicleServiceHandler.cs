using Constellation.Bca.Application.Commands;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Services;
using MapsterMapper;
using MediatR;

namespace Constellation.Bca.Application.Handlers.Command
{
    public class VehicleServiceHandler(IVehicleService vehicleService, IMapper mapper) : IRequestHandler<CreateVehicleCommand, NotificationDomain>
    {
        private readonly IVehicleService _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        public async Task<NotificationDomain> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<Vehicle>(request.Vehicle);
            return await _vehicleService.AddVehicleAsync(entity, cancellationToken);
        }
    }
}
