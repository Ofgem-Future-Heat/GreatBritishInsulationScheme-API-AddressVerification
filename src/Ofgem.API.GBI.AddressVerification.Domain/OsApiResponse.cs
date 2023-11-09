using System.Text.Json.Serialization;

namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public class OsApiResponse
    {
        [JsonPropertyName("header")]
        public OsApiResponseHeader Header { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<AddressResultContainer> Results { get; set; }

    }
}
