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

            builder.HasKey(e => e.DeviceID);

            builder.Property(e => e.DeviceID)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.DeviceID)
                .ValueGeneratedOnAdd();

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

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
