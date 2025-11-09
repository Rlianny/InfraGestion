using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.DepartmentId)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.DepartmentId)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.SectionId);
            builder.HasIndex(d => d.SectionId);
            //builder.HasOne<Section>()
            //    .WithMany()
            //    .HasForeignKey(d => d.SectionId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
