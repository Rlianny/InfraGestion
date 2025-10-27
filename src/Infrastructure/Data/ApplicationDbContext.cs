using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
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


            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<Device>()
                .HasKey(e => e.DeviceID);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.DepartmentID);

            modelBuilder.Entity<Section>()
                .HasKey(s => s.SectionID);

            // Dates
            var today = DateTime.Today;
            var dateInFifteenDays = today.AddDays(15);
            var dateInThreeDays = today.AddDays(3);
            var dateInTenDays = today.AddDays(-10);
            var dateInFifteenDaysBefore = today.AddDays(-15);
            var dateInFiveDaysBefore = today.AddDays(-5);
            var dateInTwentyDaysBefore = today.AddDays(-20);
            var dateInEighteenDaysBefore = today.AddDays(-18);
            var dateInSevenDaysBefore = today.AddDays(-7);


            // Creating seed data

            // Seed Sections
            modelBuilder.Entity<Section>().HasData(
                new { SectionID = -1, Name = "Operaciones de Red Corporativa" },
                new { SectionID = -2, Name = "Infraestructura de Centro de Datos (Data Center)" },
                new { SectionID = -3, Name = "Soporte Técnico en Campo" },
                new { SectionID = -4, Name = "Planificación y Despliegue de Red" },
                new { SectionID = -5, Name = "División de Servicios en la Nube (Cloud)" },
                new { SectionID = -6, Name = "Taller Central y Logística" },
                new { SectionID = -7, Name = "Infraestructura Interna (TI Interno)" },
                new { SectionID = -8, Name = "Seguridad Informática (Ciberseguridad)" }
            );

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new { DepartmentID = -1, Name = "Conmutación y Enrutamiento Avanzado", SectionID = -1 },
                new { DepartmentID = -2, Name = "Seguridad Perimetral y Firewalls", SectionID = -1 },
                new { DepartmentID = -3, Name = "Reparaciones de Red", SectionID = -1 },

                new { DepartmentID = -4, Name = "Servidores y Virtualización", SectionID = -2 },
                new { DepartmentID = -5, Name = "Almacenamiento y Backup", SectionID = -2 },
                new { DepartmentID = -6, Name = "Infraestructura Física y Climatización", SectionID = -2 },

                new { DepartmentID = -7, Name = "Instalaciones y Activaciones", SectionID = -3 },
                new { DepartmentID = -8, Name = "Mantenimiento Correctivo y Urgencias", SectionID = -3 },
                new { DepartmentID = -9, Name = "Soporte a Nodos Remotos", SectionID = -3 },

                new { DepartmentID = -10, Name = "Diseño y Ingeniería de Red", SectionID = -4 },
                new { DepartmentID = -11, Name = "Despliegue de Fibra Óptica y Acceso", SectionID = -4 },
                new { DepartmentID = -12, Name = "Mediciones y Certificación de Red", SectionID = -4 },

                new { DepartmentID = -13, Name = "Infraestructura como Servicio", SectionID = -5 },
                new { DepartmentID = -14, Name = "Plataforma como Servicio", SectionID = -5 },
                new { DepartmentID = -15, Name = "Operaciones Cloud y Escalabilidad", SectionID = -5 },

                new { DepartmentID = -16, Name = "Recepción y Diagnóstico Técnico", SectionID = -6 },
                new { DepartmentID = -17, Name = "Reparación y Refabricación", SectionID = -6 },
                new { DepartmentID = -18, Name = "Gestión de Inventario y Distribución", SectionID = -6 },

                new { DepartmentID = -19, Name = "Soporte al Usuario y Helpdesk", SectionID = -7 },
                new { DepartmentID = -20, Name = "Comunicaciones Unificadas y Telefonía IP", SectionID = -7 },
                new { DepartmentID = -21, Name = "Gestión de Activos y Red Local", SectionID = -7 },

                new { DepartmentID = -22, Name = "Arquitectura y Gestión de Firewalls", SectionID = -8 },
                new { DepartmentID = -23, Name = "Monitorización de Amenazas y SOC", SectionID = -8 },
                new { DepartmentID = -24, Name = "Análisis Forense y Respuesta a Incidentes", SectionID = -8 }
            );

            // Seed Users - Administrator
            modelBuilder.Entity<Administrator>().HasData(
                new { UserID = -1, FullName = "David González", PasswordHash = "admin01", DepartmentID = -21 },
                new { UserID = -2, FullName = "Laura Martínez", PasswordHash = "admin02", DepartmentID = -18 },
                new { UserID = -3, FullName = "Javier Rodríguez", PasswordHash = "admin03", DepartmentID = -18 },
                new { UserID = -4, FullName = "Carmen Sánchez", PasswordHash = "admin04", DepartmentID = -5 },
                new { UserID = -5, FullName = "Roberto López", PasswordHash = "admin05", DepartmentID = -3 }
            );

            // Director
            modelBuilder.Entity<Director>().HasData(
                new { UserID = -6, FullName = "Elena Morales", PasswordHash = "dir123", DepartmentID = -24 }
            );

            // SectionManagers
            modelBuilder.Entity<SectionManager>().HasData(
                new { UserID = -7, FullName = "Sofía Ramírez", PasswordHash = "manager01", DepartmentID = -1, SectionID = -1 },
                new { UserID = -8, FullName = "Alejandro Torres", PasswordHash = "manager02", DepartmentID = -6, SectionID = -2 },
                new { UserID = -9, FullName = "Patricia Herrera", PasswordHash = "manager03", DepartmentID = -7, SectionID = -3 },
                new { UserID = -10, FullName = "Ricardo Díaz", PasswordHash = "manager04", DepartmentID = -10, SectionID = -4 },
                new { UserID = -11, FullName = "Isabel Castro", PasswordHash = "manager05", DepartmentID = -13, SectionID = -5 }
            );

            // Technicians
            modelBuilder.Entity<Technician>().HasData(
                new { UserID = -12, FullName = "Carlos Méndez", PasswordHash = "tech01", DepartmentID = -3, YearsOfExperience = 5, Specialty = "Redes y Comunicaciones" },
                new { UserID = -13, FullName = "Eduardo Vargas", PasswordHash = "tech02", DepartmentID = -6, YearsOfExperience = 3, Specialty = "Servidores y Virtualización" },
                new { UserID = -14, FullName = "Jorge Silva", PasswordHash = "tech03", DepartmentID = -9, YearsOfExperience = 7, Specialty = "Electricidad y Energía" },
                new { UserID = -15, FullName = "María Ortega", PasswordHash = "tech04", DepartmentID = -22, YearsOfExperience = 4, Specialty = "Ciberseguridad" },
                new { UserID = -16, FullName = "Ana López", PasswordHash = "tech05", DepartmentID = -11, YearsOfExperience = 6, Specialty = "Fibra Óptica" }
            );

            // EquipmentReceiver
            modelBuilder.Entity<DeviceReceiver>().HasData(
                new { UserID = -17, FullName = "Miguel Ángel Santos", PasswordHash = "rec01", DepartmentID = -9 },
                new { UserID = -18, FullName = "Ana García", PasswordHash = "rec02", DepartmentID = -2 },
                new { UserID = -19, FullName = "Luis Fernández", PasswordHash = "rec03", DepartmentID = -6 },
                new { UserID = -20, FullName = "Marta Jiménez", PasswordHash = "rec04", DepartmentID = -24 },
                new { UserID = -21, FullName = "Carlos Ruiz", PasswordHash = "rec05", DepartmentID = -17 }
            );

            // Seed Devices
            modelBuilder.Entity<Device>().HasData(
                new { DeviceID = -1, Name = "Router de Agregación ASR 9000", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentID = -8, AcquisitionDate = dateInEighteenDaysBefore },
                new { DeviceID = -2, Name = "Servidor de Virtualización HP DL380", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.UnderMaintenance, DepartmentID = -16, AcquisitionDate = today },
                new { DeviceID = -3, Name = "Firewall de Próxima Generación PA-5200", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentID = -24, AcquisitionDate = dateInEighteenDaysBefore },
                new { DeviceID = -4, Name = "Sistema UPS Eaton 20kVA", Type = DeviceType.ElectricalInfrastructureAndSupport, OperationalState = OperationalState.Decommissioned, DepartmentID = -24, AcquisitionDate = dateInEighteenDaysBefore },
                new { DeviceID = -5, Name = "Antena de Radioenlace AirFiber 5XHD", Type = DeviceType.CommunicationsAndTransmission, OperationalState = OperationalState.Operational, DepartmentID = -12, AcquisitionDate = today },
                new { DeviceID = -6, Name = "Analizador de Espectro Viavi", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentID = -14, AcquisitionDate = dateInEighteenDaysBefore }
            );

            // Seed Mainteinance - Usa DateOnly creados correctamente
            modelBuilder.Entity<MaintenanceRecord>().HasData(
                new { TechnicianID = -12, DeviceID = -1, Date = new DateOnly(2020, 12, 30), Cost = 0.0, Type = "Preventivo", MaintenanceRecordID = -1 },
                new { TechnicianID = -13, DeviceID = -2, Date = new DateOnly(2022, 5, 13), Cost = 100.0, Type = "Correctivo", MaintenanceRecordID = -2 },
                new { TechnicianID = -14, DeviceID = -3, Date = new DateOnly(2022, 10, 10), Cost = 20.0, Type = "Correctivo", MaintenanceRecordID = -3 },
                new { TechnicianID = -15, DeviceID = -4, Date = new DateOnly(2021, 5, 11), Cost = 10.5, Type = "Preventivo", MaintenanceRecordID = -4 },
                new { TechnicianID = -16, DeviceID = -5, Date = new DateOnly(2020, 8, 24), Cost = 0.0, Type = "Correctivo", MaintenanceRecordID = -5 },
                new { TechnicianID = -14, DeviceID = -5, Date = new DateOnly(2022, 7, 18), Cost = 0.0, Type = "Correctivo", MaintenanceRecordID = -6 }
            );

            // Seed DecommissioningRequest
            modelBuilder.Entity<DecommissioningRequest>().HasData(
                new { DecommissioningRequestID = -1, TechnicianID = -14, DeviceID = -3, DeviceReceiverID = -19, Date = dateInSevenDaysBefore },
                new { DecommissioningRequestID = -2, TechnicianID = -15, DeviceID = -4, DeviceReceiverID = -20, Date = dateInFiveDaysBefore }
            );

            // Seed Decommissioning
            modelBuilder.Entity<Decommissioning>().HasData(
                new { DecommissioningID = -1, DeviceID = -4, DecommissioningRequestID = -2, DeviceReceiverID = -20, ReceiverDepartmentID = -23, DecommissioningDate = dateInFiveDaysBefore, Reason = DecommissioningReason.SeverePhysicalDamage, FinalDestination = "Reciclaje" }
            );


            // Seed ReceivingInspectionRequest
            modelBuilder.Entity<ReceivingInspectionRequest>().HasData(
            );

            // Seed Rejection
            modelBuilder.Entity<Rejection>().HasData(
            );

            // Seed PerformanceRating
            modelBuilder.Entity<PerformanceRating>().HasData(
            );
        }
    }
}
