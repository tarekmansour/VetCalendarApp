using Application.Dtos;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Queries;

public class GetAvailableTimeSlotsQueryHandler : IRequestHandler<GetAvailableTimeSlotsQuery, Result<TimeSlotCollectionDto>>
{
    private readonly ILogger _logger;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAvailableTimeSlotsQueryHandler(
        ILogger<GetAvailableTimeSlotsQueryHandler> logger,
        IAppointmentRepository appointmentRepository)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
    }

    public async Task<Result<TimeSlotCollectionDto>> Handle(GetAvailableTimeSlotsQuery request, CancellationToken cancellationToken)
    {
        var availableTimeSlots = await _appointmentRepository.GetTimeSlotsByAvailabilityAsync(isAvailable: true, cancellationToken);
        var availableTimeSlotsCollectionResult = availableTimeSlots.MapToTimeSlotCollectionDto();

        _logger.LogInformation("Successfully retrieving available time Slots.");

        return Result<TimeSlotCollectionDto>.Success(availableTimeSlotsCollectionResult);
    }
}
