using Api.Requests;
using Application.Commands.CreateClient;
using Application.Commands.CreatePatient;
using Domain.ValueObjects.Identifiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(ILogger<ClientsController> logger, IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    public async Task<IActionResult> CreateClientAsync(
        [FromBody] CreateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateClientCommand(request.Name, request.PhoneNumber, request.Email);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(result.Value);
    }

    [HttpPost("{clientId}/patients")]
    public async Task<IActionResult> CreatePatientAsync(
        int clientId,
        [FromBody] CreatePatientRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreatePatientCommand(new ClientId(clientId), request.Name, request.Species, request.DateOfBirth);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsFailure
                ? BadRequest(result.Errors is IEnumerable<Error> errorList && errorList.Any() 
                    ? errorList
                    : result.Error)
                : Ok(result.Value);
    }
}
