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

            builder.HasKey(t => t.TransferID);

            builder.Property(t => t.Date)
                .IsRequired();

            builder.HasOne<Domain.Entities.Equipment>()
                .WithMany()
                .HasForeignKey(t => t.EquipmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Section>()
                .WithMany()
                .HasForeignKey(t => t.SourceSectionID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Section>()
                .WithMany()
                .HasForeignKey(t => t.DestinySectionID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.DeviceReceiver>()
                .WithMany()
                .HasForeignKey(t => t.EquipmentReceiverID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
