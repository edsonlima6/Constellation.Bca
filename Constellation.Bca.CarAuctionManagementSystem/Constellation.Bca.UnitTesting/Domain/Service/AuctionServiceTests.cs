
using Bogus;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Services;
using Constellation.Bca.Domain.Validators;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Constellation.Bca.UnitTesting.Domain.Service
{
    [TestFixture]
    public class AuctionServiceTests
    {
        IAuctionRepository auctionRepository;
        IValidator<Auction> validator;
        IVehicleRepository vehicleRepository;
        private AuctionService AuctionService => new (auctionRepository, validator, vehicleRepository);


        [SetUp]
        public void Setup()
        {
            auctionRepository = Substitute.For<IAuctionRepository>();
            vehicleRepository = Substitute.For<IVehicleRepository>();
            validator = new AuctionValidator();
        }

        [Test]
        public async Task GivenAuctionServiceWithNoUserNameThenReceivesNotification()
        {
            // Arrange 
            var auctionMock = new Faker<Auction>().RuleFor(x => x.UserName, string.Empty)
                                                  .RuleFor(x => x.VehicleId, 1)
                                                  .RuleFor(x => x.Created_At, DateTime.Now);
            var auctionService = AuctionService;

            // Act
            var notification = await auctionService.StartAuctionAsync(auctionMock.Generate(), CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GivenAuctionServiceWithNoVehicleIdThenReceivesNotification()
        {
            // Arrange 
            var auctionMock = new Faker<Auction>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 0)
                                                  .RuleFor(x => x.Created_At, DateTime.Now);
            var auctionService = AuctionService;

            // Act
            var notification = await auctionService.StartAuctionAsync(auctionMock.Generate(), CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GivenAuctionServiceWithNoCreatedDateThenReceivesNotification()
        {
            // Arrange 
            var auctionMock = new Faker<Auction>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 10);
            var auctionService = AuctionService;

            // Act
            var notification = await auctionService.StartAuctionAsync(auctionMock.Generate(), CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GivenAuctionServiceAndRepositoryReturnsFalseThenReceivesNotification()
        {
            // Arrange 
            var cancelToken = CancellationToken.None;
            var auctionFake = new Faker<Auction>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 10)
                                                  .RuleFor(x => x.Created_At, c => DateTime.Now);
            var auctionMock = auctionFake.Generate();
            vehicleRepository.ExistsById(auctionMock.VehicleId).Returns(true);
            auctionRepository.AddEntityAsync(auctionMock, cancelToken).Returns(false);
            var auctionService = AuctionService;

            // Act
            var notification = await auctionService.StartAuctionAsync(auctionMock, cancelToken);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            await auctionRepository.Received().AddEntityAsync(auctionMock, cancelToken);

        }

        [Test]
        public async Task GivenAuctionServiceAndRepositoryReturnsTrueThenReceivesEmptyNotification()
        {
            // Arrange            
            var cancelToken = CancellationToken.None;
            var auctionFake = new Faker<Auction>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 10)
                                                  .RuleFor(x => x.Created_At, c => DateTime.Now);
            var auctionMock = auctionFake.Generate();
            vehicleRepository.ExistsById(auctionMock.VehicleId).Returns(true);
            auctionRepository.AddEntityAsync(auctionMock, cancelToken).Returns(true);
            
            var auctionService = AuctionService;

            // Act
            var notification = await auctionService.StartAuctionAsync(auctionMock, cancelToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.True);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                auctionRepository.Received().AddEntityAsync(auctionMock, cancelToken);
            });


        }

        [Test]
        public async Task GivenAuctionServiceAndVehicleRepositoryReturnsFalseThenReceivesNotification()
        {
            // Arrange            
            var cancelToken = CancellationToken.None;
            var auctionFake = new Faker<Auction>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 11)
                                                  .RuleFor(x => x.Created_At, c => DateTime.Now);
            var auctionMock = auctionFake.Generate();
            vehicleRepository.ExistsById(auctionMock.VehicleId).Returns(false);

            var auctionService = AuctionService;

            // Act
            var notification = await auctionService.StartAuctionAsync(auctionMock, cancelToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.False);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadGateway));
                auctionRepository.DidNotReceive().AddEntityAsync(auctionMock, cancelToken);
            });
        }

        [Test]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        [TestCase(-10, false)]
        public async Task GivenAnAuctionServiceAndStopAuctionIsCalledWithWrongIdThenReceivesNotification(int auctionId, bool isValidExpectedResult)
        {
            // Arrange            
            var cancelToken = CancellationToken.None;
            var auctionService = AuctionService;
            var expectedMessages = new List<string>() { ConstantProvider.GetInvalidAuctionIdMessage() };

            // Act
            var notification = await auctionService.StopAuctionAsync(auctionId, DateTime.Now, cancelToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.EqualTo(isValidExpectedResult));
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedMessages));
                auctionRepository.DidNotReceive().AddEntityAsync(Arg.Any<Auction>(), cancelToken);
            });
        }


        [TestCase(-30)]
        [TestCase(-20)]
        [TestCase(-200)]
        public async Task GivenAnAuctionServiceAndStopAuctionIsCalledWithWrongClosedd_AtThenReceivesNotification(int days)
        {
            // Arrange            
            var closed_at = DateTime.Now.Date.AddDays(days);
            var cancelToken = CancellationToken.None;
            var auctionService = AuctionService;
            var expectedMessages = new List<string>() { ConstantProvider.GetClosed_atMustBeGreanterThanCurrentDayMessage() };

            // Act
            var notification = await auctionService.StopAuctionAsync(int.MaxValue, closed_at, cancelToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.False);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadGateway));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedMessages));
                auctionRepository.DidNotReceive().AddEntityAsync(Arg.Any<Auction>(), cancelToken);
            });
        }

        [Test]
        public async Task GivenAnAuctionServiceAndStopAuctionIsCalledWithWrongAuctionIdThenReceivesNotFoundNotification()
        {
            // Arrange            
            var auctionId = int.MaxValue;
            auctionRepository.GetAuctionByIdAsync(auctionId, CancellationToken.None).ReturnsNull();
            var cancelToken = CancellationToken.None;
            var auctionService = AuctionService;
            var expectedMessages = new List<string>() { ConstantProvider.GetNotFoundAuctionIdMessage() };

            // Act
            var notification = await auctionService.StopAuctionAsync(auctionId, DateTime.Now.Date, cancelToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.False);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedMessages));
                auctionRepository.DidNotReceive().StopAuctionAsync(Arg.Any<Auction>(), cancelToken);
            });
        }

        [Test]
        public async Task GivenAnAuctionServiceAndStopAuctionIsCalledWithWrongAuctionIdThenReceivesEmptyNotification()
        {
            // Arrange            
            var auctionId = int.MaxValue;
            var auctionMock = new Faker<Auction>().Generate();
            auctionRepository.GetAuctionByIdAsync(auctionId, CancellationToken.None).Returns(auctionMock);
            var cancelToken = CancellationToken.None;
            var auctionService = AuctionService;
            var expectedMessages = new List<string>() {  };

            // Act
            var notification = await auctionService.StopAuctionAsync(auctionId, DateTime.Now.Date, cancelToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.True);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedMessages));
                auctionRepository.Received().StopAuctionAsync(auctionMock, cancelToken);
            });
        }
    }
}
