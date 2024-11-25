using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.ToTable(nameof(TimeSlot));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Id).HasConversion(
            clientId => clientId.Value,
            value => new TimeSlotId(value));

        builder.Property(a => a.StartTime).IsRequired();

        builder.Property(a => a.DurationInMinutes).IsRequired();
    }
}
