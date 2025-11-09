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

            builder.HasKey(dr => dr.DecommissioningRequestId);

            builder.Property(dr => dr.DecommissioningRequestId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(dr => new { dr.TechnicianId, dr.DeviceId, dr.Date })
                .IsUnique();

            builder.Property(dr => dr.Date)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(dr => dr.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(dr => dr.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(dr => dr.DeviceReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
