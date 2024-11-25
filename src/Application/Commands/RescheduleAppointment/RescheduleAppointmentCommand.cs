using Application.Dtos;
using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using MediatR;
using SharedKernel;

namespace Application.Commands.RescheduleAppointment;

public record RescheduleAppointmentCommand(
    AppointmentId AppointmentId,
    TimeSlot TimeSlot)
    : IRequest<Result<RescheduledAppointmentDto>>;
