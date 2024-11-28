
using Constellation.Bca.Domain.Entites;

namespace Constellation.Bca.Domain.Interfaces.Repository
{
    public interface IVehicleRepository : IRepositoryBase<Vehicle>
    {
        Task<bool> IsUniqueIdentifierInUseAsync(string uniqueIdentifier, CancellationToken cancellationToken);
        Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
    }
}
