using Ofgem.API.GBI.AddressVerification.Application.DTOs;
using Ofgem.API.GBI.AddressVerification.Application.Extensions;
using System.Text;

namespace Ofgem.API.GBI.AddressVerification.Application.Models
{
    public class SimpleAddress : IEquatable<DTOs.Address>
    {
        public string? AddressReferenceNumber { get; set; }
        public string? FlatNumberOrName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? BuildingName { get; set; }
        public string? Street { get; set; }
        public string? Town { get; set; }
        public string? Postcode { get; set; }
        public string? Uprn { get; set; }

        public bool ValidateFullAddress(Address? other)
        {
            if (other is null)
            {
                return false;
            }

            if (!this.Postcode.Equals_CaseAndWhitespaceInsensitive(other.Postcode))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.BuildingName) &&
                !this.BuildingName.Equals_CaseAndWhitespaceInsensitive(other.BuildingName) &&
                !this.BuildingName.Equals_CaseAndWhitespaceInsensitive(other.OrganisationName))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.BuildingNumber) && !this.BuildingNumber.Equals_CaseAndWhitespaceInsensitive(other.BuildingNumber))
            {
                return false;
            }

            string? buildingNumberOrName = string.IsNullOrEmpty(other.BuildingNumber) ? other.BuildingName : other.BuildingNumber;
            if (!string.IsNullOrEmpty(FlatNumberOrName) && !FlatNumberOrName.Equals_CaseAndWhitespaceInsensitive(buildingNumberOrName))
            {
                return false;
            }

            string? partialStreet = this.Street.RemoveLastWord();
            string? partialOtherStreet = other.Street.RemoveLastWord();
            if (!string.IsNullOrEmpty(partialStreet) && !partialStreet.Equals_CaseAndWhitespaceInsensitive(partialOtherStreet))
            {
                return false;
            }

            return true;
        }

        public bool Equals(Address? other)
        {
            try
            {
                if (other is not null)
                {
                    if (!this.Postcode.Equals_CaseAndWhitespaceInsensitive(other.Postcode))
                    {
                        return false;
                    }

                    string? partialStreet = this.Street.RemoveLastWord();
                    string? partialOtherStreet = other.Street.RemoveLastWord();
                    if (!string.IsNullOrEmpty(this.Street) &&
                        (!(partialStreet.Equals_CaseAndWhitespaceInsensitive(partialOtherStreet) || other.ConcatenatedAddress!.Contains(this.Street))))
                    {
                        return false;
                    }

                    if ((!string.IsNullOrEmpty(this.BuildingName) && !other.ConcatenatedAddress!.Contains(this.BuildingName)) 
                        || (!string.IsNullOrEmpty(this.BuildingNumber) && !other.ConcatenatedAddress!.Contains(this.BuildingNumber)))
                    {
                            return false;
                    }

                    string? buildingNumberOrName = string.IsNullOrEmpty(other.BuildingNumber) ? other.BuildingName : other.BuildingNumber;
                    if (!string.IsNullOrEmpty(FlatNumberOrName) && !FlatNumberOrName.Equals_CaseAndWhitespaceInsensitive(buildingNumberOrName))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            if (!string.IsNullOrEmpty(FlatNumberOrName))
            {
                sb.Append(FlatNumberOrName);
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(BuildingName))
            {
                sb.Append(BuildingName);
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(BuildingNumber))
            {
                sb.Append(BuildingNumber);
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(Street))
            {
                sb.Append(Street);
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(Town))
            {
                sb.Append(Town);
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(Postcode))
            {
                sb.Append(Postcode);
            }
            return sb.ToString();
        }
    }
}
