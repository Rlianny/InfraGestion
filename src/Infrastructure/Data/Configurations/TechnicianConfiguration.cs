using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TechnicianConfiguration : IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> builder)
        {
            builder.Property(t => t.YearsOfExperience)
                .IsRequired();

            builder.Property(t => t.Specialty)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
