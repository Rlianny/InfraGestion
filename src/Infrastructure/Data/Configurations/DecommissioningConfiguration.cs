using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DecommissioningConfiguration : IEntityTypeConfiguration<Decommissioning>
    {
        public void Configure(EntityTypeBuilder<Decommissioning> builder)
        {
            builder.ToTable("Decommissionings");

            builder.HasKey(d => d.DecommissioningRequestID);

            builder.Property(d => d.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.FinalDestination)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.DecommissioningDate)
                .IsRequired();

            builder.Property(d => d.RequestDate)
                .IsRequired();

            builder.HasOne<Domain.Entities.DeviceReceiver>()
                .WithMany()
                .HasForeignKey(d => d.DeviceReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Equipment>()
                .WithMany()
                .HasForeignKey(d => d.EquipmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Department>()
                .WithMany()
                .HasForeignKey(d => d.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
