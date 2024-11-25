using Domain.Errors;
using Domain.Services;

namespace Domain.ValueObjects;

public record ContactInfo
{
    public string PhoneNumber { get; }
    public string Email { get; }
    public ContactInfo(string phoneNumber, string email)
    {
        if (!ContactInfoChecker.IsValidPhoneNumber(phoneNumber))
        {
            throw new ArgumentException(ClientErrorMessages.InvalidClientPhoneNumber, nameof(phoneNumber));
        }

        if (!ContactInfoChecker.IsValidEmail(email))
        {
            throw new ArgumentException(ClientErrorMessages.InvalidClientEmail, nameof(email));
        }

        PhoneNumber = phoneNumber;
        Email = email;
    }
}