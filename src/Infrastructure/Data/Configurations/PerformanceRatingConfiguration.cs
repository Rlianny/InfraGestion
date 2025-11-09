using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PerformanceRatingConfiguration : IEntityTypeConfiguration<PerformanceRating>
    {
        public void Configure(EntityTypeBuilder<PerformanceRating> builder)
        {
            builder.ToTable("PerformanceRatings");

            builder.HasKey(a => a.PerformanceRatingID);

            builder.Property(a => a.PerformanceRatingID)
                .ValueGeneratedOnAdd();

            builder.HasIndex(a => new { a.UserID, a.TechnicianID, a.Date })
                .IsUnique();

            builder.Property(a => a.Score)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(a => a.Date)
                .IsRequired();

            
        //    builder.HasOne<Domain.Entities.User>()
        //        .WithMany()
        //        .HasForeignKey(a => a.UserID)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.HasOne<Domain.Entities.User>()
        //        .WithMany()
        //        .HasForeignKey(a => a.TechnicianID)
        //        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
