using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SectionManagerConfiguration : IEntityTypeConfiguration<SectionManager>
    {
        public void Configure(EntityTypeBuilder<SectionManager> builder)
        {
            builder.HasOne<Section>()
                .WithMany()
                .HasForeignKey(sm => sm.SectionID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
