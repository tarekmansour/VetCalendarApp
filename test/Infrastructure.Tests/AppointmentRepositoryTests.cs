using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Repositories;
using SharedKernel.Tests;

namespace Infrastructure.Tests;

public class AppointmentRepositoryTests : DatabaseFixture
{
    private readonly IAppointmentRepository _sut;

    public AppointmentRepositoryTests() => _sut = new AppointmentRepository(_dbContext);

    [Fact(DisplayName = "GetAllTimeSlotsAsync returns only unavailable time slots")]
    public async Task GetAllTimeSlotsAsync_ReturnsOnlyUnavailableTimeSlots()
    {
        // Arrange
        var startTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 10);
        var availableTimeSlot = new TimeSlot(new TimeSlotId(1), startTimeSlot, durationInMinutes: 30);

        var anOtherStartTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 5, startTimeInMinutes: 25);
        var anOtherTimeSlot = new TimeSlot(new TimeSlotId(2), anOtherStartTimeSlot, durationInMinutes: 30);
        anOtherTimeSlot.Release();

        _dbContext.TimeSlots.AddRange(availableTimeSlot, anOtherTimeSlot);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetTimeSlotsByAvailabilityAsync(isAvailable: false, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Should().ContainSingle(ts => ts.IsAvailable == false);
    }

    [Fact(DisplayName = "CreateAppointmentAsync adds appointment and returns its ID")]
    public async Task CreateAppointmentAsync_AddsAppointment_ReturnsAppointmentId()
    {
        // Arrange
        var startTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 10);
        var timeSlotBookedByPatient1 = new TimeSlot(new TimeSlotId(1), startTimeSlot, durationInMinutes: 30);
        var appointment = new Appointment(new AppointmentId(25), new PatientId(1), timeSlotBookedByPatient1);

        // Act
        var result = await _sut.CreateAppointmentAsync(appointment, CancellationToken.None);
        await _dbContext.SaveChangesAsync();

        // Assert
        result.Should().Be(appointment.Id);
        _dbContext.Appointments.Should().Contain(appointment);
    }

    [Fact(DisplayName = "ExistsPatientAsync returns false if patient does not exist")]
    public async Task ExistsPatientAsync_ReturnsFalseIfPatientDoesNotExist()
    {
        // Arrange & Act
        var result = await _sut.ExistsPatientAsync(new PatientId(1), CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact(DisplayName = "GetAppointmentByIdAsync returns appointment when it exists")]
    public async Task GetAppointmentByIdAsync_ReturnsAppointment_WhenItExists()
    {
        // Arrange
        var appointmentId = new AppointmentId(14);
        var startTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 10);
        var timeSlotBookedByPatient1 = new TimeSlot(new TimeSlotId(1), startTimeSlot, durationInMinutes: 30);
        var appointment = new Appointment(appointmentId, new PatientId(1), timeSlotBookedByPatient1);

        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAppointmentByIdAsync(appointmentId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(appointmentId);
    }

    [Fact(DisplayName = "GetAppointmentByIdAsync returns null when appointment does not exist")]
    public async Task GetAppointmentByIdAsync_ReturnsNull_WhenAppointmentDoesNotExist()
    {
        // Arrange
        var appointmentId = new AppointmentId(215);

        // Act
        var result = await _sut.GetAppointmentByIdAsync(appointmentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "GetAppointmentByIdAsync throws exception when cancellation is requested")]
    public async Task GetAppointmentByIdAsync_ThrowsException_WhenCancellationRequested()
    {
        // Arrange
        var appointmentId = new AppointmentId(14);
        var startTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 10);
        var timeSlotBookedByPatient1 = new TimeSlot(new TimeSlotId(1), startTimeSlot, durationInMinutes: 30);
        var appointment = new Appointment(appointmentId, new PatientId(1), timeSlotBookedByPatient1);

        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel(); // Cancel the token

        // Act
        Func<Task> act = async () => await _sut.GetAppointmentByIdAsync(appointmentId, cancellationTokenSource.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
