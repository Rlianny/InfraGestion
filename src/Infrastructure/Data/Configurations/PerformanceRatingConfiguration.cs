using Domain.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PerformanceRatingConfiguration : IEntityTypeConfiguration<PerformanceRating>
    {
        public void Configure(EntityTypeBuilder<PerformanceRating> builder)
        {
            builder.ToTable("PerformanceRatings");

            builder.HasKey(a => new { a.UserID, a.TechnicianID, a.Date });

            builder.Property(a => a.PerformanceRatingID)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Score)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(a => a.Date)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany()
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Entities.Technician>()
                .WithMany()
                .HasForeignKey(a => a.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
