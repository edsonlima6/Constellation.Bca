
using Constellation.Bca.Application.DTOs;
using Constellation.Bca.Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.Commands
{
    public class CreateVehicleCommand : IRequest<NotificationDomain>
    {
        [Required(ErrorMessage = "The Vehicle is required")]
        public VehicleDto Vehicle { get; set; }
    }
}
