using System.Text.Json.Serialization;

namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public class DpaAddressResult : AddressResult
    {
        public override string? Source
        {
            get { return "DPA"; }
        }

        [JsonPropertyName("ORGANISATION_NAME")]
        public string? OrganisationName { get; set; }

        [JsonPropertyName("BUILDING_NUMBER")]
        public string? BuildingNumber { get; set; }

        [JsonPropertyName("THOROUGHFARE_NAME")]
        public string? ThoroughfareName { get; set; }

        [JsonPropertyName("POST_TOWN")]
        public string? PostTown { get; set; }

        [JsonPropertyName("POSTCODE")]
        public string? Postcode { get; set; }
    }
}
