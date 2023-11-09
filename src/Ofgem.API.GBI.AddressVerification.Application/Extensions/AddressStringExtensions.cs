namespace Ofgem.API.GBI.AddressVerification.Application.Extensions
{
    public static class AddressStringExtensions
    {
        public static bool Equals_CaseAndWhitespaceInsensitive(this string? match, string? other)
        {
            match = match?.ToUpper()?.Replace(" ", "") ?? "";
            other = other?.ToUpper()?.Replace(" ", "") ?? "";
            return match.Equals(other);
        }

        public static string RemoveLastWord(this string? text)
        {
            string[] splitString = (text ?? "").Trim().Split(' ');
            if (splitString.Length == 0)
            {
                return "";
            }
            else if (splitString.Length == 1)
            {
                return splitString.First();
            }
            return splitString.SkipLast(1).Aggregate((current, next) => $"{current} {next}") ?? "";
        }
    }
}
