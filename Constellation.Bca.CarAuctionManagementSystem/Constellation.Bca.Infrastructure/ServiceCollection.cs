using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Validators;
using Constellation.Bca.Infrastructure.Context;
using Constellation.Bca.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Constellation.Bca.Infrastructure
{
    public static class ServiceCollection
    {
        public static void AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseInMemoryDatabase("CarAuction"));
        }

        public static void AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IAuctionBidRepository, AuctionBidRepository>();

            // Validators
            services.AddScoped<IValidator<Vehicle>, VehicleValidator>();
            services.AddScoped<IValidator<Auction>, AuctionValidator>();
            services.AddScoped<IValidator<AuctionBid>, AuctionBidValidator>();
        }

        public static void IncludeMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
        }
    }
}
