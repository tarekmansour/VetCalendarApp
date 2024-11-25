using Domain.Entities;
using Domain.ValueObjects.Identifiers;

namespace Domain.Interfaces;

public interface IClientRepository
{
    Task<ClientId>CreateClientAsync(Client client, CancellationToken cancellationToken = default);
    Task<PatientId>CreatePatientAsync(Patient patient, CancellationToken cancellationToken = default);
    Task<bool> ExistsClientAsync(ClientId clientId, CancellationToken cancellationToken = default);
}
