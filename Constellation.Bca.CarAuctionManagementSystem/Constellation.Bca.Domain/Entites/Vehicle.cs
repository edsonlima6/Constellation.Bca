
using Constellation.Bca.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Constellation.Bca.Domain.Entites
{
    [Table("Vehicle")]
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }
        public int NumberOfDoors { get; set; }
        public int RegistrationYear { get; set; }
        public decimal StartingBid { get; set; }
        public int NumberOfSeats { get; set; }
        public double? LoadCapacity { get; set; }
        public VehicleType VehicleType { get; set; }

        public required string UniqueIdentifier { get; set; }
        public required string Manufacturer { get; set; }
        public required string Model { get; set; }
        public bool IsActive { get; set; }

        public required string UserName { get; set; }

        [ForeignKey("VehicleFKBids")]
        public ICollection<AuctionBid> Bids { get; set; }
    }
}
