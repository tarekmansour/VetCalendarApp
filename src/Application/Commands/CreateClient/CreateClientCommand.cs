using Application.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Commands.CreateClient;

public record CreateClientCommand(
    string Name,
    string PhoneNumber,
    string Email) : IRequest<Result<CreatedClientDto>>;