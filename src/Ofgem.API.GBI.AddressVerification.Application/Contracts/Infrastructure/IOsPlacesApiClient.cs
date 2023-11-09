using Ofgem.API.GBI.AddressVerification.Domain;

namespace Ofgem.API.GBI.AddressVerification.Application.Contracts.Infrastructure
{
    public interface IOsPlacesApiClient
    {
        Task<OsApiResponse> FindByUprn(OsApiQuery osApiQuery);
        Task<OsApiResponse> FindBySearchQuery(OsApiQuery osApiQuery);
        Task<OsApiResponse> FindByPostcode(OsApiQuery osApiQuery);
    }
}
