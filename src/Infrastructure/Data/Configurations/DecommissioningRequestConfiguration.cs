using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DecommissioningRequestConfiguration : IEntityTypeConfiguration<DecommissioningRequest>
    {
        public void Configure(EntityTypeBuilder<DecommissioningRequest> builder)
        {
            builder.ToTable("DecommissioningRequests");

            builder.HasKey(dr => new { dr.TechnicianID, dr.DeviceID, dr.Date });

            builder.Property(dr => dr.Date)
                .IsRequired();

            builder.HasOne<Domain.Entities.Technician>()
                .WithMany()
                .HasForeignKey(dr => dr.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(dr => dr.DeviceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.DeviceReceiver>()
                .WithMany()
                .HasForeignKey(dr => dr.DeviceReceiverID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
