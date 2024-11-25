using Application.Dtos;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<CreatePatientDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<CreatePatientCommand> _commandValidator;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePatientCommandHandler(
        ILogger<CreatePatientCommandHandler> logger,
        IValidator<CreatePatientCommand> commandValidator,
        IClientRepository clientRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<CreatePatientDto>> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "New patient request is not valid with the following error codes '{errorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<CreatePatientDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var createdPatientId = await _clientRepository.CreatePatientAsync(command.MapToPatient(), cancellationToken);
        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created new patient for client '{clientId}' with name '{name}' and species '{Species}'.",
                command.ClientId,
                command.Name,
                command.Species);

        return Result<CreatePatientDto>.Success(new CreatePatientDto(createdPatientId));
    }
}
