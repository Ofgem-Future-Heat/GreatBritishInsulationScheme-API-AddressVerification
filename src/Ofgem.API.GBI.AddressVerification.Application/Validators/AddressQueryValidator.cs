using FluentValidation;
using Ofgem.API.GBI.AddressVerification.Application.Models;

namespace Ofgem.API.GBI.AddressVerification.Application.Validators
{
    public class AddressQueryValidator : AbstractValidator<AddressQuery>
    {
        public AddressQueryValidator()
        {
            RuleFor(address => address.Query)
                .NotEmpty()
                .NotNull();
        }        
    }
}
