
using Constellation.Bca.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Constellation.Bca.Infrastructure.Configuration
{
    internal class VehicleTypeConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasAlternateKey(x => new { x.Id, x.UniqueIdentifier });
        }
    }
}
