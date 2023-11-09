using System.Text.Json.Serialization;

namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public class LpiAddressResult : AddressResult
    {
        public override string? Source
        {
            get { return "LPI"; }
        }

        [JsonPropertyName("LPI_KEY")]
        public string? LpiKey { get; set; }

        [JsonPropertyName("PAO_START_NUMBER")]
        public string? PaoStartNumber { get; set; }

        [JsonPropertyName("PAO_END_NUMBER")]
        public string? PaoEndNumber { get; set; }

        [JsonPropertyName("PAO_TEXT")]
        public string? PaoText { get; set; }

        [JsonPropertyName("STREET_DESCRIPTION")]
        public string? StreetDescription { get; set; }

        [JsonPropertyName("LOCALITY_NAME")]
        public string? LocalityName { get; set; }

        [JsonPropertyName("TOWN_NAME")]
        public string? TownName { get; set; }

        [JsonPropertyName("ADMINISTRATIVE_AREA")]
        public string? AdministrativeArea { get; set; }

        [JsonPropertyName("POSTCODE_LOCATOR")]
        public string? PostcodeLocator { get; set; }
    }
}
