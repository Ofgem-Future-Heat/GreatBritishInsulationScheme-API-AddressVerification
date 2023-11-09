namespace Ofgem.API.GBI.AddressVerification.Application.Exceptions
{
    public class AddressValidationException : ApplicationException
    {
        public AddressValidationException(string message) : base(message)
        {

        }
    }
}
