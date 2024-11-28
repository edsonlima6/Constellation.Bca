
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Interfaces.Services;
using Constellation.Bca.Domain.Validators;
using FluentValidation;

namespace Constellation.Bca.Domain.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository, IValidator<Vehicle> validator) : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        public async Task<NotificationDomain> AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            var notification = new NotificationDomain() { StatusCode = System.Net.HttpStatusCode.Created};
            var validatorResult = validator.Validate(vehicle);

            if (!validatorResult.IsValid)
            {
                notification.StatusCode = System.Net.HttpStatusCode.BadRequest;
                notification.AddErrorMessages(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());
                return notification;
            }

            if (await _vehicleRepository.IsUniqueIdentifierInUseAsync(vehicle.UniqueIdentifier, cancellationToken))
            {
                notification.StatusCode = System.Net.HttpStatusCode.BadRequest;
                notification.AddErrorMessage(ConstantProvider.GetDuplicatedIdentifierMessage(vehicle.UniqueIdentifier));
                return notification;
            }

            await _vehicleRepository.AddEntityAsync(vehicle, cancellationToken);
            return notification;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken) => await _vehicleRepository.GetAllAsync(cancellationToken);
    }
}
