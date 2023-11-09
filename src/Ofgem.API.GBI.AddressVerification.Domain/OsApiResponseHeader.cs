using System.Text.Json.Serialization;

namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public class OsApiResponseHeader
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("totalresults")]
        public int TotalResults { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("dataset")]
        public string Dataset { get; set; }

        [JsonPropertyName("lr")]
        public string Lr { get; set; }

        [JsonPropertyName("maxresults")]
        public int MaxResults { get; set; }

        [JsonPropertyName("epoch")]
        public string Epoch { get; set; }

        [JsonPropertyName("output_srs")]
        public string OutputSrs { get; set; }
    }
}
