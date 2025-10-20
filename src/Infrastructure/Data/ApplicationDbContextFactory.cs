using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    /// <summary>
    /// Factory for creating DbContext instances at design-time (for migrations).
    /// This is required for EF Core tools to create migrations.
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Use a connection string for design-time operations
            // This can be updated with your actual connection string
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=InfraGestionDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
