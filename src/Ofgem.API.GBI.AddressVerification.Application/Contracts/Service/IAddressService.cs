using Ofgem.API.GBI.AddressVerification.Application.DTOs;
using Ofgem.API.GBI.AddressVerification.Application.Models;

namespace Ofgem.API.GBI.AddressVerification.Application.Contracts.Service
{
    public interface IAddressService
    {
        public Task<IEnumerable<Address>> Find(FindAddressQuery addressQuery);
        public Task<IEnumerable<Address>> FindByUprn(AddressQuery addressQuery);
        public Task<IEnumerable<Address>> FindByPostcode(AddressQuery addressQuery);
        public Task<IEnumerable<AddressValidationResult>> ValidateAddresses(IEnumerable<SimpleAddress> addresses);
    }
}
