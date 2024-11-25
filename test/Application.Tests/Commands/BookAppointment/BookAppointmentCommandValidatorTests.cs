using Application.Commands.BookAppointment;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using SharedKernel.Tests;

namespace Application.Tests.Commands.BookAppointment;

public partial class BookAppointmentCommandTests
{
    [Fact(DisplayName = "BookAppointmentCommand with invalid PatientId format")]
    public async Task WithInvalidPatientId_Should_ReturnsError()
    {
        //Arrange
        var command = new BookAppointmentCommand(null!, new TimeSlot(DateTime.Now.AddMinutes(10), 30));

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.FirstOrDefault()!.Code.Should().Be(AppointmentErrorCodes.InvalidPatientId);
        result.Errors.FirstOrDefault()!.Description.Should().Be(AppointmentErrorMessages.InvalidPatientId);
    }

    [Fact(DisplayName = "BookAppointmentCommand with invalid none existing patient")]
    public async Task WithNoneExistingPatient_Should_ReturnsError()
    {
        //Arrange
        var command = new BookAppointmentCommand(new PatientId(100), new TimeSlot(DateTime.Now.AddMinutes(4), 60));

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.FirstOrDefault()!.Code.Should().Be(AppointmentErrorCodes.InvalidPatientId);
        result.Errors.FirstOrDefault()!.Description.Should().Be(AppointmentErrorMessages.NotFoundPatientToBookAppointment);
    }

    [Fact(DisplayName = "BookAppointmentCommand with booked time slot")]
    public async Task WithBookedTimeSlot_Should_ReturnsError()
    {
        //Arrange
        var patient1 = new Patient(new PatientId(1), new ClientId(1), "Diesel", "Chien", DateTime.Now.AddYears(-1));
        var patient2 = new Patient(new PatientId(2), new ClientId(1), "Smokie", "Chat", DateTime.Now.AddYears(-3));

        var startTimeSlot = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 10);
        var timeSlotBookedByPatient1 = new TimeSlot(new TimeSlotId(1), startTimeSlot, durationInMinutes: 30);
        var scheduledAppointmentForPatient1 = new Appointment(patient1.Id, timeSlotBookedByPatient1);

        await _dbContext.Patients.AddAsync(patient1);
        await _dbContext.Patients.AddAsync(patient2);
        await _dbContext.AddAsync(scheduledAppointmentForPatient1);
        await _dbContext.SaveChangesAsync();

        var startTimeSlotForAnOtherAppointment = DataFixtureGenerator.GenerateStartTimeSlot(startTimeInHours: 1, startTimeInMinutes: 20);
        var timeSlotForPatient2 = new TimeSlot(new TimeSlotId(2), startTimeSlotForAnOtherAppointment, durationInMinutes: 30);

        var command = new BookAppointmentCommand(patient2.Id, timeSlotForPatient2);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.FirstOrDefault()!.Code.Should().Be(AppointmentErrorCodes.InvalidTimeSlot);
        result.Errors.FirstOrDefault()!.Description.Should().Be(AppointmentErrorMessages.CannotBookAppointmentForBookedTimeSlot);
    }
}
