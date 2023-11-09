using Ofgem.API.GBI.AddressVerification.Application.Models;

namespace Ofgem.API.GBI.AddressVerification.Application.DTOs
{
    public class AddressValidationResult
    {
        public SimpleAddress? Address { get; set; }
        public bool IsValid { get; set; }
        public string? Uprn { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;
        public string? CountryCode { get; set; } = null;
    }
}
