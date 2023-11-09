namespace Ofgem.API.GBI.AddressVerification.Application.Models
{
    public class AddressQuery
    {
        public string? Query { get; set; }
        public string? Source { get; set; }

        public virtual void SetDefaults()
        {
            if (string.IsNullOrEmpty(this.Source) 
                || this.Source.ToUpper() == "ALL" 
                || (
                    (this.Source.ToUpper() != "LPI") &&
                    (this.Source.ToUpper() != "DPA") &&
                    (this.Source.ToUpper() != "LPI,DPA") &&
                    (this.Source.ToUpper() != "DPA,LPI")
                ))
            {
                this.Source = "LPI,DPA";
            }
        }
    }
}
