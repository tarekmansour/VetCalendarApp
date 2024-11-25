namespace Domain.Errors
{
    public static class ClientErrorMessages
    {
        public static readonly string PatientShouldBelongsToSameClient = "Patient must belong to the same client.";
        public static readonly string ClientNameShouldNotBeEmpty = "The client name is invalid, it should not be empty.";
        public static readonly string ClientPhoneNumberShouldNotBeEmpty = "The client phone number is invalid, it should not be empty.";
        public static readonly string InvalidClientPhoneNumber = "The client phone number is not a valid French phone number.";
        public static readonly string ClientEmailShouldNotBeEmpty = "The client email is invalid, it should not be empty.";
        public static readonly string InvalidClientEmail = "The client email is not a valid email.";
        public static readonly string ClientIdShouldNotBeEmpty = "The client Id is invalid, it should not be empty.";
        public static readonly string PatientNameShouldNotBeEmpty = "The patient Name Id is invalid, it should not be empty.";
        public static readonly string PatientSpeciesShouldNotBeEmpty = "The patient species is invalid, it should not be empty.";
        public static readonly string PatientDateOfBirthShouldNotBeEmpty = "The patient DateOfBirth is invalid, it should not be empty.";
        public static readonly string PatientDateOfBirthShouldBeInThePast = "The patient DateOfBirth is invalid, it should be a date in the past.";
        public static readonly string NotFoundClientToAttachClient = "Invalid client Id. Client not found to attach patient.";
    }
}
