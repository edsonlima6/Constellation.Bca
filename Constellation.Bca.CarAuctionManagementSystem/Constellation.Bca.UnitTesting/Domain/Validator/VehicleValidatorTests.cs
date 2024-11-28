
using Bogus;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Enums;
using Constellation.Bca.Domain.Validators;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.UnitTesting.Domain.Validator
{
    [TestFixture]
    internal class VehicleValidatorTests
    {
        private IValidator<Vehicle> vehicleValidator;

        [SetUp]
        public void Setup()
        {
            vehicleValidator = new VehicleValidator();
        }

        [TestCase(VehicleType.Sedan, "JYAVP18E07A005321")]
        [TestCase(VehicleType.SUV, "5HD1GX4117K301958")]
        [TestCase(VehicleType.Truck, "1FUPFSEB3YLF03840")]
        [TestCase(VehicleType.Hatchback, "4T4BF1FK4CR236137")]
        public void GivenVehicleValidatorWithVehicleTypeAndUniqueIdentifierCorrectThenIsValid(VehicleType vehicleType, string uniqueIdentifier)
        {
            // arrange 
            var fake = new Faker<Vehicle>()
                .RuleFor(x => x.Name, y => y.Lorem.Word())
                .RuleFor(x => x.UniqueIdentifier, uniqueIdentifier)
                .RuleFor(x => x.VehicleType, vehicleType)
                .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                .RuleFor(x => x.Model, y => y.Vehicle.Model())
                .RuleFor(x => x.RegistrationYear, y => 2024);

            var vehicle = fake.Generate();

            // act
            var validatorResult = vehicleValidator.Validate(vehicle);


            // assert
            Assert.That(validatorResult.IsValid, Is.True);
            Assert.That(validatorResult.Errors, Is.Empty);
        }

        [TestCase("")]
        [TestCase(null)]
        public void GivenVehicleValidatorWithNoManufacturerThenValidatorIsNotValid(string? manufacturer)
        {
            // arrange 
            var fake = new Faker<Vehicle>()
              .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
              .RuleFor(x => x.VehicleType,  VehicleType.Sedan)
              .RuleFor(x => x.Manufacturer, manufacturer)
              .RuleFor(x => x.Model, y => y.Vehicle.Model())
              .RuleFor(x => x.RegistrationYear, y => 2024);

            var vehicle = fake.Generate();

            // act
            var validatorResult = vehicleValidator.Validate(vehicle);

            Assert.Multiple(() =>
            {
                Assert.That(validatorResult.IsValid, Is.False);
                Assert.That(validatorResult.Errors, Is.Not.Empty);
                Assert.That(validatorResult.Errors.Count, Is.GreaterThan(0));
            });
            
        }

        [TestCase("")]
        [TestCase(null)]
        public void GivenVehicleValidatorWithNoModelThenValidatorIsNotValid(string? model)
        {
            // arrange 
            var fake = new Faker<Vehicle>()
              .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
              .RuleFor(x => x.VehicleType, VehicleType.Sedan)
              .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
              .RuleFor(x => x.Model, y => model)
              .RuleFor(x => x.RegistrationYear, y => 2000);

            var vehicle = fake.Generate();

            // act
            var validatorResult = vehicleValidator.Validate(vehicle);

            Assert.Multiple(() =>
            {
                Assert.That(validatorResult.IsValid, Is.False);
                Assert.That(validatorResult.Errors, Is.Not.Empty);
                Assert.That(validatorResult.Errors.Count, Is.GreaterThan(0));
            });

        }

        [TestCase(10)]
        [TestCase(1020)]
        [TestCase(1500)]
        [TestCase(1800)]
        [TestCase(0)]
        public void GivenVehicleValidatorWithNoRegistrationYearThenValidatorIsNotValid(int? registrationYear)
        {
            // arrange 
            var fake = new Faker<Vehicle>()
              .RuleFor(x => x.UniqueIdentifier, "JYAVP18E07A005321")
              .RuleFor(x => x.VehicleType, VehicleType.Sedan)
              .RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
              .RuleFor(x => x.Model, y => y.Vehicle.Model())
              .RuleFor(x => x.RegistrationYear, registrationYear);

            var vehicle = fake.Generate();

            // act
            var validatorResult = vehicleValidator.Validate(vehicle);

            Assert.Multiple(() =>
            {
                Assert.That(validatorResult.IsValid, Is.False);
                Assert.That(validatorResult.Errors, Is.Not.Empty);
                Assert.That(validatorResult.Errors, Has.Count.GreaterThan(0));
            });

        }

        //[TestCase("JYAVP18E07")]
        //[TestCase("")]
        [TestCase(null)]
        public void GivenVehicleValidatorWithNonValidUniqueIdentifierThenValidatorIsNotValid(string? uniqueIdentifier)
        {
            // arrange 
            var fake = new Faker<Vehicle>()
              .RuleFor(x => x.UniqueIdentifier, uniqueIdentifier)
              .RuleFor(x => x.VehicleType, VehicleType.Sedan)
              .RuleFor(x => x.Manufacturer, y => y.Lorem.Word())
              .RuleFor(x => x.Model, y => y.Lorem.Word())
              .RuleFor(x => x.RegistrationYear, y => 2000);

            var vehicle = fake.Generate();

            // act
            var validatorResult = vehicleValidator.Validate(vehicle);

            Assert.Multiple(() =>
            {
                Assert.That(validatorResult.IsValid, Is.False);
                Assert.That(validatorResult.Errors, Is.Not.Empty);
                Assert.That(validatorResult.Errors, Has.Count.GreaterThan(0));
            });

        }
    }
}
