
using Constellation.Bca.Domain.Entites;
using FluentValidation;

namespace Constellation.Bca.Domain.Validators
{
    public class AuctionBidValidator : AbstractValidator<AuctionBid>
    {
        public AuctionBidValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.AuctionId).NotNull().NotEmpty();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => x.Created_at).NotNull().NotEmpty();
            RuleFor(x => x.VehicleId).NotNull().NotEmpty();
        }
    }
}
