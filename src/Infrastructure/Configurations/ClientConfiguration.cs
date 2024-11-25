using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable(nameof(Client));

        builder.Property(c => c.Id).HasConversion(
            clientId => clientId.Value,
            value => new ClientId(value));

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("[varchar](100)");

        builder.OwnsOne(c => c.ContactInfo, contactInfoBuilder =>
        {
            contactInfoBuilder.Property(ci => ci.Email)
                .HasColumnType("[varchar](100)")
                .IsRequired();
            contactInfoBuilder.Property(ci => ci.PhoneNumber)
                .HasColumnType("[varchar](15)")
                .IsRequired();
        });

        builder.HasMany(c => c.Patients)
            .WithOne()
            .HasForeignKey(p => p.ClientId);
    }
}
