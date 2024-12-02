
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Constellation.Bca.Infrastructure.Context
{
    public class DatabaseContext : DbContext, IDisposable
    {
        public DatabaseContext()
        {
                
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        {
                
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase("CarAuction");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VehicleTypeConfiguration());
            modelBuilder.Entity<Auction>().HasMany(x => x.Bids).WithOne(x => x.Auction).HasForeignKey("AuctionFKAuctionBidId").IsRequired();

            modelBuilder.Entity<AuctionBid>().HasOne(x => x.Auction).WithMany(x => x.Bids).HasForeignKey("AuctionBidFKAuctionId").IsRequired();
            modelBuilder.Entity<AuctionBid>().HasOne(x => x.Vehicle).WithMany(x => x.Bids).HasForeignKey("AuctionBidFKVehicleId").IsRequired();


            base.OnModelCreating(modelBuilder);
        }
    }
}
