
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Services;
using Constellation.Bca.Domain.Services;
using Constellation.Bca.Domain.Validators;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Constellation.Bca.Application
{
    public static class ServiceCollection
    {
        public static void AddMediatrHost(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        }

        public static void AddMapsterHost(this IServiceCollection services)
        {
            services.AddMapster();
        }

        public static void AddServicesDepencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IQueryFilterService<Vehicle>, VehicleQueryFilterService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IAuctionBidService, AuctionBidService>();
        }
    }
}
