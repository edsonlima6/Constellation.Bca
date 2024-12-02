
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Constellation.Bca.Domain.Entites
{
    [Table("Auction")]
    public class Auction
    {
        [Key]
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Closedd_At { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }

        [ForeignKey("AuctionFKVehicleBidId")]
        public Vehicle Vehicle { get; set; }

        [ForeignKey("AuctionFKAuctionBidId")]
        public IEnumerable<AuctionBid> Bids { get; set; }

    }
}
