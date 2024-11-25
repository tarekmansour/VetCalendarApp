using Application.Dtos;
using Domain.ValueObjects.Identifiers;
using MediatR;
using SharedKernel;

namespace Application.Commands.CreatePatient;

public record CreatePatientCommand(
    ClientId ClientId,
    string Name,
    string Species,
    DateTime DateOfBirth) : IRequest<Result<CreatePatientDto>>;
