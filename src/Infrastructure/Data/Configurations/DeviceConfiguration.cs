using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("Devices");

            builder.HasKey(e => e.DeviceId);

            builder.Property(e => e.DeviceId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.IsDisabled);

            builder.Property(e => e.DepartmentId)
                .IsRequired();
            
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.OperationalState)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.AcquisitionDate)
                .IsRequired();

        }
    }
}