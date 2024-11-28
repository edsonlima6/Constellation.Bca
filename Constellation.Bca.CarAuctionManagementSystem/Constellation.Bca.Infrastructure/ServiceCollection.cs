using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Infrastructure.Context;
using Constellation.Bca.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Constellation.Bca.Infrastructure
{
    public static class ServiceCollection
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseSqlite(configuration.GetConnectionString("CarManagmentDatabase")));
        }

        public static void AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
        }

        public static void InitializeDatabase(this IApplicationBuilder applicationBuilder)
        {
            var scopeFactory = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var pending = databaseContext.Database.GetPendingMigrations().ToList();
            if (pending.Count != 0)
            {
                databaseContext.Database.Migrate();
            }
        }

        public static void IncludeMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
        }
    }
}
