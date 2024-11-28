
using Constellation.Bca.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Constellation.Bca.Infrastructure.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
                
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        {
                
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CarAuction-Data.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                        .HasAlternateKey(x => new { x.Id, x.UniqueIdentifier });

            base.OnModelCreating(modelBuilder);
        }
    }
}
