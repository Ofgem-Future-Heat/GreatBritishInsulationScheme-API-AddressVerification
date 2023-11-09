using System.Text.Json.Serialization;

namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public abstract class AddressResult
    {
        public virtual string? Source
        {
            get; 
        }

        [JsonPropertyName("UPRN")]
        public string? Uprn { get; set; }

        [JsonPropertyName("ADDRESS")]
        public string? Address { get; set; }

        [JsonPropertyName("COUNTRY_CODE")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("COUNTRY_CODE_DESCRIPTION")]
        public string? CountryCodeDescription { get; set; }

        [JsonPropertyName("X_COORDINATE")]
        public float XCoordinate { get; set; }

        [JsonPropertyName("Y_COORDINATE")]
        public float YCoordinate { get; set; }

        [JsonPropertyName("MATCH")]
        public float Match { get; set; }

        [JsonPropertyName("MATCH_DESCRIPTION")]
        public string? MatchDescription { get; set; }
    }
}
