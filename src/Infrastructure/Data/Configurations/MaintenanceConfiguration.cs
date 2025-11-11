using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class MaintenanceConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
        {
            builder.ToTable("Maintenances");

            builder.HasKey(m => m.MaintenanceRecordId);
            builder.Property(m => m.MaintenanceRecordId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(m => new { m.TechnicianId, m.DeviceId, m.Date })
                .IsUnique();

            builder.Property(m => m.Cost)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(m => m.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(m => m.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(m => m.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
