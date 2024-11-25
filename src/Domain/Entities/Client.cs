using Domain.Errors;
using Domain.Exceptions;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;

public sealed class Client
{
    public ClientId Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public ContactInfo ContactInfo { get; private set; } = default!;
    private readonly ICollection<Patient> _patients = [];
    public IReadOnlyCollection<Patient> Patients => _patients.ToList();
    private Client() { } //For EF core

    public Client(string name, ContactInfo contactInfo)
    {
        Name = name;
        ContactInfo = contactInfo;
    }

    public Client(ClientId id, string name, ContactInfo contactInfo)
    {
        Id = id;
        Name = name;
        ContactInfo = contactInfo;
    }

    public void AddPatient(Patient patient)
    {
        if (patient.ClientId != Id)
            throw new ClientException(ClientErrorMessages.PatientShouldBelongsToSameClient);

        _patients.Add(patient);
    }
}