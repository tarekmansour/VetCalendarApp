using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;

namespace Domain.Tests;

public class ClientTests
{
    [Fact(DisplayName = "Attach patient to client")]
    public void AddPatient_Should_AttachPatient()
    {
        // Arrange
        var client = new Client(
            id: new ClientId(1),
            name: "Evan Mitchell",
            contactInfo: new ContactInfo(
                phoneNumber: "0122336655",
                email: "evan.mitchell@outlook.com"));

        var patient = new Patient(
            clientId: new ClientId(1),
            name: "Minou",
            species: "chat",
            dateOfBirth: DateTime.Now.AddYears(-2));

        // Act
        client.AddPatient(patient);

        // Assert
        client.Patients.FirstOrDefault().Should().NotBeNull();
        client.Patients.FirstOrDefault().Should().BeEquivalentTo(patient);
    }

    [Fact(DisplayName = "New Patient with invalid email")]
    public void NewClient_WithInvalidEmail_Should_ThrowsArgumentException()
    {
        // Arrange
        Func<Client> act = () => new Client(
            name: "Alice",
            new ContactInfo(
                phoneNumber: "0644355465",
                email: "invalid"));

        // Act & Assert
        act.Should().Throw<ArgumentException>();
    }
}
