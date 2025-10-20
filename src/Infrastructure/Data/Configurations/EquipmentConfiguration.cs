using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipments");

            builder.HasKey(e => e.EquipmentID);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.OperationalState)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.AcquisitionDate)
                .IsRequired();

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
