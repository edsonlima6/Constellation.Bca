
using Constellation.Bca.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Constellation.Bca.Domain.Entites
{
    [Table("AuctionBid")]
    public class AuctionBid
    {
        [Key]
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int VehicleId { get; set; }
        public decimal Price { get; set; }
        public DateTime Created_at { get; set; }
        public BidStatusEnum Status { get; set; }
        public string UserName { get; set; }

        [ForeignKey("AuctionBidFKAuctionId")]
        public Auction Auction { get; set; }

        [ForeignKey("AuctionBidFKVehicleId")]
        public Vehicle Vehicle { get; set; }
    }
}
