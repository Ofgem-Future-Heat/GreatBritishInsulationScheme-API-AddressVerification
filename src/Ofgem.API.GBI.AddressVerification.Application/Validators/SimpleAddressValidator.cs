using FluentValidation;
using Ofgem.API.GBI.AddressVerification.Application.Models;

namespace Ofgem.API.GBI.AddressVerification.Application.Validators
{
    public class SimpleAddressValidator : AbstractValidator<SimpleAddress>
    {
        public SimpleAddressValidator()
        {

            RuleFor(address => address)
                .Must(address => 
                    !string.IsNullOrEmpty(address.BuildingNumber) || 
                    !string.IsNullOrEmpty(address.BuildingName) || 
                    !string.IsNullOrEmpty(address.FlatNumberOrName))
                .WithMessage("Building/Flat Name/Number must not all be empty");

            RuleFor(address => address.Postcode)
                .NotEmpty()
                .NotNull();
        }
    }
}
