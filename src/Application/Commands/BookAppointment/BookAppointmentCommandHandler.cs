using Application.Dtos;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.BookAppointment;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, Result<BookedAppointmentDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<BookAppointmentCommand> _commandValidator;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookAppointmentCommandHandler(
        ILogger<BookAppointmentCommandHandler> logger,
        IValidator<BookAppointmentCommand> commandValidator,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BookedAppointmentDto>> Handle(BookAppointmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "New appointment booking request is not valid with the following error codes '{errorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<BookedAppointmentDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var bookedAppointmentId = await _appointmentRepository.CreateAppointmentAsync(command.MapToAppointment(), cancellationToken);
        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully booked appointment for patient '{patientId}' on time slot '{startTime}' with duration '{duration}'.",
                command.PatientId,
                command.TimeSlot.StartTime,
                command.TimeSlot.DurationInMinutes);

        return Result<BookedAppointmentDto>.Success(new BookedAppointmentDto(bookedAppointmentId));
    }
}
