using System.Text.Json.Serialization;

namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public class AddressResultContainer
    {
        [JsonPropertyName("DPA")]
        public DpaAddressResult? Dpa { get; set; }

        [JsonPropertyName("LPI")]
        public LpiAddressResult? Lpi { get; set; }

        public AddressResult? Value => (Lpi != null) ? Lpi : Dpa;
    }
}
