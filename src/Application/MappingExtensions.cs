using Application.Commands.BookAppointment;
using Application.Commands.CreateClient;
using Application.Commands.CreatePatient;
using Application.Dtos;
using Application.Queries;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application;

public static class MappingExtensions
{
    public static Client MapToClient(this CreateClientCommand request)
    {
        return new Client(
            name: request.Name,
            contactInfo: new ContactInfo(request.PhoneNumber, request.Email));
    }

    public static Patient MapToPatient(this CreatePatientCommand request)
    {
        return new Patient(
            clientId: request.ClientId,
            name: request.Name,
            species: request.Species,
            dateOfBirth: request.DateOfBirth);
    }

    public static Appointment MapToAppointment(this BookAppointmentCommand request)
    {
        return new Appointment(
            patientId: request.PatientId,
            timeSlot: request.TimeSlot);
    }

    public static TimeSlotCollectionDto MapToTimeSlotCollectionDto(this IEnumerable<TimeSlot> timeSlots)
    {
        return new TimeSlotCollectionDto(
            count: timeSlots.Count(),
            Items: timeSlots.Select( t => new TimeSlotDto(t.Id, t.StartTime, t.EndTime, t.IsAvailable)));
    }
}
