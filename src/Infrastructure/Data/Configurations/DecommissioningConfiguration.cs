using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DecommissioningConfiguration : IEntityTypeConfiguration<Decommissioning>
    {
        public void Configure(EntityTypeBuilder<Decommissioning> builder)
        {
            builder.ToTable("Decommissioning");

            builder.HasKey(d => d.DecommissioningID);

            builder.Property(d => d.DecommissioningID)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.FinalDestination)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.DecommissioningDate)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(d => d.DeviceReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(d => d.DeviceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Department>()
                .WithMany()
                .HasForeignKey(d => d.ReceiverDepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
