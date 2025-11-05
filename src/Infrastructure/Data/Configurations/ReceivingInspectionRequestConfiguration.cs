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

            builder.HasKey(rir => rir.ReceivingInspectionRequestID);

            builder.Property(rir => rir.ReceivingInspectionRequestID)
                .ValueGeneratedOnAdd();

            builder.HasIndex(rir => new { rir.DeviceID, rir.AdministratorID, rir.TechnicianID, rir.EmissionDate })
                .IsUnique();

            builder.Property(rir => rir.EmissionDate)
                .IsRequired();

            builder.Property(rir => rir.AcceptedDate)
                .IsRequired(false);

            builder.Property(rir => rir.RejectionDate)
                .IsRequired(false);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(rir => rir.DeviceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(rir => rir.AdministratorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(rir => rir.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
