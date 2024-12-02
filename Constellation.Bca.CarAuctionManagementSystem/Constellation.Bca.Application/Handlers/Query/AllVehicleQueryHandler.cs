using Constellation.Bca.Application.Commands;
using Constellation.Bca.Application.DTOs;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Services;
using Constellation.Bca.Domain.ValueObjects;
using MapsterMapper;
using MediatR;

namespace Constellation.Bca.Application.Handlers.Query
{
    public class AllVehicleQueryHandler(IVehicleService vehicleService, IMapper mapper) : IRequestHandler<AllVehicleQueryCommand, QueryFilterResultDto>
    {
        private readonly IVehicleService _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<QueryFilterResultDto> Handle(AllVehicleQueryCommand request, CancellationToken cancellationToken)
        {
            var queryResul = new QueryFilterResultDto();
            var filteredVehicles = await _vehicleService.GetAllAsync(mapper.Map<QueryFilter>(request), cancellationToken);
            queryResul.Vehicles = mapper.Map<List<VehicleDto>>(filteredVehicles);
            return queryResul;
        }
    }
}
