using Application.Commands.CreateClient;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands;

public partial class CreateClientCommandTests : DatabaseFixture
{
    private readonly ILogger<CreateClientCommandHandler> _logger;
    private readonly CreateClientCommandValidator _validator;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateClientCommandHandler _sut;

    public CreateClientCommandTests()
    {
        _logger = Substitute.For<ILogger<CreateClientCommandHandler>>();
        _validator = new CreateClientCommandValidator();
        _clientRepository = new ClientRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
        _sut = new CreateClientCommandHandler(_logger, _validator, _clientRepository, _unitOfWork);
    }
}
