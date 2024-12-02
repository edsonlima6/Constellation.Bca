
using Constellation.Bca.Application.DTOs.Enums;
using Constellation.Bca.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.DTOs
{
    public class AuctionBidDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Auction Id must be informed")]
        public int AuctionId { get; set; }

        [Required(ErrorMessage = "Vehicle Id must be informed")]
        public int VehicleId { get; set; }
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Created date must be informed")]
        public DateTime Created_at { get; set; }
        public AuctionBidStatusEnumDto Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User name must be informed")]
        public string UserName { get; set; }
    }
}
