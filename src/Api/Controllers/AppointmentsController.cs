using Api.Requests;
using Application.Commands.BookAppointment;
using Application.Commands.CancelAppointment;
using Application.Commands.RescheduleAppointment;
using Application.Queries;
using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
            => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost]
        public async Task<IActionResult> BookAppointmentAsync(
            [FromBody] BookAppointmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new BookAppointmentCommand(
                PatientId: request.PatientId,
                TimeSlot: new TimeSlot(request.StartTime, request.DurationInMinutes));

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsFailure
                    ? BadRequest(result.Errors)
                    : Ok(result.Value);
        }

        [HttpPatch("{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(
            int appointmentId,
            CancellationToken cancellationToken)
        {
            var command = new CancelAppointmentCommand(new AppointmentId(appointmentId));

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsFailure
                    ? BadRequest(result.Errors is IEnumerable<Error> errorList && errorList.Any()
                        ? errorList
                        : result.Error)
                    : Ok(result.Value);
        }

        [HttpPatch("{appointmentId}/reschedule")]
        public async Task<IActionResult> RescheduleAppointment(
            int appointmentId,
            [FromBody] RescheduleAppointmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RescheduleAppointmentCommand(
                new AppointmentId(appointmentId),
                new TimeSlot(request.StartTime, request.DurationInMinutes));

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsFailure
                    ? BadRequest(result.Errors is IEnumerable<Error> errorList && errorList.Any()
                        ? errorList
                        : result.Error)
                    : Ok(result.Value);
        }

        [HttpGet("/availableTimeSlots")]
        public async Task<IActionResult> GetAvailableTimeSlots(CancellationToken cancellationToken)
        {
            var query = new GetAvailableTimeSlotsQuery();
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result?.Value);
        }
    }
}
 