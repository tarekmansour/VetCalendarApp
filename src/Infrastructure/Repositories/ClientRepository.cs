using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identifiers;

namespace Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ClientRepository(ApplicationDbContext dbContext) => 
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<ClientId> CreateClientAsync(Client client, CancellationToken cancellationToken)
    {
        await _dbContext.Clients.AddAsync(client, cancellationToken);
        return client.Id;
    }

    public async Task<PatientId> CreatePatientAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await _dbContext.Patients.AddAsync(patient, cancellationToken);
        return patient.Id;
    }

    public async Task<bool> ExistsClientAsync(ClientId clientId, CancellationToken cancellationToken)
    {
        var client = await _dbContext.Clients.FindAsync(clientId, cancellationToken);
        return client != null;
    }
}
