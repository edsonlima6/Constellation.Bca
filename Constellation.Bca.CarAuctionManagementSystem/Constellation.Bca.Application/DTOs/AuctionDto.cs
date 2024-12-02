
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.DTOs
{
    public class AuctionDto
    {
        [Required(ErrorMessage = "Vehicle must be informed")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Created date must be informed")]
        public DateTime Created_At { get; set; }
        public bool IsActive { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User name must be informed")]
        public string UserName { get; set; }
    }
}
