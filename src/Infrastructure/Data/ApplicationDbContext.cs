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
        public DbSet<DeviceReceiver> EquipmentReceivers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

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
                .HasKey(r => new { r.EquipmentID, r.AdministratorID, r.TechnicianID });

            modelBuilder.Entity<Rejection>()
                .HasKey(r => new { r.EquipmentReceiverID, r.TechnicianID, r.EquipmentID });

            modelBuilder.Entity<Transfer>()
                .HasKey(t => t.TransferID);

            modelBuilder.Entity<MaintenanceRecord>()
                .HasKey(m => new { m.TechnicianID, m.DeviceID, m.Date });

            // Configuraciones para User y sus herederos
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<Equipment>()
                .HasKey(e => e.EquipmentID);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.DepartmentID);

            modelBuilder.Entity<Section>()
                .HasKey(s => s.SectionID);

            // Definir GUIDs constantes para las referencias
            var deptTI = Guid.Parse("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8");
            var deptProduccion = Guid.Parse("b4c493f9-6b62-48f4-b293-28e30b3d77a8");
            var deptMantenimiento = Guid.Parse("1c0c1962-b336-42bf-a9e0-7b1098da51c4");
            var deptLogistica = Guid.Parse("5abcde20-13fa-416f-85b9-ad1a00ca5959");

            var sectDesarrollo = Guid.Parse("2a48f950-9a2d-42e4-b324-d510c101247a");
            var sectInfraestructura = Guid.Parse("3f9edad3-41fd-47df-b0af-0f7cc9c28de7");
            var sectEnsamblaje = Guid.Parse("53a63a6f-eecc-4a53-83e1-66d8a972cb52");
            var sectControlCalidad = Guid.Parse("64e12757-0e8d-4b2f-98be-234c37d44553");
            var sectMantElectrico = Guid.Parse("75f23a48-a4b2-4cc9-8c09-f05778d559fd");
            var sectMantMecanico = Guid.Parse("86a34c3a-5bb1-4dad-9c0f-77d1469820de");

            // GUIDs para los usuarios
            var adminID = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var directorID = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var sectionManager1ID = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var sectionManager2ID = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var technician1ID = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var technician2ID = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var receiverID = Guid.Parse("77777777-7777-7777-7777-777777777777");

            // GUIDs para equipos
            var equip1ID = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var equip2ID = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var equip3ID = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
            var equip4ID = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

            // GUIDs para agregaciones
            var transfer1ID = Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1");
            var maintainance1ID = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1");
            var maintainance2ID = Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2");
            var decommissioningReqID = Guid.Parse("abcdef01-abcd-abcd-abcd-abcdef012345");
            var decommissioningID = Guid.Parse("fedcba98-fedc-fedc-fedc-fedcba987654");

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
                new Section(sectDesarrollo, "Desarrollo de Software") { SectionID = sectDesarrollo, Name = "Desarrollo de Software" },
                new Section(sectInfraestructura, "Infraestructura") { SectionID = sectInfraestructura, Name = "Infraestructura" },
                new Section(sectEnsamblaje, "Línea de Ensamblaje") { SectionID = sectEnsamblaje, Name = "Línea de Ensamblaje" },
                new Section(sectControlCalidad, "Control de Calidad") { SectionID = sectControlCalidad, Name = "Control de Calidad" },
                new Section(sectMantElectrico, "Mantenimiento Eléctrico") { SectionID = sectMantElectrico, Name = "Mantenimiento Eléctrico" },
                new Section(sectMantMecanico, "Mantenimiento Mecánico") { SectionID = sectMantMecanico, Name = "Mantenimiento Mecánico" }
            );

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentID = deptTI, SectionID = sectDesarrollo },
                new Department { DepartmentID = deptProduccion, SectionID = sectEnsamblaje },
                new Department { DepartmentID = deptMantenimiento, SectionID = sectMantElectrico },
                new Department { DepartmentID = deptLogistica, SectionID = sectControlCalidad }
            );

            // Seed Users
            // Administrator
            modelBuilder.Entity<Administrator>().HasData(
                new Administrator("Administrador Principal", "AdminHash123", deptTI)
                {
                    UserID = adminID,
                    FullName = "Administrador Principal",
                    PasswordHash = "AdminHash123",
                    DepartmentID = deptTI
                }
            );

            // Director
            modelBuilder.Entity<Director>().HasData(
                new Director("Director General", "DirHash123", deptTI)
                {
                    UserID = directorID,
                    FullName = "Director General",
                    PasswordHash = "DirHash123",
                    DepartmentID = deptTI
                }
            );

            // SectionManagers
            modelBuilder.Entity<SectionManager>().HasData(
                new SectionManager("Gerente Desarrollo", "GerHash123", deptTI, sectDesarrollo)
                {
                    UserID = sectionManager1ID,
                    FullName = "Gerente Desarrollo",
                    PasswordHash = "GerHash123",
                    DepartmentID = deptTI,
                    SectionID = sectDesarrollo
                },
                new SectionManager("Gerente Producción", "GerHash456", deptProduccion, sectEnsamblaje)
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
                new Technician("Técnico Informática", "TecHash123", deptTI, 5, "Redes")
                {
                    UserID = technician1ID,
                    FullName = "Técnico Informática",
                    PasswordHash = "TecHash123",
                    DepartmentID = deptTI,
                    YearsOfExperience = 5,
                    Specialty = "Redes"
                },
                new Technician("Técnico Eléctrico", "TecHash456", deptMantenimiento, 8, "Electricidad Industrial")
                {
                    UserID = technician2ID,
                    FullName = "Técnico Eléctrico",
                    PasswordHash = "TecHash456",
                    DepartmentID = deptMantenimiento,
                    YearsOfExperience = 8,
                    Specialty = "Electricidad Industrial"
                }
            );

            // EquipmentReceiver
            modelBuilder.Entity<DeviceReceiver>().HasData(
                new EquipmentReceiver("Receptor Equipos", "RecHash123", deptLogistica)
                {
                    UserID = receiverID,
                    FullName = "Receptor Equipos",
                    PasswordHash = "RecHash123",
                    DepartmentID = deptLogistica
                }
            );

            // Seed Equipment
            modelBuilder.Entity<Equipment>().HasData(
                new Equipment
                {
                    EquipmentID = equip1ID,
                    Name = "Servidor Principal",
                    Type = DeviceType.Informatical,
                    OperationalState = default(OperationalState),
                    DepartmentID = deptTI,
                    AcquisitionDate = today.AddYears(-2)
                },
                new Equipment
                {
                    EquipmentID = equip2ID,
                    Name = "Computadora Desarrollo",
                    Type = DeviceType.Informatical,
                    OperationalState = default(OperationalState),
                    DepartmentID = deptTI,
                    AcquisitionDate = today.AddMonths(-8)
                },
                new Equipment
                {
                    EquipmentID = equip3ID,
                    Name = "Máquina Ensamblaje",
                    Type = DeviceType.Electrical,
                    OperationalState = default(OperationalState),
                    DepartmentID = deptProduccion,
                    AcquisitionDate = today.AddYears(-1)
                },
                new Equipment
                {
                    EquipmentID = equip4ID,
                    Name = "Sistema de Comunicación",
                    Type = DeviceType.Comunicational,
                    OperationalState = default(OperationalState),
                    DepartmentID = deptLogistica,
                    AcquisitionDate = today.AddMonths(-5)
                }
            );

            // Seed Mainteinance - Usa DateOnly creados correctamente
            modelBuilder.Entity<MaintenanceRecord>().HasData(
                new Mainteinance(technician1ID, equip1ID, DateOnly.FromDateTime(dateInFifteenDays), 500.00, "Preventivo")
                {
                    TechnicianID = technician1ID,
                    DeviceID = equip1ID,
                    Date = DateOnly.FromDateTime(dateInFifteenDays),
                    Cost = 500.00,
                    Type = "Preventivo"
                },
                new Mainteinance(technician2ID, equip3ID, DateOnly.FromDateTime(dateInThreeDays), 1200.00, "Correctivo")
                {
                    TechnicianID = technician2ID,
                    DeviceID = equip3ID,
                    Date = DateOnly.FromDateTime(dateInThreeDays),
                    Cost = 1200.00,
                    Type = "Correctivo"
                }
            );

            // Seed DecommissioningRequest
            modelBuilder.Entity<DecommissioningRequest>().HasData(
                new DecommissioningRequest(technician2ID, equip3ID, dateInFifteenDaysBefore)
                {
                    TechnicianID = technician2ID,
                    DeviceID = equip3ID,
                    Date = dateInFifteenDaysBefore,
                    EquipmentReceiverID = receiverID
                }
            );

            // Seed Decommissioning
            modelBuilder.Entity<Decommissioning>().HasData(
                new Decommissioning(
                    decommissioningReqID,
                    receiverID,
                    equip3ID,
                    dateInFiveDaysBefore,
                    dateInFifteenDaysBefore,
                    "Equipo obsoleto",
                    "Donación a institución educativa")
                {
                    DecommissioningRequestID = decommissioningReqID,
                    DeviceReceiverID = receiverID,
                    EquipmentID = equip3ID,
                    DecommissioningDate = dateInFiveDaysBefore,
                    RequestDate = dateInFifteenDaysBefore,
                    DepartmentID = deptProduccion,
                    Reason = "Equipo obsoleto",
                    FinalDestination = "Donación a institución educativa"
                }
            );

            // Seed ReceivingInspectionRequest
            modelBuilder.Entity<ReceivingInspectionRequest>().HasData(
                new ReceivingInspectionRequest(equip2ID, adminID, technician1ID, dateInTwentyDaysBefore)
                {
                    EquipmentID = equip2ID,
                    AdministratorID = adminID,
                    TechnicianID = technician1ID,
                    EmissionDate = dateInTwentyDaysBefore,
                    AcceptedDate = dateInEighteenDaysBefore,
                    RejectionDate = null
                }
            );

            // Seed Rejection
            modelBuilder.Entity<Rejection>().HasData(
                new Rejection(receiverID, technician1ID, equip4ID, dateInTwentyDaysBefore, dateInFifteenDaysBefore)
                {
                    EquipmentReceiverID = receiverID,
                    TechnicianID = technician1ID,
                    EquipmentID = equip4ID,
                    DecommissioningRequestDate = dateInTwentyDaysBefore,
                    RejectionDate = dateInFifteenDaysBefore
                }
            );

            // Seed Assessments
            modelBuilder.Entity<PerformanceRating>().HasData(
                new Assessments(directorID, technician1ID, dateInTwentyDaysBefore, 4.5)
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
