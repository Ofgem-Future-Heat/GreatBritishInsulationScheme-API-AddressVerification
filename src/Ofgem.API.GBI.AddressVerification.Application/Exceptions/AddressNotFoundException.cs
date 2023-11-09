namespace Ofgem.API.GBI.AddressVerification.Application.Exceptions
{
    public class AddressNotFoundException : ApplicationException
    {
        public AddressNotFoundException() : base("Resource not found")
        {

        }
    }
}
