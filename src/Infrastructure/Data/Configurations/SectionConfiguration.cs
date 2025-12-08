using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Sections");

            builder.HasKey(s => s.SectionId);

            builder.Property(d => d.SectionId)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.SectionId)
                .ValueGeneratedOnAdd();
            builder.Property(s => s.IsDisabled);
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

             builder.Property(s => s.SectionManagerId)
                .IsRequired(false);
        }
    }
}
