
using Bogus;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Enums;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Interfaces.Services;
using Constellation.Bca.Domain.Services;
using Constellation.Bca.Domain.Validators;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Runtime.CompilerServices;

namespace Constellation.Bca.UnitTesting.Domain.Service
{
    [TestFixture]
    internal class VehicleServiceTests
    {
        private IVehicleRepository _vehicleRepository;
        private IValidator<Vehicle> _validator;
        private IVehicleService _vehicleService;

        [SetUp]
        public void Setup()
        {
            _vehicleRepository = Substitute.For<IVehicleRepository>();
            _validator = new VehicleValidator();
        }

        [Test]
        public async Task GivenVehicleServiceAndTriggerAddVehicleAsyncThenReturnsNoErrorMessages()
        {
            // Arrange
            var fake = new Faker<Vehicle>()
               .RuleFor(x => x.Name, y => y.Lorem.Word())
               .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
               .RuleFor(x => x.VehicleType, VehicleType.Sedan)
               .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
               .RuleFor(x => x.Model, y => y.Vehicle.Model())
               .RuleFor(x => x.RegistrationYear, y => 2024);

            var vehicle = fake.Generate();

            _vehicleService = new VehicleService(_vehicleRepository, _validator);

            // Act
            NotificationDomain notificationDomain = await _vehicleService.AddVehicleAsync(vehicle, CancellationToken.None);


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notificationDomain, Is.Not.Null);
                Assert.That(notificationDomain.IsValid, Is.True);
                Assert.That(notificationDomain.ErrorMessages, Has.Count.EqualTo(0));
            });
        }

        [TestCase("5HD1GX4117K301958", false, HttpStatusCode.Created)]
        [TestCase("1FUPFSEB3YLF03840", false, HttpStatusCode.Created)]
        public async Task GivenVehicleServiceWithNonExistingIdentifierAndTriggerAddVehicleAsyncThenReturnsCreated(string identifier, bool isUniqueIdentifierInUse, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            _vehicleRepository.IsUniqueIdentifierInUseAsync(identifier, CancellationToken.None).Returns(isUniqueIdentifierInUse);

            var fake = new Faker<Vehicle>()
               .RuleFor(x => x.Name, y => y.Lorem.Word())
               .RuleFor(x => x.UniqueIdentifier, y => identifier)
               .RuleFor(x => x.VehicleType, VehicleType.Sedan)
               .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
               .RuleFor(x => x.Model, y => y.Vehicle.Model())
               .RuleFor(x => x.RegistrationYear, y => 2024);

            var vehicle = fake.Generate();
            _vehicleService = new VehicleService(_vehicleRepository, _validator);

            // Act
            NotificationDomain notificationDomain = await _vehicleService.AddVehicleAsync(vehicle, CancellationToken.None);


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notificationDomain, Is.Not.Null);
                Assert.That(notificationDomain.IsValid, Is.True);
                Assert.That(notificationDomain.ErrorMessages, Has.Count.EqualTo(0));
                Assert.That(notificationDomain.StatusCode, Is.EqualTo(expectedStatusCode));
                _vehicleRepository.Received().AddEntityAsync(vehicle, CancellationToken.None);
            });
        }

        [TestCase("5HD1GX4117K301958", true, HttpStatusCode.BadRequest)]
        [TestCase("1FUPFSEB3YLF03840", true, HttpStatusCode.BadRequest)]
        public async Task GivenVehicleServiceWithExistingIdentifierAndTriggerAddVehicleAsyncThenReturnsBadRequest(string identifier, bool isUniqueIdentifierInUse, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            _vehicleRepository.IsUniqueIdentifierInUseAsync(identifier, CancellationToken.None).Returns(isUniqueIdentifierInUse);

            var fake = new Faker<Vehicle>()
               .RuleFor(x => x.UniqueIdentifier, y => identifier)
               .RuleFor(x => x.VehicleType, VehicleType.Sedan)
               .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
               .RuleFor(x => x.Model, y => y.Vehicle.Model())
               .RuleFor(x => x.RegistrationYear, y => 2024);

            var vehicle = fake.Generate();
            _vehicleService = new VehicleService(_vehicleRepository, _validator);

            // Act
            NotificationDomain notificationDomain = await _vehicleService.AddVehicleAsync(vehicle, CancellationToken.None);


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notificationDomain, Is.Not.Null);
                Assert.That(notificationDomain.IsValid, Is.False);
                Assert.That(notificationDomain.ErrorMessages, Has.Count.GreaterThan(0));
                Assert.That(notificationDomain.StatusCode, Is.EqualTo(expectedStatusCode));
                _vehicleRepository.DidNotReceive().AddEntityAsync(Arg.Any<Vehicle>(), CancellationToken.None);
            });
        }

        [TestCase("5HD1GX4117K", VehicleType.Sedan, HttpStatusCode.BadRequest)]
        [TestCase("", VehicleType.SUV, HttpStatusCode.BadRequest)]
        public async Task GivenVehicleServiceWithWrongIdentifierThenReturnsErrorMessagesAndBadRequest(string? identifier, VehicleType vehicleType, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fake = new Faker<Vehicle>()
               .RuleFor(x => x.UniqueIdentifier, y => identifier)
               .RuleFor(x => x.VehicleType, vehicleType)
               .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
               .RuleFor(x => x.Model, y => y.Vehicle.Model())
               .RuleFor(x => x.RegistrationYear, y => 2024);

            var vehicle = fake.Generate();
            _vehicleService = new VehicleService(_vehicleRepository, _validator);

            // Act
            NotificationDomain notificationDomain = await _vehicleService.AddVehicleAsync(vehicle, CancellationToken.None);


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notificationDomain, Is.Not.Null);
                Assert.That(notificationDomain.IsValid, Is.False);
                Assert.That(notificationDomain.ErrorMessages, Has.Count.GreaterThan(0));
                Assert.That(notificationDomain.StatusCode, Is.EqualTo(expectedStatusCode));
                _vehicleRepository.DidNotReceive().IsUniqueIdentifierInUseAsync(Arg.Any<string>(), CancellationToken.None);
                _vehicleRepository.DidNotReceive().AddEntityAsync(Arg.Any<Vehicle>(), CancellationToken.None);
            });
        }
    }
}
