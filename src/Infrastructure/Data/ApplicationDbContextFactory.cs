using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
     
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Use SQLite instead of SQL Server
            optionsBuilder.UseSqlite(
                "Data Source=InfraGestionDb.sqlite");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
