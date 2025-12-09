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
          : base(options)
      {
      }

      // Entities
      public DbSet<User> Users { get; set; }
      public DbSet<Role> Roles { get; set; }
      public DbSet<Department> Departments { get; set; }
      public DbSet<Section> Sections { get; set; }
      public DbSet<Device> Devices { get; set; }

      // Aggregations
      public DbSet<Transfer> Transfers { get; set; }
      public DbSet<MaintenanceRecord> Mainteinances { get; set; }
      public DbSet<DecommissioningRequest> DecommissioningRequests { get; set; }
      public DbSet<ReceivingInspectionRequest> ReceivingInspectionRequests { get; set; }
      public DbSet<Rejection> Rejections { get; set; }
      public DbSet<PerformanceRating> Assessments { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Domain.Entities.Role>().HasData(
            new { RoleId = (int)RoleEnum.Technician, Name = "Technician" },
            new { RoleId = (int)RoleEnum.EquipmentReceiver, Name = "EquipmentReceiver" },
            new { RoleId = (int)RoleEnum.SectionManager, Name = "SectionManager" },
            new { RoleId = (int)RoleEnum.Administrator, Name = "Administrator" },
            new { RoleId = (int)RoleEnum.Director, Name = "Director" },
            new { RoleId = (int)RoleEnum.Logistician, Name = "Logistician" }
         );

         // Dates STATICS
         var today = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
         var dateInFifteenDays = new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc);
         var dateInThreeDays = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc);
         var dateInTenDays = new DateTime(2024, 12, 22, 0, 0, 0, DateTimeKind.Utc);
         var dateInFifteenDaysBefore = new DateTime(2024, 12, 17, 0, 0, 0, DateTimeKind.Utc);
         var dateInFiveDaysBefore = new DateTime(2024, 12, 27, 0, 0, 0, DateTimeKind.Utc);
         var dateInTwentyDaysBefore = new DateTime(2024, 12, 12, 0, 0, 0, DateTimeKind.Utc);
         var dateInEighteenDaysBefore = new DateTime(2024, 12, 14, 0, 0, 0, DateTimeKind.Utc);
         var dateInSevenDaysBefore = new DateTime(2024, 12, 25, 0, 0, 0, DateTimeKind.Utc);

         // Passwords STATICS (hashed using BCrypt) DON'T DELETE
         var passwordManager1 = "$2a$11$eZJoOAWDoV6iLxXJtV3sjeW134dzJnATrW0BUXedvDUpm8D3v1vC6"; // manager01s
         var passwordManager2 = "$2a$11$2p2NYJ3apLI342Dtp5EpEub2nxcPFkzsb88ci3oDI25IykC.0Unfm"; // manager02
         var passwordManager3 = "$2a$11$ZeaAzBiPv.Xw6IYn5HsfM.TsEo8Rsh7iwC6xyVRAY6kAibSVuK.IW"; // manager03
         var passwordManager4 = "$2a$11$DuHpt0qFrD1nX4vvR6ypUu59ou5tGK8J.v8k3KhzMCQ/LzqyFOfWi"; // manager04
         var passwordManager5 = "$2a$11$6l3KjoYoKF0fDqIgEDwB4u8gUJ6IKOQgMf1K6n4DoYneSFqPrMRvu"; // manager05

         var passwordAdmin1 = "$2a$11$kkxPo0Sl6gHf46gy7Pe5xeZZa0X2gGboRBG4Rd9gWniOm1PGHZ7he"; // admin01
         var passwordAdmin2 = "$2a$11$2u/fqdnoGBupkujZw8HXeu3tjULZ6e.EqZVEQwLmWGWRvPPpu8gee"; // admin02
         var passwordAdmin3 = "$2a$11$/jwi2T7PHM6fSAyAelnoaOOxPMLU25uXG/3NvTyTK5rMlEoSZm55y"; // admin03
         var passwordAdmin4 = "$2a$11$J0ZStMp50aiYljT3vj.yQucG.tqZOI7x43quH5EdmzB6K5z5cIVuq"; // admin04
         var passwordAdmin5 = "$2a$11$OlHPfN0V9EcZwDZ2NhvzaOT0E6F8/EfWo2wHzJhSFEVEwd7fqBkCa"; // admin05

         var passwordDirector1 = "$2a$11$frxc8XpGXgGi53fiLcSRoOt3Nq7aE56VMPf.ECnNWXJkxIFPXxrMq"; // dir01

         // Seed Sections
         modelBuilder.Entity<Section>().HasData(
            new { SectionId = -1, Name = "Operaciones de Red Corporativa", IsDisabled = false },
            new { SectionId = -2, Name = "Infraestructura de Centro de Datos (Data Center)", IsDisabled = false },
            new { SectionId = -3, Name = "Soporte Técnico en Campo", IsDisabled = false },
            new { SectionId = -4, Name = "Planificación y Despliegue de Red", IsDisabled = false },
            new { SectionId = -5, Name = "División de Servicios en la Nube (Cloud)", IsDisabled = false },
            new { SectionId = -6, Name = "Taller Central y Logística", IsDisabled = false },
            new { SectionId = -7, Name = "Infraestructura Interna (TI Interno)", IsDisabled = false },
            new { SectionId = -8, Name = "Seguridad Informática (Ciberseguridad)", IsDisabled = false }
         );

         // Seed Departments
         modelBuilder.Entity<Department>().HasData(

            new { DepartmentId = 1, Name = "Almacen General", SectionId = -1, IsDisabled = false },

            new { DepartmentId = -1, Name = "Conmutación y Enrutamiento Avanzado", SectionId = -1, IsDisabled = false },
            new { DepartmentId = -2, Name = "Seguridad Perimetral y Firewalls", SectionId = -1, IsDisabled = false },
            new { DepartmentId = -3, Name = "Reparaciones de Red", SectionId = -1, IsDisabled = false },

            new { DepartmentId = -4, Name = "Servidores y Virtualización", SectionId = -2, IsDisabled = false },
            new { DepartmentId = -5, Name = "Almacenamiento y Backup", SectionId = -2, IsDisabled = false },
            new { DepartmentId = -6, Name = "Infraestructura Física y Climatización", SectionId = -2, IsDisabled = false },

            new { DepartmentId = -7, Name = "Instalaciones y Activaciones", SectionId = -3, IsDisabled = false },
            new { DepartmentId = -8, Name = "Mantenimiento Correctivo y Urgencias", SectionId = -3, IsDisabled = false },
            new { DepartmentId = -9, Name = "Soporte a Nodos Remotos", SectionId = -3, IsDisabled = false },

            new { DepartmentId = -10, Name = "Diseño y Ingeniería de Red", SectionId = -4, IsDisabled = false },
            new { DepartmentId = -11, Name = "Despliegue de Fibra Óptica y Acceso", SectionId = -4, IsDisabled = false },
            new { DepartmentId = -12, Name = "Mediciones y Certificación de Red", SectionId = -4, IsDisabled = false },

            new { DepartmentId = -13, Name = "Infraestructura como Servicio", SectionId = -5, IsDisabled = false },
            new { DepartmentId = -14, Name = "Plataforma como Servicio", SectionId = -5, IsDisabled = false },
            new { DepartmentId = -15, Name = "Operaciones Cloud y Escalabilidad", SectionId = -5, IsDisabled = false },

            new { DepartmentId = -16, Name = "Recepción y Diagnóstico Técnico", SectionId = -6, IsDisabled = false },
            new { DepartmentId = -17, Name = "Reparación y Refabricación", SectionId = -6, IsDisabled = false },
            new { DepartmentId = -18, Name = "Gestión de Inventario y Distribución", SectionId = -6, IsDisabled = false },

            new { DepartmentId = -19, Name = "Soporte al Usuario y Helpdesk", SectionId = -7, IsDisabled = false },
            new { DepartmentId = -20, Name = "Comunicaciones Unificadas y Telefonía IP", SectionId = -7, IsDisabled = false },
            new { DepartmentId = -21, Name = "Gestión de Activos y Red Local", SectionId = -7, IsDisabled = false },

            new { DepartmentId = -22, Name = "Arquitectura y Gestión de Firewalls", SectionId = -8, IsDisabled = false },
            new { DepartmentId = -23, Name = "Monitorización de Amenazas y SOC", SectionId = -8, IsDisabled = false },
            new { DepartmentId = -24, Name = "Análisis Forense y Respuesta a Incidentes", SectionId = -8, IsDisabled = false }
         );

         // Seed Users - Administrator
         modelBuilder.Entity<User>().HasData(
            new { UserId = -1, Username = "dgonzalez", FullName = "David González", PasswordHash = passwordAdmin1, DepartmentId = -21, RoleId = (int)RoleEnum.Administrator, CreatedAt = today, IsActive = true },
            new { UserId = -2, Username = "lmartinez", FullName = "Laura Martínez", PasswordHash = passwordAdmin2, DepartmentId = -18, RoleId = (int)RoleEnum.Administrator, CreatedAt = today, IsActive = true },
            new { UserId = -3, Username = "jrodriguez", FullName = "Javier Rodríguez", PasswordHash = passwordAdmin3, DepartmentId = -18, RoleId = (int)RoleEnum.Administrator, CreatedAt = today, IsActive = true },
            new { UserId = -4, Username = "csanchez", FullName = "Carmen Sánchez", PasswordHash = passwordAdmin4, DepartmentId = -5, RoleId = (int)RoleEnum.Administrator, CreatedAt = today, IsActive = true },
            new { UserId = -5, Username = "rlopez", FullName = "Roberto López", PasswordHash = passwordAdmin5, DepartmentId = -3, RoleId = (int)RoleEnum.Administrator, CreatedAt = today, IsActive = true }
         );

         // Director
         modelBuilder.Entity<User>().HasData(
            new { UserId = -6, Username = "emorales", FullName = "Elena Morales", PasswordHash = passwordDirector1, DepartmentId = -24, RoleId = (int)RoleEnum.Director, CreatedAt = today, IsActive = true }
         );

         // SectionManagers
         modelBuilder.Entity<User>().HasData(
            new { UserId = -7, Username = "sramirez", FullName = "Sofía Ramírez", PasswordHash = passwordManager1, DepartmentId = -1, RoleId = (int)RoleEnum.SectionManager, CreatedAt = today, IsActive = true },
            new { UserId = -8, Username = "atorres", FullName = "Alejandro Torres", PasswordHash = passwordManager2, DepartmentId = -6, RoleId = (int)RoleEnum.SectionManager, CreatedAt = today, IsActive = true },
            new { UserId = -9, Username = "pherrera", FullName = "Patricia Herrera", PasswordHash = passwordManager3, DepartmentId = -7, RoleId = (int)RoleEnum.SectionManager, CreatedAt = today, IsActive = true },
            new { UserId = -10, Username = "rdiaz", FullName = "Ricardo Díaz", PasswordHash = passwordManager4, DepartmentId = -10, RoleId = (int)RoleEnum.SectionManager, CreatedAt = today, IsActive = true },
            new { UserId = -11, Username = "icastro", FullName = "Isabel Castro", PasswordHash = passwordManager5, DepartmentId = -13, RoleId = (int)RoleEnum.SectionManager, CreatedAt = today, IsActive = true }
         );

         // Technicians
         modelBuilder.Entity<User>().HasData(
            new { UserId = -12, Username = "cmendez", FullName = "Carlos Méndez", PasswordHash = "tech01", DepartmentId = -3, YearsOfExperience = 5, Specialty = "Redes y Comunicaciones", RoleId = (int)RoleEnum.Technician, CreatedAt = today, IsActive = true },
            new { UserId = -13, Username = "evargas", FullName = "Eduardo Vargas", PasswordHash = "tech02", DepartmentId = -6, YearsOfExperience = 3, Specialty = "Servidores y Virtualización", RoleId = (int)RoleEnum.Technician, CreatedAt = today, IsActive = true },
            new { UserId = -14, Username = "jsilva", FullName = "Jorge Silva", PasswordHash = "tech03", DepartmentId = -9, YearsOfExperience = 7, Specialty = "Electricidad y Energía", RoleId = (int)RoleEnum.Technician, CreatedAt = today, IsActive = true },
            new { UserId = -15, Username = "mortega", FullName = "María Ortega", PasswordHash = "tech04", DepartmentId = -22, YearsOfExperience = 4, Specialty = "Ciberseguridad", RoleId = (int)RoleEnum.Technician, CreatedAt = today, IsActive = true },
            new { UserId = -16, Username = "alopez", FullName = "Ana López", PasswordHash = "tech05", DepartmentId = -11, YearsOfExperience = 6, Specialty = "Fibra Óptica", RoleId = (int)RoleEnum.Technician, CreatedAt = today, IsActive = true }
         );

         // EquipmentReceiver
         modelBuilder.Entity<User>().HasData(
            new { UserId = -17, Username = "msantos", FullName = "Miguel Ángel Santos", PasswordHash = "rec01", DepartmentId = -9, RoleId = (int)RoleEnum.EquipmentReceiver, CreatedAt = today, IsActive = true },
            new { UserId = -18, Username = "agarcia", FullName = "Ana García", PasswordHash = "rec02", DepartmentId = -2, RoleId = (int)RoleEnum.EquipmentReceiver, CreatedAt = today, IsActive = true },
            new { UserId = -19, Username = "lfernandez", FullName = "Luis Fernández", PasswordHash = "rec03", DepartmentId = -6, RoleId = (int)RoleEnum.EquipmentReceiver, CreatedAt = today, IsActive = true },
            new { UserId = -20, Username = "mjimenez", FullName = "Marta Jiménez", PasswordHash = "rec04", DepartmentId = -24, RoleId = (int)RoleEnum.EquipmentReceiver, CreatedAt = today, IsActive = true },
            new { UserId = -21, Username = "cruiz", FullName = "Carlos Ruiz", PasswordHash = "rec05", DepartmentId = -17, RoleId = (int)RoleEnum.EquipmentReceiver, CreatedAt = today, IsActive = true }
         );

         // Seed Devices
         modelBuilder.Entity<Device>().HasData(
            new { DeviceId = -1, Name = "Router de Agregación ASR 9000", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -8, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },
            new { DeviceId = -2, Name = "Servidor de Virtualización HP DL380", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.UnderMaintenance, DepartmentId = -16, AcquisitionDate = today, IsDisabled = false },
            new { DeviceId = -3, Name = "Firewall de Próxima Generación PA-5200", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -24, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },
            new { DeviceId = -4, Name = "Sistema UPS Eaton 20kVA", Type = DeviceType.ElectricalInfrastructureAndSupport, OperationalState = OperationalState.Decommissioned, DepartmentId = -24, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },
            new { DeviceId = -5, Name = "Antena de Radioenlace AirFiber 5XHD", Type = DeviceType.CommunicationsAndTransmission, OperationalState = OperationalState.Operational, DepartmentId = -12, AcquisitionDate = today, IsDisabled = false },
            new { DeviceId = -6, Name = "Analizador de Espectro Viavi", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentId = -14, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false }
         );

         modelBuilder.Entity<MaintenanceRecord>().HasData(
            new { MaintenanceRecordId = -1, TechnicianId = -12, DeviceId = -1, Date = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), Cost = 120.0, Type = MaintenanceType.Preventive, Description = "Chequeo de enlaces y limpieza de puertos" },
            new { MaintenanceRecordId = -2, TechnicianId = -13, DeviceId = -2, Date = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc), Cost = 450.0, Type = MaintenanceType.Corrective, Description = "Reemplazo de módulo RAID" },
            new { MaintenanceRecordId = -3, TechnicianId = -14, DeviceId = -3, Date = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc), Cost = 210.0, Type = MaintenanceType.Preventive, Description = "Actualización de firmware y pruebas de firewall" },
            new { MaintenanceRecordId = -4, TechnicianId = -15, DeviceId = -4, Date = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), Cost = 80.0, Type = MaintenanceType.Corrective, Description = "Calibración y reemplazo de baterías UPS" },
            new { MaintenanceRecordId = -5, TechnicianId = -16, DeviceId = -5, Date = new DateTime(2025, 5, 12, 0, 0, 0, DateTimeKind.Utc), Cost = 150.0, Type = MaintenanceType.Predictive, Description = "Ajuste de antena y verificación de potencia" },
            new { MaintenanceRecordId = -6, TechnicianId = -12, DeviceId = -6, Date = new DateTime(2025, 6, 20, 0, 0, 0, DateTimeKind.Utc), Cost = 60.0, Type = MaintenanceType.Preventive, Description = "Revisión de calibración del analizador" }
         );

         modelBuilder.Entity<DecommissioningRequest>().HasData(
            new { DecommissioningRequestId = -1, TechnicianId = -14, DeviceId = -4, DeviceReceiverId = -19, Date = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Approved, Reason = DecommissioningReason.SeverePhysicalDamage, IsApproved = true, UserId = -1 },
            new { DecommissioningRequestId = -2, TechnicianId = -15, DeviceId = -3, DeviceReceiverId = -18, Date = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Pending, Reason = DecommissioningReason.TechnologicalObsolescence, IsApproved = false, UserId = (int?)null },
            new { DecommissioningRequestId = -3, TechnicianId = -16, DeviceId = -5, DeviceReceiverId = -17, Date = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Rejected, Reason = DecommissioningReason.ExcessiveRepairCost, IsApproved = false, UserId = -2 }
         );

         modelBuilder.Entity<Transfer>().HasData(
            new { TransferId = -1, DeviceId = -1, Date = new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -1, DestinationSectionId = -2, DeviceReceiverId = -17, Status = TransferStatus.InTransit, IsDisabled = false },
            new { TransferId = -2, DeviceId = -2, Date = new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -6, DestinationSectionId = -3, DeviceReceiverId = -18, Status = TransferStatus.Completed, IsDisabled = false },
            new { TransferId = -3, DeviceId = -5, Date = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -4, DestinationSectionId = -5, DeviceReceiverId = -19, Status = TransferStatus.Pending, IsDisabled = false }
         );

         modelBuilder.Entity<ReceivingInspectionRequest>().HasData(
            new { ReceivingInspectionRequestId = -1, DeviceId = -1, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.IrreparableTechnicalFailure },
            new { ReceivingInspectionRequestId = -2, DeviceId = -2, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 2, 17, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -3, DeviceId = -3, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 3, 20, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 3, 22, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.ExcessiveRepairCost }
         );

         modelBuilder.Entity<Rejection>().HasData(
            new { RejectionId = -1, DeviceReceiverId = -19, TechnicianId = -15, DeviceId = -3, DecommissioningRequestDate = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), RejectionDate = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new { RejectionId = -2, DeviceReceiverId = -17, TechnicianId = -16, DeviceId = -5, DecommissioningRequestDate = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc), RejectionDate = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc) }
         );

         modelBuilder.Entity<PerformanceRating>().HasData(
            new { PerformanceRatingId = -1, UserId = -7, TechnicianId = -12, Date = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), Score = 4.8, Description = "Excelente trabajo en red troncal" },
            new { PerformanceRatingId = -2, UserId = -8, TechnicianId = -13, Date = new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc), Score = 3.9, Description = "Buen manejo de servidores" },
            new { PerformanceRatingId = -3, UserId = -9, TechnicianId = -14, Date = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Utc), Score = 2.5, Description = "Mejorar tiempos de respuesta" },
            new { PerformanceRatingId = -4, UserId = -10, TechnicianId = -15, Date = new DateTime(2025, 4, 22, 0, 0, 0, DateTimeKind.Utc), Score = 4.2, Description = "Cumplimiento en ciberseguridad" },
            new { PerformanceRatingId = -5, UserId = -11, TechnicianId = -16, Date = new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), Score = 4.0, Description = "Trabajo constante en fibra" }
         );
      }
   }
}
