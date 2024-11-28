
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;

namespace Constellation.Bca.Domain.Interfaces.Services
{
    public interface IVehicleService
    {
        public Task<NotificationDomain> AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken);

        Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
    }
}
