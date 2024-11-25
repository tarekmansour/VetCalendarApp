using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable(nameof(Appointment));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Id).HasConversion(
            clientId => clientId.Value,
            value => new AppointmentId(value));

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .HasConstraintName("FK_Appointment_Patient")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(a => a.TimeSlot) 
            .WithOne()
            .HasForeignKey<Appointment>(a => a.TimeSlotId)
            .IsRequired();

        builder.OwnsOne(a => a.Status, appointmentStatusBuilder =>
        {
            appointmentStatusBuilder.Property(s => s.Value).HasColumnType("[varchar](20)");
        });
    }
}
