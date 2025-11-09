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

            builder.HasKey(a => a.PerformanceRatingId);

            builder.Property(a => a.PerformanceRatingId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(a => new { a.UserId, a.TechnicianId, a.Date })
                .IsUnique();

            builder.Property(a => a.Score)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(a => a.Date)
                .IsRequired();

            
        //    builder.HasOne<Domain.Entities.User>()
        //        .WithMany()
        //        .HasForeignKey(a => a.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    builder.HasOne<Domain.Entities.User>()
        //        .WithMany()
        //        .HasForeignKey(a => a.TechnicianId)
        //        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
