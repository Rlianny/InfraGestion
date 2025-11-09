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

            builder.HasKey(d => d.DepartmentID);

            builder.Property(d => d.DepartmentID)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.DepartmentID)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.SectionID);
            builder.HasIndex(d => d.SectionID);
            //builder.HasOne<Section>()
            //    .WithMany()
            //    .HasForeignKey(d => d.SectionID)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
