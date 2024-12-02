
using Bogus;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Enums;
using Constellation.Bca.Domain.Interfaces.Services;
using Constellation.Bca.Domain.Services;
using Constellation.Bca.Domain.ValueObjects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace Constellation.Bca.UnitTesting.Domain.Service
{
    [TestFixture]
    internal class QueryFilterServiceTest
    {
        IQueryFilterService<Vehicle> queryFilterService;
        private IQueryable<Vehicle> GetVehiclesMockByModel(string model) =>
            new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                           .RuleFor(x => x.Model, y => model)
                                           .RuleFor(x => x.Name, y => y.Name.Suffix())
                                           .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
                                           .Generate(10).AsQueryable();

        private IQueryable<Vehicle> GetVehiclesMockByName(string name) =>
            new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                           .RuleFor(x => x.Model, y => y.Name.Suffix())
                                           .RuleFor(x => x.Name, y => name)
                                           .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
                                           .Generate(10).AsQueryable();

        private IQueryable<Vehicle> GetVehiclesMockByManufacturer(string manufacturer) =>
          new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => manufacturer)
                                         .RuleFor(x => x.Model, y => y.Name.Suffix())
                                         .RuleFor(x => x.Name, y => y.Name.Suffix())
                                         .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
                                         .Generate(10).AsQueryable();

        private IQueryable<Vehicle> GetVehiclesMockByVehicleType(VehicleType vehicleType) =>
        new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                       .RuleFor(x => x.Model, y => y.Name.Suffix())
                                       .RuleFor(x => x.Name, y => y.Name.Suffix())
                                       .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
                                       .RuleFor(x => x.VehicleType, vehicleType)
                                       .Generate(10).AsQueryable();

        private IQueryable<Vehicle> GetVehiclesMockByRegistrationYear(int registrationYear) =>
        new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                .RuleFor(x => x.Model, y => y.Name.Suffix())
                                .RuleFor(x => x.Name, y => y.Name.Suffix())
                                .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
                                .RuleFor(x => x.VehicleType, VehicleType.SUV)
                                .RuleFor(x => x.RegistrationYear, registrationYear)
                                .Generate(10).AsQueryable();

        private IQueryable<Vehicle> GetDefaultVehicleMock() => new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                           .RuleFor(x => x.Model, y => y.Vehicle.Model())
                                           .RuleFor(x => x.Name, y => y.Name.Suffix())
                                           .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin()).Generate(10).AsQueryable();

        [TestCase("Renault")]
        [TestCase("Megane")]
        public void GivenQueryFilterAndFilterEqualsNameThenReturnVehiclesByName(string name)
        {
            // Arrange
            var vehicles = new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                           .RuleFor(x => x.Model, y => y.Vehicle.Model())
                                           .RuleFor(x => x.Name, name)
                                           .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin())
                                           .Generate(10).AsQueryable();

            var randonVehicles = new Faker<Vehicle>().RuleFor(x => x.Manufacturer, y => y.Vehicle.Manufacturer())
                                           .RuleFor(x => x.Model, y => y.Vehicle.Model())
                                           .RuleFor(x => x.Name, y => y.Name.Suffix())
                                           .RuleFor(x => x.UniqueIdentifier, y => y.Vehicle.Vin()).Generate(10).ToList();

            randonVehicles.AddRange(vehicles);
            var queryFilter = new Faker<QueryFilter>().RuleFor(x => x.Field, Columns.Name)
                .RuleFor(x => x.Value, name)
                .RuleFor(x => x.Operator, Operator.Equal);
            queryFilterService = new VehicleQueryFilterService();

            // Act 
            var expression = queryFilterService.GetExpressionFilter(queryFilter);
            var filterList = randonVehicles.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(expression, Is.Not.Null);
                Assert.That(filterList, Is.Not.Null);
                Assert.That(filterList, Has.Count.EqualTo(vehicles.Count()));
            });
        }

        [TestCase("Serie 2")]
        public void GivenQueryFilterAndFilterModelColumnWithEqualsOpThenReturnVehiclesByName(string model)
        {
            // Arrange
            var vehicles = GetVehiclesMockByModel(model);

            var randonVehicles = GetDefaultVehicleMock().ToList();

            randonVehicles.AddRange(vehicles);
            var queryFilter = new Faker<QueryFilter>().RuleFor(x => x.Field, Columns.Model)
                .RuleFor(x => x.Value, model)
                .RuleFor(x => x.Operator, Operator.Equal);
            queryFilterService = new VehicleQueryFilterService();

            // Act 
            var expression = queryFilterService.GetExpressionFilter(queryFilter);
            var filterList = randonVehicles.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(expression, Is.Not.Null);
                Assert.That(filterList, Is.Not.Null);
                Assert.That(filterList, Has.Count.EqualTo(vehicles.Count()));
            });
        }

        [TestCase(VehicleType.SUV)]
        [TestCase(VehicleType.Truck)]
        [TestCase(VehicleType.Sedan)]
        public void GivenQueryFilterAndFilterVehicleTypeColumnWithEqualsOpThenReturnVehiclesByName(VehicleType vehicleType)
        {
            // Arrange
            var vehicles = GetVehiclesMockByVehicleType(vehicleType);
            var randonVehicles = GetDefaultVehicleMock().ToList();

            randonVehicles.AddRange(vehicles);
            var queryFilter = new Faker<QueryFilter>().RuleFor(x => x.Field, Columns.VehicleType)
                .RuleFor(x => x.Value, vehicleType.ToString())
                .RuleFor(x => x.Operator, Operator.Equal);
            queryFilterService = new VehicleQueryFilterService();

            // Act 
            var expression = queryFilterService.GetExpressionFilter(queryFilter);
            var filterList = randonVehicles.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(expression, Is.Not.Null);
                Assert.That(filterList, Is.Not.Null);
                Assert.That(filterList, Has.Count.EqualTo(vehicles.Count()));
                Assert.That(filterList.Select(x => x.VehicleType), Is.EquivalentTo(vehicles.Select(x => x.VehicleType)));
            });
        }

        [TestCase(VehicleType.SUV, "modelMock")]
        [TestCase(VehicleType.Truck, "modelMock2")]
        [TestCase(VehicleType.Sedan, "modelMock3")]
        public void GivenQueryFilterAndFilterVehicleTypeAndModelThenReturnVehiclesByName(VehicleType vehicleType, string model)
        {
            // Arrange
            var vehiclesByType = GetVehiclesMockByVehicleType(vehicleType).ToList();
            var vehiclesByModel = GetVehiclesMockByModel(model).ToList();
            var randonVehicles = GetDefaultVehicleMock().ToList();
            vehiclesByType.ForEach(x => x.Model = model);
            vehiclesByModel.ForEach(x => x.VehicleType = vehicleType);

            randonVehicles.AddRange(vehiclesByType);
            randonVehicles.AddRange(vehiclesByModel);
            int countListExpected = vehiclesByType.Count() + vehiclesByModel.Count();
            var queryFilter = new Faker<QueryFilter>().RuleFor(x => x.Field, Columns.VehicleType)
                .RuleFor(x => x.Value, vehicleType.ToString())
                .RuleFor(x => x.Operator, Operator.Equal)
                .RuleFor(x => x.QueryFilters, new List<QueryFilter>())
                .RuleFor(x => x.Logic, Logic.And).Generate();

            var subQuery = new Faker<QueryFilter>().RuleFor(x => x.Field, Columns.Model)
                .RuleFor(x => x.Value, model.Substring(0,3))
                .RuleFor(x => x.Operator, Operator.Contains)
                .RuleFor(x => x.Logic, Logic.None).Generate();

            queryFilter.QueryFilters.Add(subQuery);

            queryFilterService = new VehicleQueryFilterService();

            // Act 
            var expression = queryFilterService.GetExpressionFilter(queryFilter);
            var filterList = randonVehicles.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(expression, Is.Not.Null);
                Assert.That(filterList, Is.Not.Null);
                Assert.That(filterList, Has.Count.EqualTo(countListExpected));
            });
        }
    }
}
