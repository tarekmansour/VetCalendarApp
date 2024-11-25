using Application.Dtos;
using Domain.ValueObjects.Identifiers;
using MediatR;
using SharedKernel;

namespace Application.Commands.CancelAppointment;

public record CancelAppointmentCommand(AppointmentId AppointmentId)
    : IRequest<Result<CancelledAppointmentDto>>;
