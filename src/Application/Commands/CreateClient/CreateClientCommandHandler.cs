using Application.Dtos;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.CreateClient;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<CreatedClientDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<CreateClientCommand> _commandValidator;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(
        ILogger<CreateClientCommandHandler> logger,
        IValidator<CreateClientCommand> commandValidator,
        IClientRepository clientRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<CreatedClientDto>> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "New client request is not valid with the following error codes '{errorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<CreatedClientDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var createdClientId = await _clientRepository.CreateClientAsync(command.MapToClient(), cancellationToken); 
        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created client of name '{name}' with email '{email}'.",
                command.Name,
                command.Email);

        return Result<CreatedClientDto>.Success(new CreatedClientDto(createdClientId));
    }
}
