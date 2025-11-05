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

            builder.HasKey(m => m.MaintenanceRecordID);
            builder.Property(m => m.MaintenanceRecordID)
                .ValueGeneratedOnAdd();

            builder.HasIndex(m => new { m.TechnicianID, m.DeviceID, m.Date })
                .IsUnique();

            builder.Property(m => m.Cost)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(m => m.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(m => m.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(m => m.DeviceID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
