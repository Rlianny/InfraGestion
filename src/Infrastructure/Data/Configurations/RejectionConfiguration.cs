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

            builder.HasKey(r => r.RejectionID);

            builder.Property(r => r.RejectionID)
                .ValueGeneratedOnAdd();

            builder.HasIndex(r => new { r.DeviceReceiverID, r.TechnicianID, r.DeviceID, r.DecommissioningRequestDate })
                .IsUnique();

            builder.Property(r => r.DecommissioningRequestDate)
                .IsRequired();

            builder.Property(r => r.RejectionDate)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(r => r.DeviceReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(r => r.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(r => r.DeviceID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
