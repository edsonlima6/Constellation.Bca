
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Interfaces.Services;
using FluentValidation;
using System.Net;
using Constellation.Bca.Domain.ValueObjects;

namespace Constellation.Bca.Domain.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository, 
                                IValidator<Vehicle> validator, 
                                IQueryFilterService<Vehicle> queryFilterManagerBase) : ServiceBase, IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        public async Task<NotificationDomain> AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            var notification = new NotificationDomain() { StatusCode = System.Net.HttpStatusCode.Created};
            var validatorResult = validator.Validate(vehicle);

            if (!validatorResult.IsValid)
                return GetNotificationDomain(HttpStatusCode.BadRequest, validatorResult.Errors.Select(x => x.ErrorMessage).ToList(), vehicle);

            if (await _vehicleRepository.IsUniqueIdentifierInUseAsync(vehicle.UniqueIdentifier, cancellationToken))
               return GetNotificationDomain(HttpStatusCode.BadRequest, [ConstantProvider.GetDuplicatedIdentifierMessage(vehicle.UniqueIdentifier)], vehicle);

            await _vehicleRepository.AddEntityAsync(vehicle, cancellationToken);
            return notification;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(QueryFilter queryFilter, CancellationToken cancellationToken)
        {
            if (queryFilter is null || string.IsNullOrEmpty(queryFilter.Field.ToString()) || string.IsNullOrEmpty(queryFilter.Value))
                return await _vehicleRepository.GetAllAsQueryableAsync(cancellationToken);

            var lambdaResponse = queryFilterManagerBase.GetExpressionFilter(queryFilter);
            var vehicleListFromDb = await _vehicleRepository.GetAllAsQueryableAsync(cancellationToken);
            var data = vehicleListFromDb.AsQueryable().Where(lambdaResponse).ToList();

            return data;
        }
    }
}
