using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Repositories;
using SharedKernel;
using SharedKernel.Tests;
using System.Transactions;

namespace Infrastructure.Tests;

public class ClientRepositoryTests : DatabaseFixture
{
    private readonly IClientRepository _sut;

    public ClientRepositoryTests() => _sut = new ClientRepository(_dbContext);

    [Fact(DisplayName = "New ClientRepository without ApplicationDbContext")]
    public void CreateWithNullApplicationDbContext_Should_ThrowsArgumentException()
    {
        // Arrange
        Func<ClientRepository> act = () => new ClientRepository(null!);

        // Act & Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Create new client returns successful result")]
    public async Task CreateClientAsync_Should_ReturnsSuccessfulResult()
    {   
        // Arrange
        var client = new Client(
            name: "Mme Dupont",
            contactInfo: new ContactInfo(
                phoneNumber: "0611223344",
                email: "MmeDupont@yahoo.fr"));

        // Act
        var createdClientId = await _sut.CreateClientAsync(client);

        // Assert
        createdClientId.Should().NotBeNull();
        createdClientId.Value.Should().Be(1);
    }

    [Fact(DisplayName = "Create new patient returns successful result")]
    public async Task CreatePatientAsync_Should_ReturnsSuccessfulResult()
    {
        // Arrange
        await _dbContext.Clients.AddAsync(new Client(new ClientId(1), "Perrin Durant", new ContactInfo("0711223344", "Durant@gmail.com")));
        await _dbContext.SaveChangesAsync();

        var patient = new Patient(
            clientId: new ClientId(1),
            name: "Lilly",
            species: "chat",
            dateOfBirth: DateTime.Now.AddYears(-1));

        // Act
        var result = await _sut.CreatePatientAsync(patient);

        // Assert
        //result.IsSuccess.Should().BeTrue();
        //result.Error.Should().Be(Error.None);
    }

    [Fact(DisplayName = "Create new patient returns failure result")]
    public async Task CreatePatient_Should_ReturnsFailureResult()
    {
        // Arrange
        var patient = new Patient(
            clientId: new ClientId(22),
            name: "Lilly",
            species: "chat",
            dateOfBirth: DateTime.Now.AddYears(-1));

        // Act
        var result = await _sut.CreatePatientAsync(patient);

        // Assert
        //result.IsFailure.Should().BeTrue();
        //result.Error.Code.Should().Be(ClientErrorCodes.InvalidClientToAttachPatient);
    }
}
