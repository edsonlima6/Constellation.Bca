
using Constellation.Bca.Application.DTOs;
using Constellation.Bca.Application.DTOs.Enums;
using Constellation.Bca.Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.Commands
{
    public class AllVehicleQueryCommand : IRequest<QueryFilterResultDto>
    {
        public ColumnsDto? Field { get; set; }
        public VehicleOperatorDto? Operator { get; set; }
        public string? Value { get; set; }
        public LogicDto? Logic { get; set; }
        public List<AllVehicleQueryCommand>? QueryFilters { get; set; }
    }
}
