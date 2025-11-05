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

            builder.HasKey(dr => dr.DecommissioningRequestID);

            builder.Property(dr => dr.DecommissioningRequestID)
                .ValueGeneratedOnAdd();

            builder.HasIndex(dr => new { dr.TechnicianID, dr.DeviceID, dr.Date })
                .IsUnique();

            builder.Property(dr => dr.Date)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(dr => dr.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(dr => dr.DeviceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(dr => dr.DeviceReceiverID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
