using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.ToTable("Transfers");

            builder.HasKey(t => t.TransferId);

            builder.Property(d => d.TransferId)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Date)
                .IsRequired();

            builder.HasOne<Domain.Entities.Device>()
                .WithMany()
                .HasForeignKey(t => t.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Section>()
                .WithMany()
                .HasForeignKey(t => t.SourceSectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Section>()
                .WithMany()
                .HasForeignKey(t => t.DestinationSectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(t => t.DeviceReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
