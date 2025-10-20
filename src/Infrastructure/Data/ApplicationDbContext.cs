using Domain.Aggregations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<SectionManager> SectionManagers { get; set; }
        public DbSet<EquipmentReceiver> EquipmentReceivers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

        // Aggregations
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Mainteinance> Mainteinances { get; set; }
        public DbSet<Decommissioning> Decommissionings { get; set; }
        public DbSet<DecommissioningRequest> DecommissioningRequests { get; set; }
        public DbSet<ReceivingInspectionRequest> ReceivingInspectionRequests { get; set; }
        public DbSet<Rejection> Rejections { get; set; }
        public DbSet<Assessments> Assessments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
