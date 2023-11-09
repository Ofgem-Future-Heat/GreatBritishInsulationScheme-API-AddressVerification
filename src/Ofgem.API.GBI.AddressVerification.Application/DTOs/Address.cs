namespace Ofgem.API.GBI.AddressVerification.Application.DTOs
{
    public class Address
    {
        public string? Uprn { get; set; }
        public string? Source { get; set; }
        public string? OrganisationName { get; set; }
        public string? BuildingName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Street { get; set; }
        public string? Town { get; set; }
        public string? Postcode { get; set; }
        public string? ConcatenatedAddress { get; set; }
        public float Match { get; set; }
        public string? MatchDescription { get; set; }

        protected string? _countryCode;

        public string? CountryCode
        {
            get
            {
                return _countryCode?.ToUpper() switch
                {
                    "S" => "GB-SCT",
                    "E" => "GB-ENG",
                    "W" => "GB-WLS",
                    "N" => "GB-NIR",
                    _ => null
                };
            }
            set { _countryCode = value; }
        }
    }
}
