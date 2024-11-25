using Application.Dtos;
using Domain.Errors;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result<CancelledAppointmentDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<CancelAppointmentCommand> _commandValidator;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppointmentCommandHandler(
        ILogger<CancelAppointmentCommandHandler> logger,
        IValidator<CancelAppointmentCommand> commandValidator,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<CancelledAppointmentDto>> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Cancelling appointment request is not valid with the following error codes '{errorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<CancelledAppointmentDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(command.AppointmentId, cancellationToken);
        if (appointment == null)
        {
            _logger.LogWarning("Target appointment to cancel is not found for AppointmentId '{appointmentId}'.", command.AppointmentId);

            return Result<CancelledAppointmentDto>.Failure(new Error(AppointmentErrorCodes.InvalidPatientId, AppointmentErrorMessages.NotFoundPatientToBookAppointment));
        }

        appointment.Cancel();

        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully cancelled appointment for patientId '{patientId}' on time slot '{startTime}' with duration '{duration}'.",
                appointment.PatientId,
                appointment.TimeSlot.StartTime,
                appointment.TimeSlot.DurationInMinutes);

        return Result<CancelledAppointmentDto>.Success(new CancelledAppointmentDto(
            Id: appointment.Id,
            PatientId: appointment.PatientId,
            StartTime: appointment.TimeSlot.StartTime,
            Status: appointment.Status));
    }
}