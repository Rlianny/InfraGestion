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
            builder.Property(d => d.IsDisabled)
                .HasDefaultValue(false);
            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.DepartmentId)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.IsDisabled);
            builder.Property(d => d.SectionId)
                .IsRequired();

            builder.HasOne(d => d.Section)
                .WithMany()
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasMany(d => d.Staff)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}