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

            builder.HasKey(r => r.RejectionId);

            builder.Property(r => r.RejectionId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(r => new { r.DeviceReceiverId, r.TechnicianId, r.DeviceId, r.DecommissioningRequestDate })
                .IsUnique();

            builder.Property(r => r.DecommissioningRequestDate)
                .IsRequired();

            builder.Property(r => r.RejectionDate)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(r => r.DeviceReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(r => r.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(r => r.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
