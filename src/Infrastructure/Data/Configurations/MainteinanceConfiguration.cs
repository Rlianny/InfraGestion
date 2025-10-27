using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class MainteinanceConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
        {
            builder.ToTable("Mainteinances");

            builder.HasKey(m => new { m.TechnicianID, m.DeviceID, m.Date });

            builder.Property(m => m.Cost)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(m => m.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<Domain.Entities.Technician>()
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
