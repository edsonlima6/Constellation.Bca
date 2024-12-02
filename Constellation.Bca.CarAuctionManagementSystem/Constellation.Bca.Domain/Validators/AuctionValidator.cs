
using Constellation.Bca.Domain.Entites;
using FluentValidation;

namespace Constellation.Bca.Domain.Validators
{
    public class AuctionValidator : AbstractValidator<Auction>
    {
        public AuctionValidator()
        {
            RuleForDefaultProperties();
        }

        private void RuleForDefaultProperties()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.VehicleId).NotNull().NotEmpty();
            RuleFor(x => x.Created_At).NotNull().NotEmpty();
        }
    }
}
