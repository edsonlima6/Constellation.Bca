
using Constellation.Bca.Application.DTOs.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.DTOs
{
    [DisplayName("vehicle")]
    public class VehicleDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public int NumberOfDoors { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Identifier is required.")]
        [Length(17, 17, ErrorMessage = "The identifier must have 17 characters")]
        public string UniqueIdentifier { get; set; }

        [Required(ErrorMessage = "Registration year is required.")]
        [Range(1900, 2030)]
        public int RegistrationYear { get; set; }
        public decimal StartingBid { get; set; }
        public int NumberOfSeats { get; set; }
        public double LoadCapacity { get; set; }

        [EnumDataType(typeof(VehicleTypeEnumDto), ErrorMessage = $"Vehicle type was missing, the expected types are ({nameof(VehicleTypeEnumDto.Truck)}, {nameof(VehicleTypeEnumDto.Hatchback)}, {nameof(VehicleTypeEnumDto.SUV)}, {nameof(VehicleTypeEnumDto.Sedan)})")]
        public VehicleTypeEnumDto VehicleType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Manufacturer is required.")]
        public string Manufacturer { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User name must be informed")]
        public string UserName { get; set; }

        public bool IsActive { get; set; }
    }
}
