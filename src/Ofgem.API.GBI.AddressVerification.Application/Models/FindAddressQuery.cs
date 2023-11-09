namespace Ofgem.API.GBI.AddressVerification.Application.Models
{
    public class FindAddressQuery : AddressQuery
    {
        public int MaxResults { get; set; }
        public float MinMatch { get; set; }
        public int MatchPrecision { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            if (this.MinMatch < 0.1f)
            {
                this.MinMatch = 0.8f;
            }
            else if (this?.MinMatch > 1.0f)
            {
                this.MinMatch = 1.0f;
            }

            if (this.MatchPrecision < 1)
            {
                this.MatchPrecision = 1;
            }
            else if (this?.MatchPrecision > 10)
            {
                this.MatchPrecision = 10;
            }

            if (this.MaxResults < 1)
            {
                this.MaxResults = 1;
            }
            else if (this?.MaxResults > 100)
            {
                this.MaxResults = 100;
            }
        }
    }
}
