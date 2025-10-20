using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.UserID);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(u => u.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TPH (Table Per Hierarchy) inheritance
            builder.HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Administrator>("Administrator")
                .HasValue<Director>("Director")
                .HasValue<Technician>("Technician")
                .HasValue<SectionManager>("SectionManager")
                .HasValue<EquipmentReceiver>("EquipmentReceiver");
        }
    }
}
