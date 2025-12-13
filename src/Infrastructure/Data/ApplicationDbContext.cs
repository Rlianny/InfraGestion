using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Services;
using Infrastructure.Data.Configurations;
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

         // Apply configuration only where needed (keeps current model changes scoped)
         modelBuilder.ApplyConfiguration(new DecommissioningRequestConfiguration());

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
            new { DeviceId = -6, Name = "Analizador de Espectro Viavi", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentId = -14, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },

            // Additional devices (for richer, consistent seeded data)
            new { DeviceId = -7, Name = "Switch Core Catalyst 9500", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -1, AcquisitionDate = dateInTwentyDaysBefore, IsDisabled = false },
            new { DeviceId = -8, Name = "Servidor de Base de Datos Dell R740", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.Operational, DepartmentId = -4, AcquisitionDate = dateInFifteenDaysBefore, IsDisabled = false },
            new { DeviceId = -9, Name = "Storage SAN NetApp AFF", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.UnderMaintenance, DepartmentId = -5, AcquisitionDate = dateInSevenDaysBefore, IsDisabled = false },
            new { DeviceId = -10, Name = "Sistema de Climatización CRAC", Type = DeviceType.ElectricalInfrastructureAndSupport, OperationalState = OperationalState.Decommissioned, DepartmentId = -6, AcquisitionDate = dateInTenDays, IsDisabled = false },
            new { DeviceId = -11, Name = "OTDR Fibra Óptica EXFO", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentId = -12, AcquisitionDate = dateInFiveDaysBefore, IsDisabled = false },
            new { DeviceId = -12, Name = "Radioenlace Cambium PTP", Type = DeviceType.CommunicationsAndTransmission, OperationalState = OperationalState.Operational, DepartmentId = -11, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },
            new { DeviceId = -13, Name = "Firewall FortiGate 200F", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -22, AcquisitionDate = dateInFiveDaysBefore, IsDisabled = false },
            new { DeviceId = -14, Name = "Servidor de Monitoreo Zabbix", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.Decommissioned, DepartmentId = -23, AcquisitionDate = dateInTenDays, IsDisabled = false },
            new { DeviceId = -15, Name = "UPS APC Smart-UPS 10kVA", Type = DeviceType.ElectricalInfrastructureAndSupport, OperationalState = OperationalState.Operational, DepartmentId = -6, AcquisitionDate = dateInFifteenDaysBefore, IsDisabled = false },
            new { DeviceId = -16, Name = "Analizador de Protocolos Wireshark Probe", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentId = -24, AcquisitionDate = dateInTwentyDaysBefore, IsDisabled = false },
            new { DeviceId = -17, Name = "Switch Acceso Aruba 2930F", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -21, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },
            new { DeviceId = -18, Name = "Servidor de Aplicaciones VMHost", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.Operational, DepartmentId = -4, AcquisitionDate = dateInSevenDaysBefore, IsDisabled = false },
            new { DeviceId = -19, Name = "CPE GPON Huawei", Type = DeviceType.CommunicationsAndTransmission, OperationalState = OperationalState.Operational, DepartmentId = -11, AcquisitionDate = dateInFiveDaysBefore, IsDisabled = false },
            new { DeviceId = -20, Name = "Medidor de Potencia Óptica", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentId = -12, AcquisitionDate = dateInFifteenDaysBefore, IsDisabled = false },

            // Some already decommissioned devices to match approved requests
            new { DeviceId = -21, Name = "Servidor Legacy Xeon E5", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.Decommissioned, DepartmentId = -4, AcquisitionDate = dateInTwentyDaysBefore, IsDisabled = false },
            new { DeviceId = -22, Name = "Router Legacy ISR 2900", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Decommissioned, DepartmentId = -3, AcquisitionDate = dateInTwentyDaysBefore, IsDisabled = false },
            new { DeviceId = -23, Name = "UPS Line-Interactive 3kVA", Type = DeviceType.ElectricalInfrastructureAndSupport, OperationalState = OperationalState.Decommissioned, DepartmentId = -6, AcquisitionDate = dateInTwentyDaysBefore, IsDisabled = false },

            // Remaining devices (operational/pending candidates)
            new { DeviceId = -24, Name = "Switch Distribución Nexus 3000", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -1, AcquisitionDate = dateInFifteenDaysBefore, IsDisabled = false },
            new { DeviceId = -25, Name = "Servidor Backup Veeam Proxy", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.UnderMaintenance, DepartmentId = -5, AcquisitionDate = dateInTenDays, IsDisabled = false },
            new { DeviceId = -26, Name = "Equipo de Pruebas Fluke Networks", Type = DeviceType.DiagnosticAndMeasurement, OperationalState = OperationalState.Operational, DepartmentId = -12, AcquisitionDate = dateInSevenDaysBefore, IsDisabled = false },
            new { DeviceId = -27, Name = "Radio Punto-a-Punto Ubiquiti", Type = DeviceType.CommunicationsAndTransmission, OperationalState = OperationalState.Operational, DepartmentId = -9, AcquisitionDate = dateInFifteenDaysBefore, IsDisabled = false },
            new { DeviceId = -28, Name = "Firewall Legacy ASA 5516", Type = DeviceType.ConnectivityAndNetwork, OperationalState = OperationalState.Operational, DepartmentId = -22, AcquisitionDate = dateInTwentyDaysBefore, IsDisabled = false },
            new { DeviceId = -29, Name = "Servidor de Logs SIEM", Type = DeviceType.ComputingAndIT, OperationalState = OperationalState.Operational, DepartmentId = -23, AcquisitionDate = dateInEighteenDaysBefore, IsDisabled = false },
            new { DeviceId = -30, Name = "UPS Modular 30kVA", Type = DeviceType.ElectricalInfrastructureAndSupport, OperationalState = OperationalState.Operational, DepartmentId = -6, AcquisitionDate = dateInSevenDaysBefore, IsDisabled = false }
         );

         modelBuilder.Entity<MaintenanceRecord>().HasData(
            new { MaintenanceRecordId = -1, TechnicianId = -12, DeviceId = -1, Date = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), Cost = 120.0, Type = MaintenanceType.Preventive, Description = "Chequeo de enlaces y limpieza de puertos" },
            new { MaintenanceRecordId = -2, TechnicianId = -13, DeviceId = -2, Date = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc), Cost = 450.0, Type = MaintenanceType.Corrective, Description = "Reemplazo de módulo RAID" },
            new { MaintenanceRecordId = -3, TechnicianId = -14, DeviceId = -3, Date = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc), Cost = 210.0, Type = MaintenanceType.Preventive, Description = "Actualización de firmware y pruebas de firewall" },
            new { MaintenanceRecordId = -4, TechnicianId = -15, DeviceId = -4, Date = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), Cost = 80.0, Type = MaintenanceType.Corrective, Description = "Calibración y reemplazo de baterías UPS" },
            new { MaintenanceRecordId = -5, TechnicianId = -16, DeviceId = -5, Date = new DateTime(2025, 5, 12, 0, 0, 0, DateTimeKind.Utc), Cost = 150.0, Type = MaintenanceType.Predictive, Description = "Ajuste de antena y verificación de potencia" },
            new { MaintenanceRecordId = -6, TechnicianId = -12, DeviceId = -6, Date = new DateTime(2025, 6, 20, 0, 0, 0, DateTimeKind.Utc), Cost = 60.0, Type = MaintenanceType.Preventive, Description = "Revisión de calibración del analizador" },

            // Additional maintenance records
            new { MaintenanceRecordId = -7, TechnicianId = -13, DeviceId = -7, Date = new DateTime(2025, 1, 22, 0, 0, 0, DateTimeKind.Utc), Cost = 95.0, Type = MaintenanceType.Preventive, Description = "Inspección de ventiladores y revisión de VLANs" },
            new { MaintenanceRecordId = -8, TechnicianId = -14, DeviceId = -8, Date = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc), Cost = 180.0, Type = MaintenanceType.Preventive, Description = "Actualización de parches del SO y verificación de discos" },
            new { MaintenanceRecordId = -9, TechnicianId = -15, DeviceId = -9, Date = new DateTime(2025, 2, 18, 0, 0, 0, DateTimeKind.Utc), Cost = 320.0, Type = MaintenanceType.Corrective, Description = "Reemplazo de fuente redundante y pruebas I/O" },
            new { MaintenanceRecordId = -10, TechnicianId = -16, DeviceId = -11, Date = new DateTime(2025, 3, 7, 0, 0, 0, DateTimeKind.Utc), Cost = 50.0, Type = MaintenanceType.Preventive, Description = "Recalibración y limpieza de conectores" },
            new { MaintenanceRecordId = -11, TechnicianId = -12, DeviceId = -12, Date = new DateTime(2025, 3, 21, 0, 0, 0, DateTimeKind.Utc), Cost = 140.0, Type = MaintenanceType.Predictive, Description = "Medición de potencia y análisis de degradación" },
            new { MaintenanceRecordId = -12, TechnicianId = -13, DeviceId = -13, Date = new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), Cost = 110.0, Type = MaintenanceType.Preventive, Description = "Revisión de reglas y backup de configuración" }
         );

         modelBuilder.Entity<DecommissioningRequest>().HasData(
            // Keep the original three scenarios, aligned to current model
            new
            {
               DecommissioningRequestId = -1,
               TechnicianId = -14,
               DeviceId = -4,
               EmissionDate = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 1, 25, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Approved,
               Reason = DecommissioningReason.SeverePhysicalDamage,
               DeviceReceiverId = -19,
               IsApproved = (bool?)true,
               FinalDestinationDepartmentID = -18,
               logisticId = -1,
               description = "Daño físico severo: equipo fuera de servicio. Se autoriza baja y traslado a almacén general."
            },
            new
            {
               DecommissioningRequestId = -2,
               TechnicianId = -15,
               DeviceId = -3,
               EmissionDate = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.TechnologicalObsolescence,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -3,
               TechnicianId = -16,
               DeviceId = -5,
               EmissionDate = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.ExcessiveRepairCost,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -2,
               description = "Rechazado: se requiere reevaluación, no se justifica baja con la evidencia presentada."
            },

            // Additional consistent requests (mix of Pending/Approved/Rejected)
            new
            {
               DecommissioningRequestId = -4,
               TechnicianId = -12,
               DeviceId = -7,
               EmissionDate = new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -5,
               TechnicianId = -13,
               DeviceId = -8,
               EmissionDate = new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.IrreparableTechnicalFailure,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -3,
               description = "Rechazado: el equipo puede repararse; se deriva a mantenimiento correctivo."
            },
            new
            {
               DecommissioningRequestId = -6,
               TechnicianId = -14,
               DeviceId = -9,
               EmissionDate = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.ExcessiveRepairCost,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -7,
               TechnicianId = -15,
               DeviceId = -10,
               EmissionDate = new DateTime(2025, 1, 14, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 1, 18, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Approved,
               Reason = DecommissioningReason.SeverePhysicalDamage,
               DeviceReceiverId = -21,
               IsApproved = (bool?)true,
               FinalDestinationDepartmentID = -18,
               logisticId = -4,
               description = "Aprobado: daño estructural en unidad CRAC. Baja autorizada y retiro por logística."
            },
            new
            {
               DecommissioningRequestId = -8,
               TechnicianId = -16,
               DeviceId = -11,
               EmissionDate = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -9,
               TechnicianId = -12,
               DeviceId = -12,
               EmissionDate = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.TechnologicalObsolescence,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -5,
               description = "Rechazado: aún cumple requerimientos mínimos; se mantiene en inventario operativo."
            },
            new
            {
               DecommissioningRequestId = -10,
               TechnicianId = -13,
               DeviceId = -13,
               EmissionDate = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.IrreparableTechnicalFailure,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -11,
               TechnicianId = -14,
               DeviceId = -14,
               EmissionDate = new DateTime(2025, 2, 18, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Approved,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = -20,
               IsApproved = (bool?)true,
               FinalDestinationDepartmentID = -18,
               logisticId = -1,
               description = "Aprobado: se reemplaza por plataforma nueva; se da de baja el servidor de monitoreo."
            },
            new
            {
               DecommissioningRequestId = -12,
               TechnicianId = -15,
               DeviceId = -15,
               EmissionDate = new DateTime(2025, 2, 22, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.ExcessiveRepairCost,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -13,
               TechnicianId = -16,
               DeviceId = -16,
               EmissionDate = new DateTime(2025, 2, 28, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 3, 3, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.IrreparableTechnicalFailure,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -2,
               description = "Rechazado: el diagnóstico indica fallas corregibles; se programa reparación."
            },
            new
            {
               DecommissioningRequestId = -14,
               TechnicianId = -12,
               DeviceId = -17,
               EmissionDate = new DateTime(2025, 3, 6, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -15,
               TechnicianId = -13,
               DeviceId = -18,
               EmissionDate = new DateTime(2025, 3, 9, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 3, 13, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.ExcessiveRepairCost,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -3,
               description = "Rechazado: la estimación de reparación se considera aceptable frente al reemplazo."
            },
            new
            {
               DecommissioningRequestId = -16,
               TechnicianId = -14,
               DeviceId = -19,
               EmissionDate = new DateTime(2025, 3, 14, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.TechnologicalObsolescence,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -17,
               TechnicianId = -15,
               DeviceId = -20,
               EmissionDate = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.IrreparableTechnicalFailure,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -18,
               TechnicianId = -16,
               DeviceId = -21,
               EmissionDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 4, 6, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Approved,
               Reason = DecommissioningReason.TechnologicalObsolescence,
               DeviceReceiverId = -17,
               IsApproved = (bool?)true,
               FinalDestinationDepartmentID = -18,
               logisticId = -4,
               description = "Aprobado: servidor legacy fuera de soporte (EOL). Baja y traslado a almacén."
            },
            new
            {
               DecommissioningRequestId = -19,
               TechnicianId = -12,
               DeviceId = -22,
               EmissionDate = new DateTime(2025, 4, 3, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Approved,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = -18,
               IsApproved = (bool?)true,
               FinalDestinationDepartmentID = -18,
               logisticId = -5,
               description = "Aprobado: reemplazo por router actualizado; equipo anterior pasa a baja."
            },
            new
            {
               DecommissioningRequestId = -20,
               TechnicianId = -13,
               DeviceId = -23,
               EmissionDate = new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 4, 9, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Approved,
               Reason = DecommissioningReason.SeverePhysicalDamage,
               DeviceReceiverId = -19,
               IsApproved = (bool?)true,
               FinalDestinationDepartmentID = -18,
               logisticId = -1,
               description = "Aprobado: UPS con daño irreparable en módulos de potencia."
            },
            new
            {
               DecommissioningRequestId = -21,
               TechnicianId = -14,
               DeviceId = -24,
               EmissionDate = new DateTime(2025, 4, 11, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -22,
               TechnicianId = -15,
               DeviceId = -25,
               EmissionDate = new DateTime(2025, 4, 13, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.ExcessiveRepairCost,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -23,
               TechnicianId = -16,
               DeviceId = -26,
               EmissionDate = new DateTime(2025, 4, 16, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 4, 19, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.PlannedTechnologyUpgrade,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -2,
               description = "Rechazado: el equipo de pruebas sigue vigente; se reubica a otro departamento."
            },
            new
            {
               DecommissioningRequestId = -24,
               TechnicianId = -12,
               DeviceId = -27,
               EmissionDate = new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.TechnologicalObsolescence,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -25,
               TechnicianId = -13,
               DeviceId = -28,
               EmissionDate = new DateTime(2025, 4, 22, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.IrreparableTechnicalFailure,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -26,
               TechnicianId = -14,
               DeviceId = -29,
               EmissionDate = new DateTime(2025, 4, 25, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = (DateTime?)null,
               Status = RequestStatus.Pending,
               Reason = DecommissioningReason.ExcessiveRepairCost,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)null,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = (int?)null,
               description = ""
            },
            new
            {
               DecommissioningRequestId = -27,
               TechnicianId = -15,
               DeviceId = -30,
               EmissionDate = new DateTime(2025, 4, 28, 0, 0, 0, DateTimeKind.Utc),
               AnswerDate = new DateTime(2025, 5, 2, 0, 0, 0, DateTimeKind.Utc),
               Status = RequestStatus.Rejected,
               Reason = DecommissioningReason.SeverePhysicalDamage,
               DeviceReceiverId = (int?)null,
               IsApproved = (bool?)false,
               FinalDestinationDepartmentID = (int?)null,
               logisticId = -3,
               description = "Rechazado: el daño es superficial; se autoriza reparación y retorno a operación."
            }
         );

         modelBuilder.Entity<Transfer>().HasData(
            new { TransferId = -1, DeviceId = -1, Date = new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -1, DestinationSectionId = -2, DeviceReceiverId = -17, Status = TransferStatus.InTransit, IsDisabled = false },
            new { TransferId = -2, DeviceId = -2, Date = new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -6, DestinationSectionId = -3, DeviceReceiverId = -18, Status = TransferStatus.Completed, IsDisabled = false },
            new { TransferId = -3, DeviceId = -5, Date = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -4, DestinationSectionId = -5, DeviceReceiverId = -19, Status = TransferStatus.Pending, IsDisabled = false },

            // Additional transfers
            new { TransferId = -4, DeviceId = -7, Date = new DateTime(2025, 1, 25, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -1, DestinationSectionId = -7, DeviceReceiverId = -20, Status = TransferStatus.Completed, IsDisabled = false },
            new { TransferId = -5, DeviceId = -8, Date = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -2, DestinationSectionId = -6, DeviceReceiverId = -21, Status = TransferStatus.InTransit, IsDisabled = false },
            new { TransferId = -6, DeviceId = -10, Date = new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -6, DestinationSectionId = -5, DeviceReceiverId = -19, Status = TransferStatus.Completed, IsDisabled = false },
            new { TransferId = -7, DeviceId = -14, Date = new DateTime(2025, 2, 19, 0, 0, 0, DateTimeKind.Utc), SourceSectionId = -8, DestinationSectionId = -6, DeviceReceiverId = -18, Status = TransferStatus.Pending, IsDisabled = false }
         );

         modelBuilder.Entity<ReceivingInspectionRequest>().HasData(
            // One inspection request per device (-1..-30)
            new { ReceivingInspectionRequestId = -1, DeviceId = -1, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -2, DeviceId = -2, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -3, DeviceId = -3, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.ExcessiveRepairCost },
            new { ReceivingInspectionRequestId = -4, DeviceId = -4, AdministratorId = -4, TechnicianId = -15, EmissionDate = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -5, DeviceId = -5, AdministratorId = -5, TechnicianId = -16, EmissionDate = new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -6, DeviceId = -6, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 9, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -7, DeviceId = -7, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -8, DeviceId = -8, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 1, 9, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 1, 11, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.IrreparableTechnicalFailure },
            new { ReceivingInspectionRequestId = -9, DeviceId = -9, AdministratorId = -4, TechnicianId = -15, EmissionDate = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -10, DeviceId = -10, AdministratorId = -5, TechnicianId = -16, EmissionDate = new DateTime(2025, 1, 11, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -11, DeviceId = -11, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 14, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -12, DeviceId = -12, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 1, 13, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.SeverePhysicalDamage },
            new { ReceivingInspectionRequestId = -13, DeviceId = -13, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 1, 14, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -14, DeviceId = -14, AdministratorId = -4, TechnicianId = -15, EmissionDate = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 17, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -15, DeviceId = -15, AdministratorId = -5, TechnicianId = -16, EmissionDate = new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -16, DeviceId = -16, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 17, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 1, 19, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.ExcessiveRepairCost },
            new { ReceivingInspectionRequestId = -17, DeviceId = -17, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 1, 18, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -18, DeviceId = -18, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 1, 19, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -19, DeviceId = -19, AdministratorId = -4, TechnicianId = -15, EmissionDate = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 22, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -20, DeviceId = -20, AdministratorId = -5, TechnicianId = -16, EmissionDate = new DateTime(2025, 1, 21, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 1, 23, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.TechnologicalObsolescence },
            new { ReceivingInspectionRequestId = -21, DeviceId = -21, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 22, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -22, DeviceId = -22, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 1, 23, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 25, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -23, DeviceId = -23, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 1, 24, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -24, DeviceId = -24, AdministratorId = -4, TechnicianId = -15, EmissionDate = new DateTime(2025, 1, 25, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 27, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -25, DeviceId = -25, AdministratorId = -5, TechnicianId = -16, EmissionDate = new DateTime(2025, 1, 26, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 1, 28, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.ExcessiveRepairCost },
            new { ReceivingInspectionRequestId = -26, DeviceId = -26, AdministratorId = -1, TechnicianId = -12, EmissionDate = new DateTime(2025, 1, 27, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -27, DeviceId = -27, AdministratorId = -2, TechnicianId = -13, EmissionDate = new DateTime(2025, 1, 28, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 1, 30, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -28, DeviceId = -28, AdministratorId = -3, TechnicianId = -14, EmissionDate = new DateTime(2025, 1, 29, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Pending, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade },
            new { ReceivingInspectionRequestId = -29, DeviceId = -29, AdministratorId = -4, TechnicianId = -15, EmissionDate = new DateTime(2025, 1, 30, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = (DateTime?)null, RejectionDate = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), Status = InspectionRequestStatus.Rejected, RejectReason = DecommissioningReason.IrreparableTechnicalFailure },
            new { ReceivingInspectionRequestId = -30, DeviceId = -30, AdministratorId = -5, TechnicianId = -16, EmissionDate = new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Utc), AcceptedDate = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc), RejectionDate = (DateTime?)null, Status = InspectionRequestStatus.Accepted, RejectReason = DecommissioningReason.PlannedTechnologyUpgrade }
         );

         modelBuilder.Entity<Rejection>().HasData(
            // Rejections aligned with approved decommissioning requests (receiver must exist)
            new { RejectionId = -1, DeviceReceiverId = -19, TechnicianId = -14, DeviceId = -4, DecommissioningRequestDate = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), RejectionDate = new DateTime(2025, 1, 23, 0, 0, 0, DateTimeKind.Utc) },
            new { RejectionId = -2, DeviceReceiverId = -18, TechnicianId = -12, DeviceId = -22, DecommissioningRequestDate = new DateTime(2025, 4, 3, 0, 0, 0, DateTimeKind.Utc), RejectionDate = new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc) }
         );

         modelBuilder.Entity<PerformanceRating>().HasData(
            new { PerformanceRatingId = -1, UserId = -7, TechnicianId = -12, Date = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), Score = 4.8, Description = "Excelente trabajo en red troncal" },
            new { PerformanceRatingId = -2, UserId = -8, TechnicianId = -13, Date = new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc), Score = 3.9, Description = "Buen manejo de servidores" },
            new { PerformanceRatingId = -3, UserId = -9, TechnicianId = -14, Date = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Utc), Score = 2.5, Description = "Mejorar tiempos de respuesta" },
            new { PerformanceRatingId = -4, UserId = -10, TechnicianId = -15, Date = new DateTime(2025, 4, 22, 0, 0, 0, DateTimeKind.Utc), Score = 4.2, Description = "Cumplimiento en ciberseguridad" },
            new { PerformanceRatingId = -5, UserId = -11, TechnicianId = -16, Date = new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), Score = 4.0, Description = "Trabajo constante en fibra" },

            // Additional ratings
            new { PerformanceRatingId = -6, UserId = -7, TechnicianId = -13, Date = new DateTime(2025, 6, 10, 0, 0, 0, DateTimeKind.Utc), Score = 4.3, Description = "Buen soporte en incidentes" },
            new { PerformanceRatingId = -7, UserId = -8, TechnicianId = -14, Date = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc), Score = 3.4, Description = "Necesita mejorar documentación" },
            new { PerformanceRatingId = -8, UserId = -9, TechnicianId = -15, Date = new DateTime(2025, 7, 2, 0, 0, 0, DateTimeKind.Utc), Score = 4.7, Description = "Excelente gestión de riesgos" },
            new { PerformanceRatingId = -9, UserId = -10, TechnicianId = -16, Date = new DateTime(2025, 7, 14, 0, 0, 0, DateTimeKind.Utc), Score = 3.8, Description = "Rendimiento estable" },
            new { PerformanceRatingId = -10, UserId = -11, TechnicianId = -12, Date = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc), Score = 4.1, Description = "Buen trabajo con tareas preventivas" }
         );
      }
   }
}
