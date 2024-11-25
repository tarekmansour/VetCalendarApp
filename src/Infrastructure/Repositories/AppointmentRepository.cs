using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<TimeSlot>> GetTimeSlotsByAvailabilityAsync(bool isAvailable, CancellationToken cancellationToken)
    {
        return await _dbContext.TimeSlots
            .Where(ts => ts.IsAvailable == isAvailable)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppointmentId> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        await _dbContext.Appointments.AddAsync(appointment, cancellationToken);
        return appointment.Id;
    }

    public async Task<bool> ExistsPatientAsync(PatientId patientId, CancellationToken cancellationToken)
    {
        var patient = await _dbContext.Patients.FindAsync(patientId, cancellationToken);
        return patient != null;
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(AppointmentId appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Appointments
            .Where(a => a.Id == appointmentId)
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(cancellationToken);
    }


}