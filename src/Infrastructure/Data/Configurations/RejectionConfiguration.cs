using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RejectionConfiguration : IEntityTypeConfiguration<Rejection>
    {
        public void Configure(EntityTypeBuilder<Rejection> builder)
        {
            builder.ToTable("Rejections");

            builder.HasKey(r => new { r.EquipmentReceiverID, r.TechnicianID, r.EquipmentID, r.DecommissioningRequestDate });

            builder.Property(r => r.DecommissioningRequestDate)
                .IsRequired();

            builder.Property(r => r.RejectionDate)
                .IsRequired();

            builder.HasOne<Domain.Entities.DeviceReceiver>()
                .WithMany()
                .HasForeignKey(r => r.EquipmentReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Technician>()
                .WithMany()
                .HasForeignKey(r => r.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Equipment>()
                .WithMany()
                .HasForeignKey(r => r.EquipmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
