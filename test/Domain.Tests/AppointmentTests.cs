using Domain.Entities;
using Domain.Errors;
using Domain.Exceptions;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using SharedKernel.Tests;

namespace Domain.Tests;

public class AppointmentTests
{
    [Fact(DisplayName = "Book an appointment")]
    public void New_Appointment()
    {
        // Arrange
        var patientId = new PatientId(10);
        var timeSlot = new TimeSlot(
            startTime: DateTime.Now.AddDays(3),
            durationInMinutes: 30);

        // Act
        var newAppointment = new Appointment(patientId, timeSlot);

        // Assert
        newAppointment.Status.Should().Be(AppointmentStatus.Scheduled);        
        newAppointment.TimeSlot.DurationInMinutes.Should().Be(30);        
    }

    [Fact(DisplayName = "Book an appointment with invalid time slot in the past")]
    public void NewAppointment_With_InvalidTimeSlot()
    {
        // Arrange
        Func<TimeSlot> act = () => new TimeSlot(
            startTime: DateTime.Now.AddDays(-1),
            durationInMinutes: 0);

        // Act & Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact(DisplayName = "Cancel an appointment")]
    public void Cancel_Appointment()
    {
        // Arrange
        var patientId = new PatientId(10);
        var timeSlot = new TimeSlot(
            startTime: DateTime.Now.AddDays(3),
            durationInMinutes: 30);

        var appointment = new Appointment(patientId, timeSlot);

        // Act
        appointment.Cancel();

        // Assert
        appointment.Status.Should().Be(AppointmentStatus.Cancelled);
        appointment.TimeSlot.IsAvailable.Should().BeTrue();
    }

    [Fact(DisplayName = "Cancel an appointment which already cancelled")]
    public void Cancel_Appointment_AlreadyCancelled()
    {
        // Arrange
        var patientId = new PatientId(10);
        var timeSlot = new TimeSlot(
            startTime: DateTime.Now.AddDays(3),
            durationInMinutes: 30);

        var timeSlotId = new TimeSlotId(1);

        var appointment = new Appointment(patientId, timeSlot);
        appointment.Cancel();

        // Act
        Action act = () => appointment.Cancel();

        // Assert
        act.Should().Throw<AppointmentException>()
            .WithMessage(AppointmentErrorMessages.AlreadyCancelled);
    }

    [Fact(DisplayName = "Reschedule a cancelled appointment")]
    public void Reschedule_Cancelled_Appointment()
    {
        // Arrange
        var startTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 7, startTimeInMinutes: 20);
        var scheduledAppointment = new Appointment(new PatientId(5), new TimeSlot(new TimeSlotId(1), startTimeSlot, durationInMinutes: 30));
        scheduledAppointment.Cancel();

        // Act
        var newStartTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 6, startTimeInMinutes: 20);
        Action act = () => scheduledAppointment.Reschedule(new TimeSlot(newStartTimeSlot, 30));

        // Assert
        act.Should().Throw<AppointmentException>()
            .WithMessage(AppointmentErrorMessages.CannotRescheduleCancelled);
    }
    
}
