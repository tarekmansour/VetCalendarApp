using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;

public sealed class Patient
{
    public PatientId Id { get; private set; } = default!;
    public ClientId ClientId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Species { get; private set; } = default!;
    public DateTime DateOfBirth { get; private set; } = default!;

    private Patient() { } //For EF core

    public Patient(ClientId clientId, string name, string species, DateTime dateOfBirth)
    {
        ClientId = clientId;
        Name = name;
        Species = species;
        DateOfBirth = dateOfBirth;
    }

    public Patient(PatientId id, ClientId clientId, string name, string species, DateTime dateOfBirth)
    {
        Id = id;
        ClientId = clientId;
        Name = name;
        Species = species;
        DateOfBirth = dateOfBirth;
    }
}