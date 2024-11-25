using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    public DbSet<Client> Clients { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<TimeSlot> TimeSlots { get; set; }

    public void Seed()
    {
        if (!TimeSlots.Any())
        {
            var seedData = GenerateTimeSlots(
                startDate: DateTime.Now.Date.AddHours(8),
                numberOfDays: 7,
                intervalMinutes: 30,
                durationMinutes: 30,
                workingHoursStart: new TimeSpan(8, 0, 0),
                workingHoursEnd: new TimeSpan(18, 0, 0)
            );

            TimeSlots.AddRange(seedData);
            SaveChanges();
        }
    }

    private List<TimeSlot> GenerateTimeSlots(
        DateTime startDate,
        int numberOfDays,
        int intervalMinutes,
        int durationMinutes,
        TimeSpan workingHoursStart,
        TimeSpan workingHoursEnd)
    {
        var timeSlots = new List<TimeSlot>();
        var currentDate = startDate.Date;
        var idCounter = 1;

        for (int day = 0; day < numberOfDays; day++)
        {
            var currentStartTime = currentDate.Add(workingHoursStart);
            var dayEndTime = currentDate.Add(workingHoursEnd);

            while (currentStartTime < dayEndTime)
            {
                timeSlots.Add(new TimeSlot(
                    id: new TimeSlotId(idCounter++),
                    startTime: currentStartTime,
                    durationInMinutes: durationMinutes,
                    isAvailable: true));

                currentStartTime = currentStartTime.AddMinutes(intervalMinutes);
            }

            currentDate = currentDate.AddDays(1);
        }

        return timeSlots;
    }
}
