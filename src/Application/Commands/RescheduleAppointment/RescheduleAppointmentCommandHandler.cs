using Application.Commands.CancelAppointment;
using Application.Dtos;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Domain.Errors;

namespace Application.Commands.RescheduleAppointment;

public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, Result<RescheduledAppointmentDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<RescheduleAppointmentCommand> _commandValidator;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RescheduleAppointmentCommandHandler(
        ILogger<CancelAppointmentCommandHandler> logger,
        IValidator<RescheduleAppointmentCommand> commandValidator,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<RescheduledAppointmentDto>> Handle(RescheduleAppointmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Rescheduling appointment request is not valid with the following error codes '{errorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<RescheduledAppointmentDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(command.AppointmentId, cancellationToken);
        if (appointment == null)
        {
            _logger.LogWarning("Target appointment to reschedule is not found for AppointmentId '{appointmentId}'.", command.AppointmentId);

            return Result<RescheduledAppointmentDto>.Failure(new Error(AppointmentErrorCodes.InvalidPatientId, AppointmentErrorMessages.NotFoundPatientToBookAppointment));
        }

        appointment.Reschedule(command.TimeSlot);

        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully rescheduled appointment for patientId '{patientId}' on new time slot '{startTime}' with duration '{duration}'.",
                appointment.PatientId,
                command.TimeSlot.StartTime,
                command.TimeSlot.DurationInMinutes);

        return Result<RescheduledAppointmentDto>.Success(new RescheduledAppointmentDto(
            Id: appointment.Id,
            PatientId: appointment.PatientId,
            NewStartTime: appointment.TimeSlot.StartTime,
            Status: appointment.Status));

    }
}
