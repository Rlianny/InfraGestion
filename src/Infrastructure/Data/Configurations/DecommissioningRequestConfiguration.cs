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

            builder.HasIndex(dr => new { dr.TechnicianId, dr.DeviceId, dr.EmissionDate })
                .IsUnique();

            builder.Property(dr => dr.EmissionDate)
                .IsRequired();

            builder.Property(dr => dr.AnswerDate)
                .IsRequired(false);

            builder.Property(dr => dr.Status)
                .IsRequired();

            builder.Property(dr => dr.Reason)
                .IsRequired();

            builder.Property(dr => dr.IsApproved)
                .IsRequired(false);

            builder.Property(dr => dr.description)
                .IsRequired()
                .HasMaxLength(2000);

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

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(dr => dr.logisticId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Department>()
                .WithMany()
                .HasForeignKey(dr => dr.FinalDestinationDepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
