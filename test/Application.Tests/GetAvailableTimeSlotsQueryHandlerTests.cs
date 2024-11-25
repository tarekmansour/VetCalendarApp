using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests;

public class GetAvailableTimeSlotsQueryHandlerTests
{
    private readonly GetAvailableTimeSlotsQueryHandler _handler;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<GetAvailableTimeSlotsQueryHandler> _logger;

    public GetAvailableTimeSlotsQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _logger = Substitute.For<ILogger<GetAvailableTimeSlotsQueryHandler>>();
        _handler = new GetAvailableTimeSlotsQueryHandler(_logger, _appointmentRepository);
    }

    [Fact(DisplayName = "Handler returns success when TimeSlots found")]
    public async Task Handle_ShouldReturnSuccess_WhenTimeSlotsFound()
    {
        // Arrange
        var availableTimeSlots = new List<TimeSlot>();

        var startTimeSlot1 = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 10);
        var timeSlot1 = new TimeSlot(new TimeSlotId(1), startTimeSlot1, durationInMinutes: 30);

        var startTimeSlot2 = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 8, startTimeInMinutes: 40);
        var timeSlot2 = new TimeSlot(new TimeSlotId(1), startTimeSlot2, durationInMinutes: 60);

        availableTimeSlots.Add(timeSlot1);
        availableTimeSlots.Add(timeSlot2);

        //var timeSlotCollectionDto = new TimeSlotCollectionDto { /* Map to DTO */ };

        _appointmentRepository.GetTimeSlotsByAvailabilityAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(availableTimeSlots);

        // Act
        var result = await _handler.Handle(new GetAvailableTimeSlotsQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.count.Should().Be(2);
        result?.Value?.Items?.FirstOrDefault().Should().NotBeNull();
        _appointmentRepository?.Received(1).GetTimeSlotsByAvailabilityAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>());
        _logger.Received().LogInformation("Successfully retrieving available time Slots.");
    }

    [Fact(DisplayName = "Handler returns success when no TimeSlots found")]
    public async Task Handle_ShouldReturnSuccess_WhenNoTimeSlotsFound()
    {
        // Arrange
        _appointmentRepository.GetTimeSlotsByAvailabilityAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<TimeSlot>());

        // Act
        var result = await _handler.Handle(new GetAvailableTimeSlotsQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.count.Should().Be(0);
        _appointmentRepository?.Received(1).GetTimeSlotsByAvailabilityAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>());
        _logger.Received().LogInformation("Successfully retrieving available time Slots.");
    }
}
