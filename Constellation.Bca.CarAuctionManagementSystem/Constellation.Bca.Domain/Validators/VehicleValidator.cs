
using Constellation.Bca.Domain.Entites;
using FluentValidation;
using FluentValidation.Results;

namespace Constellation.Bca.Domain.Validators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        public VehicleValidator()
        {
            RuleForDefaultProperties();
        }

        private void RuleForDefaultProperties()
        {
            RuleFor(x => x.Manufacturer).NotNull().NotEmpty();
            RuleFor(x => x.Model).NotNull().NotEmpty();
            RuleFor(x => x.RegistrationYear).InclusiveBetween(1900, DateTime.Now.Year).NotNull();
            RuleFor(x => x.StartingBid).NotNull();
            RuleFor(x => x.VehicleType).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.UniqueIdentifier).NotNull().NotEmpty().Length(17);
            RuleFor(x => x.UserName).NotEmpty().NotNull();
            RuleFor(x => x.IsActive).Equal(true).NotEmpty().NotNull();
        }
    }
}
