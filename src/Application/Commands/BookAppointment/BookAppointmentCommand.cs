using Application.Dtos;
using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using MediatR;
using SharedKernel;

namespace Application.Commands.BookAppointment;

public record BookAppointmentCommand(
    PatientId PatientId,
    TimeSlot TimeSlot) : IRequest<Result<BookedAppointmentDto>>;
