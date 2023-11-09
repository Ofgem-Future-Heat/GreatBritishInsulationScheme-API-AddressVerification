namespace Ofgem.API.GBI.AddressVerification.Domain
{
    public class OsApiQuery
    {
        public string? Query { get; set; }
        public string? Source { get; set; }
        public float MinMatch { get; set; }
        public int MatchPrecision { get; set; }
        public int MaxResults { get; set; }
    }
}
