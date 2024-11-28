
using Constellation.Bca.Domain.Enums;

namespace Constellation.Bca.Domain.Entites
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int NumberOfDoors { get; set; }
        public int RegistrationYear { get; set; }
        public decimal StartingBid { get; set; }
        public int NumberOfSeats { get; set; }
        public double LoadCapacity { get; set; }
        public VehicleType VehicleType { get; set; }

        public required string UniqueIdentifier { get; set; }
        public required string Manufacturer { get; set; }
        public required string Model { get; set; }
    }
}
