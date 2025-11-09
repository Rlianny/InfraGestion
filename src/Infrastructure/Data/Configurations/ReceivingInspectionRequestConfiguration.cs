using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReceivingInspectionRequestConfiguration : IEntityTypeConfiguration<ReceivingInspectionRequest>
    {
        public void Configure(EntityTypeBuilder<ReceivingInspectionRequest> builder)
        {
            builder.ToTable("ReceivingInspectionRequests");

            builder.HasKey(rir => rir.ReceivingInspectionRequestId);

            builder.Property(rir => rir.ReceivingInspectionRequestId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(rir => new { rir.DeviceId, rir.AdministratorId, rir.TechnicianId, rir.EmissionDate })
                .IsUnique();

            builder.Property(rir => rir.EmissionDate)
                .IsRequired();

            builder.Property(rir => rir.AcceptedDate)
                .IsRequired(false);

            builder.Property(rir => rir.RejectionDate)
                .IsRequired(false);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(rir => rir.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(rir => rir.AdministratorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(rir => rir.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
