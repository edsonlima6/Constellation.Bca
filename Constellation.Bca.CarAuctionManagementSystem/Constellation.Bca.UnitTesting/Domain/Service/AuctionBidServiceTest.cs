
using Bogus;
using Constellation.Bca.Domain.Common;
using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Interfaces.Repository;
using Constellation.Bca.Domain.Interfaces.Services;
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
    internal class AuctionBidServiceTest
    {
        private IAuctionBidRepository auctionBidRepository;
        private IValidator<AuctionBid> auctionBidValidator;
        private IAuctionRepository auctionRepository;
        private IAuctionBidService auctionBidService;

        [SetUp]
        public void Setup()
        {
            auctionBidRepository = Substitute.For<IAuctionBidRepository>();
            auctionRepository = Substitute.For<IAuctionRepository>();
            auctionBidValidator = new AuctionBidValidator();
        }

        [Test]
        public async Task GivenAuctionBidServiceWithNoUserNameThenReceivesBadRequestNotification()
        {
            // Arrange 
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 1)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, 1)
                                                  .RuleFor(x => x.Price, decimal.MaxValue);
            var auctionBidMock = auctionFake.Generate();
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GivenAuctionBidServiceWithNoVehicleIdThenReceivesBadRequestNotification()
        {
            // Arrange 
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 0)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, 1)
                                                  .RuleFor(x => x.Price, decimal.MaxValue);
            var auctionBidMock = auctionFake.Generate();
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GivenAuctionBidServiceWithNoAuctionIdThenReceivesBadRequestNotification()
        {
            // Arrange 
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, 1)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, 0)
                                                  .RuleFor(x => x.Price, decimal.MaxValue);
            var auctionBidMock = auctionFake.Generate();
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task GivenAuctionBidServiceAndHasNoActiveAuctionThenReceivesBadRequestNotification()
        {
            // Arrange 
            int vehicleId = 10;
            auctionRepository.HasAnyActiveAuctionAsync(vehicleId, CancellationToken.None).Returns(false);
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, vehicleId)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, 1)
                                                  .RuleFor(x => x.Price, decimal.MaxValue);
            var auctionBidMock = auctionFake.Generate();
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);
            var expectedNotificationList = new List<string> { ConstantProvider.GetNonExistingActiveAuctionMessage() };

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification.IsValid, Is.False);
            Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedNotificationList));
            await auctionRepository.Received().HasAnyActiveAuctionAsync(vehicleId, CancellationToken.None);
        }

        [Test]
        public async Task GivenAuctionBidServiceAndCannotGetAuctionByIdThenReceivesNotFoundtNotification()
        {
            // Arrange 
            int auctionId = 10;
            int vehicleId = 12;
            var cancelToken = CancellationToken.None;
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, vehicleId)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, auctionId)
                                                  .RuleFor(x => x.Price, decimal.MaxValue);
            var auctionBidMock = auctionFake.Generate();
            auctionRepository.HasAnyActiveAuctionAsync(auctionBidMock.VehicleId, cancelToken).Returns(true);
            auctionRepository.GetAuctionByIdAsync(auctionBidMock.AuctionId, cancelToken).ReturnsNull();
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);
            var expectedNotificationList = new List<string> { ConstantProvider.GetNotFoundAuctionIdMessage() };

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.Multiple(async () =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.False);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedNotificationList));
                await auctionRepository.Received().HasAnyActiveAuctionAsync(vehicleId, cancelToken);
                await auctionRepository.Received().GetAuctionByIdAsync(auctionId, cancelToken);
            });
        }

        [TestCase(10, 12)]
        public async Task GivenAuctionBidServiceAndPriceLesserThanLatestThenReceivesBadRequestNotification(int auctionId, int vehicleId)
        {
            // Arrange 
            var cancelToken = CancellationToken.None;
            var latestAuctionBidPrice = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, vehicleId)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, auctionId)
                                                  .RuleFor(x => x.Price, decimal.MaxValue).Generate();
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, vehicleId)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, auctionId)
                                                  .RuleFor(x => x.Price, decimal.MinValue);
            var auctionBidMock = auctionFake.Generate();
            auctionRepository.HasAnyActiveAuctionAsync(auctionBidMock.VehicleId, cancelToken).Returns(true);
            auctionRepository.GetAuctionByIdAsync(auctionBidMock.AuctionId, cancelToken).Returns(new Faker<Auction>().Generate());
            auctionBidRepository.GetLastAuctionBidAsync(auctionBidMock.AuctionId, auctionBidMock.VehicleId, cancelToken).Returns(latestAuctionBidPrice);
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);
            var expectedNotificationList = new List<string> { ConstantProvider.GetAmountLesserThanLatestBidAuctiontionMessage(latestAuctionBidPrice.Price.ToString()) };

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.Multiple(async () =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.False);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedNotificationList));
                await auctionRepository.Received().HasAnyActiveAuctionAsync(vehicleId, cancelToken);
                await auctionRepository.Received().GetAuctionByIdAsync(auctionId, cancelToken);
            });
        }

        [TestCase(10, 12, 100, 300)]
        public async Task GivenAuctionBidServiceAndPriceGreaterThanLatestThenReceivesEmptyNotification(int auctionId, int vehicleId, int currentMinValue, int newMaxValue)
        {
            // Arrange 
            var cancelToken = CancellationToken.None;
            var latestAuctionBidPrice = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, vehicleId)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, auctionId)
                                                  .RuleFor(x => x.Price, currentMinValue).Generate();
            var auctionFake = new Faker<AuctionBid>().RuleFor(x => x.UserName, y => y.Name.FirstName())
                                                  .RuleFor(x => x.VehicleId, vehicleId)
                                                  .RuleFor(x => x.Created_at, DateTime.Now)
                                                  .RuleFor(x => x.AuctionId, auctionId)
                                                  .RuleFor(x => x.Price, newMaxValue);
            var auctionBidMock = auctionFake.Generate();
            auctionRepository.HasAnyActiveAuctionAsync(auctionBidMock.VehicleId, cancelToken).Returns(true);
            auctionRepository.GetAuctionByIdAsync(auctionBidMock.AuctionId, cancelToken).Returns(new Faker<Auction>().Generate());
            auctionBidRepository.GetLastAuctionBidAsync(auctionBidMock.AuctionId, auctionBidMock.VehicleId, cancelToken).Returns(latestAuctionBidPrice);
            var auctionBidService = new AuctionBidService(auctionBidRepository, auctionBidValidator, auctionRepository);
            var expectedNotificationList = new List<string> { };

            // Act
            var notification = await auctionBidService.PlaceABidAsync(auctionBidMock, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(notification, Is.Not.Null);
                Assert.That(notification.IsValid, Is.True);
                Assert.That(notification.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(notification.ErrorMessages, Is.EquivalentTo(expectedNotificationList));
            });
        }
    }
}
