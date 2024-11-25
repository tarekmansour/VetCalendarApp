using System.Text.RegularExpressions;

namespace Domain.Services;

internal static class ContactInfoChecker
{
    private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+33|0)[1-9](\d{2}){4}$", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    internal static bool IsValidPhoneNumber(string phoneNumber)
    {
        return PhoneNumberRegex.IsMatch(phoneNumber);
    }

    internal static bool IsValidEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}
