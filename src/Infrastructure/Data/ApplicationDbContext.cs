using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
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
        public DbSet<DeviceReceiver> DeviceReceivers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Device> Devices { get; set; }

        // Aggregations
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<MaintenanceRecord> Mainteinances { get; set; }
        public DbSet<Decommissioning> Decommissionings { get; set; }
        public DbSet<DecommissioningRequest> DecommissioningRequests { get; set; }
        public DbSet<ReceivingInspectionRequest> ReceivingInspectionRequests { get; set; }
        public DbSet<Rejection> Rejections { get; set; }
        public DbSet<PerformanceRating> Assessments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar claves primarias para todas las entidades
            modelBuilder.Entity<PerformanceRating>()
                .HasKey(a => new { a.UserID, a.TechnicianID, a.Date });

            modelBuilder.Entity<Decommissioning>()
                .HasKey(d => d.DecommissioningRequestID);

            modelBuilder.Entity<DecommissioningRequest>()
                .HasKey(dr => new { dr.TechnicianID, dr.DeviceID, dr.Date });

            modelBuilder.Entity<ReceivingInspectionRequest>()
                .HasKey(r => new { r.DeviceID, r.AdministratorID, r.TechnicianID });

            modelBuilder.Entity<Rejection>()
                .HasKey(r => new { r.DeviceReceiverID, r.TechnicianID, r.DeviceID });

            modelBuilder.Entity<Transfer>()
                .HasKey(t => t.TransferID);

            modelBuilder.Entity<MaintenanceRecord>()
                .HasKey(m => new { m.TechnicianID, m.DeviceID, m.Date });

            // Configuraciones para User y sus herederos
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<Device>()
                .HasKey(e => e.DeviceID);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.DepartmentID);

            modelBuilder.Entity<Section>()
                .HasKey(s => s.SectionID);

            // Definir IDs constantes para las referencias (usando int en lugar de Guid)
            var deptTI = 1;
            var deptProduccion = 2;
            var deptMantenimiento = 3;
            var deptLogistica = 4;

            var sectDesarrollo = 1;
            var sectInfraestructura = 2;
            var sectEnsamblaje = 3;
            var sectControlCalidad = 4;
            var sectMantElectrico = 5;
            var sectMantMecanico = 6;

            // IDs para los usuarios
            var adminID = 1;
            var directorID = 2;
            var sectionManager1ID = 3;
            var sectionManager2ID = 4;
            var technician1ID = 5;
            var technician2ID = 6;
            var receiverID = 7;

            // IDs para equipos (devices)
            var device1ID = 1;
            var device2ID = 2;
            var device3ID = 3;
            var device4ID = 4;

            // IDs para agregaciones
            var decommissioningReqID = 1;

            // Fechas base para evitar errores
            var today = DateTime.Today;
            var dateInFifteenDays = today.AddDays(15);
            var dateInThreeDays = today.AddDays(3);
            var dateInTenDays = today.AddDays(-10);
            var dateInFifteenDaysBefore = today.AddDays(-15);
            var dateInFiveDaysBefore = today.AddDays(-5);
            var dateInTwentyDaysBefore = today.AddDays(-20);
            var dateInEighteenDaysBefore = today.AddDays(-18);

            // Seed Sections primero (ya que Department depende de Section)
            modelBuilder.Entity<Section>().HasData(
                new { SectionID = sectDesarrollo, Name = "Desarrollo de Software" },
                new { SectionID = sectInfraestructura, Name = "Infraestructura" },
                new { SectionID = sectEnsamblaje, Name = "Línea de Ensamblaje" },
                new { SectionID = sectControlCalidad, Name = "Control de Calidad" },
                new { SectionID = sectMantElectrico, Name = "Mantenimiento Eléctrico" },
                new { SectionID = sectMantMecanico, Name = "Mantenimiento Mecánico" }
            );

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new { DepartmentID = deptTI, Name = "Tecnologías de Información", SectionID = sectDesarrollo },
                new { DepartmentID = deptProduccion, Name = "Producción", SectionID = sectEnsamblaje },
                new { DepartmentID = deptMantenimiento, Name = "Mantenimiento", SectionID = sectMantElectrico },
                new { DepartmentID = deptLogistica, Name = "Logística", SectionID = sectControlCalidad }
            );

            // Seed Users
            // Administrator
            modelBuilder.Entity<Administrator>().HasData(
                new 
                {
                    UserID = adminID,
                    FullName = "Administrador Principal",
                    PasswordHash = "AdminHash123",
                    DepartmentID = deptTI
                }
            );

            // Director
            modelBuilder.Entity<Director>().HasData(
                new 
                {
                    UserID = directorID,
                    FullName = "Director General",
                    PasswordHash = "DirHash123",
                    DepartmentID = deptTI
                }
            );

            // SectionManagers
            modelBuilder.Entity<SectionManager>().HasData(
                new 
                {
                    UserID = sectionManager1ID,
                    FullName = "Gerente Desarrollo",
                    PasswordHash = "GerHash123",
                    DepartmentID = deptTI,
                    SectionID = sectDesarrollo
                },
                new 
                {
                    UserID = sectionManager2ID,
                    FullName = "Gerente Producción",
                    PasswordHash = "GerHash456",
                    DepartmentID = deptProduccion,
                    SectionID = sectEnsamblaje
                }
            );

            // Technicians
            modelBuilder.Entity<Technician>().HasData(
                new 
                {
                    UserID = technician1ID,
                    FullName = "Técnico Informática",
                    PasswordHash = "TecHash123",
                    DepartmentID = deptTI,
                    YearsOfExperience = 5,
                    Specialty = "Redes"
                },
                new 
                {
                    UserID = technician2ID,
                    FullName = "Técnico Eléctrico",
                    PasswordHash = "TecHash456",
                    DepartmentID = deptMantenimiento,
                    YearsOfExperience = 8,
                    Specialty = "Electricidad Industrial"
                }
            );

            // DeviceReceiver
            modelBuilder.Entity<DeviceReceiver>().HasData(
                new 
                {
                    UserID = receiverID,
                    FullName = "Receptor Equipos",
                    PasswordHash = "RecHash123",
                    DepartmentID = deptLogistica
                }
            );

            // Seed Devices
            modelBuilder.Entity<Device>().HasData(
                new 
                {
                    DeviceID = device1ID,
                    Name = "Servidor Principal",
                    Type = DeviceType.ComputingAndIT,
                    OperationalState = OperationalState.Operational,
                    DepartmentID = deptTI,
                    AcquisitionDate = today.AddYears(-2)
                },
                new 
                {
                    DeviceID = device2ID,
                    Name = "Computadora Desarrollo",
                    Type = DeviceType.ComputingAndIT,
                    OperationalState = OperationalState.Operational,
                    DepartmentID = deptTI,
                    AcquisitionDate = today.AddMonths(-8)
                },
                new 
                {
                    DeviceID = device3ID,
                    Name = "Máquina Ensamblaje",
                    Type = DeviceType.ElectricalInfrastructureAndSupport,
                    OperationalState = OperationalState.UnderMaintenance,
                    DepartmentID = deptProduccion,
                    AcquisitionDate = today.AddYears(-1)
                },
                new 
                {
                    DeviceID = device4ID,
                    Name = "Sistema de Comunicación",
                    Type = DeviceType.CommunicationsAndTransmission,
                    OperationalState = OperationalState.Operational,
                    DepartmentID = deptLogistica,
                    AcquisitionDate = today.AddMonths(-5)
                }
            );

            // Seed MaintenanceRecord
            modelBuilder.Entity<MaintenanceRecord>().HasData(
                new 
                {
                    TechnicianID = technician1ID,
                    DeviceID = device1ID,
                    Date = DateOnly.FromDateTime(dateInFifteenDays),
                    Cost = 500.00,
                    Type = "Preventivo"
                },
                new 
                {
                    TechnicianID = technician2ID,
                    DeviceID = device3ID,
                    Date = DateOnly.FromDateTime(dateInThreeDays),
                    Cost = 1200.00,
                    Type = "Correctivo"
                }
            );

            // Seed DecommissioningRequest
            modelBuilder.Entity<DecommissioningRequest>().HasData(
                new 
                {
                    DecommissioningRequestID = decommissioningReqID,
                    TechnicianID = technician2ID,
                    DeviceID = device3ID,
                    Date = dateInFifteenDaysBefore,
                    DeviceReceiverID = receiverID
                }
            );

            // Seed Decommissioning
            modelBuilder.Entity<Decommissioning>().HasData(
                new 
                {
                    DecommissioningID = 1,
                    DecommissioningRequestID = decommissioningReqID,
                    DeviceReceiverID = receiverID,
                    DeviceID = device3ID,
                    DecommissioningDate = dateInFiveDaysBefore,
                    Reason = DecommissioningReason.TechnologicalObsolescence,
                    FinalDestination = "Donación a institución educativa",
                    ReceiverDepartmentID = deptProduccion
                }
            );

            // Seed ReceivingInspectionRequest
            modelBuilder.Entity<ReceivingInspectionRequest>().HasData(
                new 
                {
                    ReceivingInspectionRequestID = 1,
                    DeviceID = device2ID,
                    AdministratorID = adminID,
                    TechnicianID = technician1ID,
                    EmissionDate = dateInTwentyDaysBefore,
                    AcceptedDate = (DateTime?)dateInEighteenDaysBefore,
                    RejectionDate = (DateTime?)null
                }
            );

            // Seed Rejection
            modelBuilder.Entity<Rejection>().HasData(
                new 
                {
                    RejectionID = 1,
                    DeviceReceiverID = receiverID,
                    TechnicianID = technician1ID,
                    DeviceID = device4ID,
                    DecommissioningRequestDate = dateInTwentyDaysBefore,
                    RejectionDate = dateInFifteenDaysBefore
                }
            );

            // Seed PerformanceRating
            modelBuilder.Entity<PerformanceRating>().HasData(
                new 
                {
                    UserID = directorID,
                    TechnicianID = technician1ID,
                    Date = dateInTwentyDaysBefore,
                    Score = 4.5
                }
            );
        }
    }
}
