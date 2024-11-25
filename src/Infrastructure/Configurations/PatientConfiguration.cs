using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.ValueObjects.Identifiers;

namespace Infrastructure.Configurations;
public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable(nameof(Patient));

        builder.Property(p => p.Id).HasConversion(
            patientId => patientId.Value,
            value => new PatientId(value));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedOnAdd();        

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .HasConstraintName("FK_Patient_Client")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnType("[varchar](100)");

        builder.Property(p => p.Species)
            .IsRequired()
            .HasColumnType("[varchar](100)");

        builder.Property(p => p.DateOfBirth)
            .IsRequired();
    }
}
