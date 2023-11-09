using FluentValidation;

namespace Ofgem.API.GBI.AddressVerification.Application.Validators
{
    public class UprnAddressQueryValidator : AddressQueryValidator
    {
        public UprnAddressQueryValidator()
        {
            RuleFor(address => address.Query).Matches("^[0-9]{1,12}$");
        }        
    }
}
