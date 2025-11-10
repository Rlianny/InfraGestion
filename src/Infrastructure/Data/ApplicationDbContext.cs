using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
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

            // Aplicar todas las configuraciones del assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Seed Roles
            modelBuilder
                .Entity<Domain.Entities.Role>()
                .HasData(
                    new { RoleId = (int)RoleEnum.Technician, Name = "Technician" },
                    new { RoleId = (int)RoleEnum.EquipmentReceiver, Name = "EquipmentReceiver" },
                    new { RoleId = (int)RoleEnum.SectionManager, Name = "SectionManager" },
                    new { RoleId = (int)RoleEnum.Administrator, Name = "Administrator" },
                    new { RoleId = (int)RoleEnum.Director, Name = "Director" }
                );

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

            // Seed Sections - SIN SectionManagerId
            modelBuilder
                .Entity<Section>()
                .HasData(
                    new { SectionId = -1, Name = "Operaciones de Red Corporativa" },
                    new
                    {
                        SectionId = -2,
                        Name = "Infraestructura de Centro de Datos (Data Center)",
                    },
                    new { SectionId = -3, Name = "Soporte Técnico en Campo" },
                    new { SectionId = -4, Name = "Planificación y Despliegue de Red" },
                    new { SectionId = -5, Name = "División de Servicios en la Nube (Cloud)" },
                    new { SectionId = -6, Name = "Taller Central y Logística" },
                    new { SectionId = -7, Name = "Infraestructura Interna (TI Interno)" },
                    new { SectionId = -8, Name = "Seguridad Informática (Ciberseguridad)" }
                );

            // Seed Departments
            modelBuilder
                .Entity<Department>()
                .HasData(
                    new
                    {
                        DepartmentId = 1,
                        Name = "Mocking Deparment",
                        SectionId = -1,
                    },
                    new
                    {
                        DepartmentId = -1,
                        Name = "Conmutación y Enrutamiento Avanzado",
                        SectionId = -1,
                    },
                    new
                    {
                        DepartmentId = -2,
                        Name = "Seguridad Perimetral y Firewalls",
                        SectionId = -1,
                    },
                    new
                    {
                        DepartmentId = -3,
                        Name = "Reparaciones de Red",
                        SectionId = -1,
                    },
                    new
                    {
                        DepartmentId = -4,
                        Name = "Servidores y Virtualización",
                        SectionId = -2,
                    },
                    new
                    {
                        DepartmentId = -5,
                        Name = "Almacenamiento y Backup",
                        SectionId = -2,
                    },
                    new
                    {
                        DepartmentId = -6,
                        Name = "Infraestructura Física y Climatización",
                        SectionId = -2,
                    },
                    new
                    {
                        DepartmentId = -7,
                        Name = "Instalaciones y Activaciones",
                        SectionId = -3,
                    },
                    new
                    {
                        DepartmentId = -8,
                        Name = "Mantenimiento Correctivo y Urgencias",
                        SectionId = -3,
                    },
                    new
                    {
                        DepartmentId = -9,
                        Name = "Soporte a Nodos Remotos",
                        SectionId = -3,
                    },
                    new
                    {
                        DepartmentId = -10,
                        Name = "Diseño y Ingeniería de Red",
                        SectionId = -4,
                    },
                    new
                    {
                        DepartmentId = -11,
                        Name = "Despliegue de Fibra Óptica y Acceso",
                        SectionId = -4,
                    },
                    new
                    {
                        DepartmentId = -12,
                        Name = "Mediciones y Certificación de Red",
                        SectionId = -4,
                    },
                    new
                    {
                        DepartmentId = -13,
                        Name = "Infraestructura como Servicio",
                        SectionId = -5,
                    },
                    new
                    {
                        DepartmentId = -14,
                        Name = "Plataforma como Servicio",
                        SectionId = -5,
                    },
                    new
                    {
                        DepartmentId = -15,
                        Name = "Operaciones Cloud y Escalabilidad",
                        SectionId = -5,
                    },
                    new
                    {
                        DepartmentId = -16,
                        Name = "Recepción y Diagnóstico Técnico",
                        SectionId = -6,
                    },
                    new
                    {
                        DepartmentId = -17,
                        Name = "Reparación y Refabricación",
                        SectionId = -6,
                    },
                    new
                    {
                        DepartmentId = -18,
                        Name = "Gestión de Inventario y Distribución",
                        SectionId = -6,
                    },
                    new
                    {
                        DepartmentId = -19,
                        Name = "Soporte al Usuario y Helpdesk",
                        SectionId = -7,
                    },
                    new
                    {
                        DepartmentId = -20,
                        Name = "Comunicaciones Unificadas y Telefonía IP",
                        SectionId = -7,
                    },
                    new
                    {
                        DepartmentId = -21,
                        Name = "Gestión de Activos y Red Local",
                        SectionId = -7,
                    },
                    new
                    {
                        DepartmentId = -22,
                        Name = "Arquitectura y Gestión de Firewalls",
                        SectionId = -8,
                    },
                    new
                    {
                        DepartmentId = -23,
                        Name = "Monitorización de Amenazas y SOC",
                        SectionId = -8,
                    },
                    new
                    {
                        DepartmentId = -24,
                        Name = "Análisis Forense y Respuesta a Incidentes",
                        SectionId = -8,
                    }
                );

            var passwordHasher = new PasswordHasher();

            // Seed Users - Administrator
            modelBuilder
                .Entity<User>()
                .HasData(
                    new
                    {
                        UserId = -1,
                        Username = "dgonzalez",
                        FullName = "David González",
                        PasswordHash = passwordHasher.HashPassword("admin01"),
                        DepartmentId = -21,
                        RoleId = (int)RoleEnum.Administrator,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -2,
                        Username = "lmartinez",
                        FullName = "Laura Martínez",
                        PasswordHash = passwordHasher.HashPassword("admin02"),
                        DepartmentId = -18,
                        RoleId = (int)RoleEnum.Administrator,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -3,
                        Username = "jrodriguez",
                        FullName = "Javier Rodríguez",
                        PasswordHash = passwordHasher.HashPassword("admin03"),
                        DepartmentId = -18,
                        RoleId = (int)RoleEnum.Administrator,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -4,
                        Username = "csanchez",
                        FullName = "Carmen Sánchez",
                        PasswordHash = passwordHasher.HashPassword("admin04"),
                        DepartmentId = -5,
                        RoleId = (int)RoleEnum.Administrator,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -5,
                        Username = "rlopez",
                        FullName = "Roberto López",
                        PasswordHash = passwordHasher.HashPassword("admin05"),
                        DepartmentId = -3,
                        RoleId = (int)RoleEnum.Administrator,
                        CreatedAt = today,
                        IsActive = true,
                    }
                );

            // Director
            modelBuilder
                .Entity<User>()
                .HasData(
                    new
                    {
                        UserId = -6,
                        Username = "emorales",
                        FullName = "Elena Morales",
                        PasswordHash = passwordHasher.HashPassword("dir123"),
                        DepartmentId = -24,
                        RoleId = (int)RoleEnum.Director,
                        CreatedAt = today,
                        IsActive = true,
                    }
                );

            // SectionManagers
            modelBuilder
                .Entity<User>()
                .HasData(
                    new
                    {
                        UserId = -7,
                        Username = "sramirez",
                        FullName = "Sofía Ramírez",
                        PasswordHash = passwordHasher.HashPassword("manager01"),
                        DepartmentId = -1,
                        RoleId = (int)RoleEnum.SectionManager,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -8,
                        Username = "atorres",
                        FullName = "Alejandro Torres",
                        PasswordHash = passwordHasher.HashPassword("manager02"),
                        DepartmentId = -6,
                        RoleId = (int)RoleEnum.SectionManager,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -9,
                        Username = "pherrera",
                        FullName = "Patricia Herrera",
                        PasswordHash = passwordHasher.HashPassword("manager03"),
                        DepartmentId = -7,
                        RoleId = (int)RoleEnum.SectionManager,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -10,
                        Username = "rdiaz",
                        FullName = "Ricardo Díaz",
                        PasswordHash = passwordHasher.HashPassword("manager04"),
                        DepartmentId = -10,
                        RoleId = (int)RoleEnum.SectionManager,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -11,
                        Username = "icastro",
                        FullName = "Isabel Castro",
                        PasswordHash = passwordHasher.HashPassword("manager05"),
                        DepartmentId = -13,
                        RoleId = (int)RoleEnum.SectionManager,
                        CreatedAt = today,
                        IsActive = true,
                    }
                );

            // Technicians
            modelBuilder
                .Entity<User>()
                .HasData(
                    new
                    {
                        UserId = -12,
                        Username = "cmendez",
                        FullName = "Carlos Méndez",
                        PasswordHash = passwordHasher.HashPassword("tech01"),
                        DepartmentId = -3,
                        YearsOfExperience = 5,
                        Specialty = "Redes y Comunicaciones",
                        RoleId = (int)RoleEnum.Technician,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -13,
                        Username = "evargas",
                        FullName = "Eduardo Vargas",
                        PasswordHash = passwordHasher.HashPassword("tech02"),
                        DepartmentId = -6,
                        YearsOfExperience = 3,
                        Specialty = "Servidores y Virtualización",
                        RoleId = (int)RoleEnum.Technician,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -14,
                        Username = "jsilva",
                        FullName = "Jorge Silva",
                        PasswordHash = passwordHasher.HashPassword("tech03"),
                        DepartmentId = -9,
                        YearsOfExperience = 7,
                        Specialty = "Electricidad y Energía",
                        RoleId = (int)RoleEnum.Technician,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -15,
                        Username = "mortega",
                        FullName = "María Ortega",
                        PasswordHash = passwordHasher.HashPassword("tech04"),
                        DepartmentId = -22,
                        YearsOfExperience = 4,
                        Specialty = "Ciberseguridad",
                        RoleId = (int)RoleEnum.Technician,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -16,
                        Username = "alopez",
                        FullName = "Ana López",
                        PasswordHash = passwordHasher.HashPassword("tech05"),
                        DepartmentId = -11,
                        YearsOfExperience = 6,
                        Specialty = "Fibra Óptica",
                        RoleId = (int)RoleEnum.Technician,
                        CreatedAt = today,
                        IsActive = true,
                    }
                );

            // EquipmentReceiver
            modelBuilder
                .Entity<User>()
                .HasData(
                    new
                    {
                        UserId = -17,
                        Username = "msantos",
                        FullName = "Miguel Ángel Santos",
                        PasswordHash = passwordHasher.HashPassword("rec01"),
                        DepartmentId = -9,
                        RoleId = (int)RoleEnum.EquipmentReceiver,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -18,
                        Username = "agarcia",
                        FullName = "Ana García",
                        PasswordHash = passwordHasher.HashPassword("rec02"),
                        DepartmentId = -2,
                        RoleId = (int)RoleEnum.EquipmentReceiver,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -19,
                        Username = "lfernandez",
                        FullName = "Luis Fernández",
                        PasswordHash = passwordHasher.HashPassword("rec03"),
                        DepartmentId = -6,
                        RoleId = (int)RoleEnum.EquipmentReceiver,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -20,
                        Username = "mjimenez",
                        FullName = "Marta Jiménez",
                        PasswordHash = passwordHasher.HashPassword("rec04"),
                        DepartmentId = -24,
                        RoleId = (int)RoleEnum.EquipmentReceiver,
                        CreatedAt = today,
                        IsActive = true,
                    },
                    new
                    {
                        UserId = -21,
                        Username = "cruiz",
                        FullName = "Carlos Ruiz",
                        PasswordHash = passwordHasher.HashPassword("rec05"),
                        DepartmentId = -17,
                        RoleId = (int)RoleEnum.EquipmentReceiver,
                        CreatedAt = today,
                        IsActive = true,
                    }
                );

            // Seed Devices
            modelBuilder
                .Entity<Device>()
                .HasData(
                    new
                    {
                        DeviceId = -1,
                        Name = "Router de Agregación ASR 9000",
                        Type = DeviceType.ConnectivityAndNetwork,
                        OperationalState = OperationalState.Operational,
                        DepartmentId = -8,
                        AcquisitionDate = dateInEighteenDaysBefore,
                    },
                    new
                    {
                        DeviceId = -2,
                        Name = "Servidor de Virtualización HP DL380",
                        Type = DeviceType.ComputingAndIT,
                        OperationalState = OperationalState.UnderMaintenance,
                        DepartmentId = -16,
                        AcquisitionDate = today,
                    },
                    new
                    {
                        DeviceId = -3,
                        Name = "Firewall de Próxima Generación PA-5200",
                        Type = DeviceType.ConnectivityAndNetwork,
                        OperationalState = OperationalState.Operational,
                        DepartmentId = -24,
                        AcquisitionDate = dateInEighteenDaysBefore,
                    },
                    new
                    {
                        DeviceId = -4,
                        Name = "Sistema UPS Eaton 20kVA",
                        Type = DeviceType.ElectricalInfrastructureAndSupport,
                        OperationalState = OperationalState.Decommissioned,
                        DepartmentId = -24,
                        AcquisitionDate = dateInEighteenDaysBefore,
                    },
                    new
                    {
                        DeviceId = -5,
                        Name = "Antena de Radioenlace AirFiber 5XHD",
                        Type = DeviceType.CommunicationsAndTransmission,
                        OperationalState = OperationalState.Operational,
                        DepartmentId = -12,
                        AcquisitionDate = today,
                    },
                    new
                    {
                        DeviceId = -6,
                        Name = "Analizador de Espectro Viavi",
                        Type = DeviceType.DiagnosticAndMeasurement,
                        OperationalState = OperationalState.Operational,
                        DepartmentId = -14,
                        AcquisitionDate = dateInEighteenDaysBefore,
                    }
                );

            // Seed Mainteinance - Usa DateOnly creados correctamente
            // modelBuilder.Entity<MaintenanceRecord>().HasData(
            //    new { TechnicianId = -12, DeviceId = -1, Date = new DateOnly(2020, 12, 30), Cost = 0.0, Type = "Preventivo", MaintenanceRecordId = -1 },
            //    new { TechnicianId = -13, DeviceId = -2, Date = new DateOnly(2022, 5, 13), Cost = 100.0, Type = "Correctivo", MaintenanceRecordId = -2 },
            //    new { TechnicianId = -14, DeviceId = -3, Date = new DateOnly(2022, 10, 10), Cost = 20.0, Type = "Correctivo", MaintenanceRecordId = -3 },
            //    new { TechnicianId = -15, DeviceId = -4, Date = new DateOnly(2021, 5, 11), Cost = 10.5, Type = "Preventivo", MaintenanceRecordId = -4 },
            //    new { TechnicianId = -16, DeviceId = -5, Date = new DateOnly(2020, 8, 24), Cost = 0.0, Type = "Correctivo", MaintenanceRecordId = -5 },
            //    new { TechnicianId = -14, DeviceId = -5, Date = new DateOnly(2022, 7, 18), Cost = 0.0, Type = "Correctivo", MaintenanceRecordId = -6 }
            // );

            // // Seed DecommissioningRequest
            // modelBuilder.Entity<DecommissioningRequest>().HasData(
            //    new { DecommissioningRequestId = -1, TechnicianId = -14, DeviceId = -3, DeviceReceiverId = -19, Date = dateInSevenDaysBefore },
            //    new { DecommissioningRequestId = -2, TechnicianId = -15, DeviceId = -4, DeviceReceiverId = -20, Date = dateInFiveDaysBefore }
            // );

            // // Seed Decommissioning
            // modelBuilder.Entity<Decommissioning>().HasData(
            //    new { DecommissioningId = -1, DeviceId = -4, DecommissioningRequestId = -2, DeviceReceiverId = -20, ReceiverDepartmentId = -23, DecommissioningDate = dateInFiveDaysBefore, Reason = DecommissioningReason.SeverePhysicalDamage, FinalDestination = "Reciclaje" }
            // );

            // // Seed ReceivingInspectionRequest
            // modelBuilder.Entity<ReceivingInspectionRequest>().HasData(
            // );

            // // Seed Rejection
            // modelBuilder.Entity<Rejection>().HasData(
            // );

            // // Seed PerformanceRating
            // modelBuilder.Entity<PerformanceRating>().HasData(
            // );
        }
    }
}
