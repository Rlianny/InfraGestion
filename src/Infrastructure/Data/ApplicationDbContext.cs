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


            // Creating seed data

            Section section01 = new Section("Operaciones de Red Corporativa");
            Section section02 = new Section("Infraestructura de Centro de Datos (Data Center)");
            Section section03 = new Section("Soporte Técnico en Campo");
            Section section04 = new Section("Planificación y Despliegue de Red");
            Section section05 = new Section("División de Servicios en la Nube (Cloud)");
            Section section06 = new Section("Taller Central y Logística");
            Section section07 = new Section("Infraestructura Interna (TI Interno)");
            Section section08 = new Section("Seguridad Informática (Ciberseguridad)");
            Section section09 = new Section("Dirección General");

            // Seed Sections
            modelBuilder.Entity<Section>().HasData(
                section01,
                section02,
                section03,
                section04,
                section05,
                section06,
                section07,
                section08
            );

            Department department011 = new Department("Conmutación y Enrutamiento Avanzado", section01.SectionID);
            Department department012 = new Department("Seguridad Perimetral y Firewalls", section01.SectionID);
            Department department013 = new Department("Reparaciones de Red", section01.SectionID);

            Department department021 = new Department("Servidores y Virtualización", section02.SectionID);
            Department department022 = new Department("Almacenamiento y Backup", section02.SectionID);
            Department department023 = new Department("Infraestructura Física y Climatización", section02.SectionID);

            Department department031 = new Department("Instalaciones y Activaciones", section03.SectionID);
            Department department032 = new Department("Mantenimiento Correctivo y Urgencias", section03.SectionID);
            Department department033 = new Department("Soporte a Nodos Remotos", section03.SectionID);

            Department department041 = new Department("Diseño y Ingeniería de Red", section04.SectionID);
            Department department042 = new Department("Despliegue de Fibra Óptica y Acceso", section04.SectionID);
            Department department043 = new Department("Mediciones y Certificación de Red", section04.SectionID);

            Department department051 = new Department("Infraestructura como Servicio", section05.SectionID);
            Department department052 = new Department("Plataforma como Servicio", section05.SectionID);
            Department department053 = new Department("Operaciones Cloud y Escalabilidad", section05.SectionID);

            Department department061 = new Department("Recepción y Diagnóstico Técnico", section06.SectionID);
            Department department062 = new Department("Reparación y Refabricación", section06.SectionID);
            Department department063 = new Department("Gestión de Inventario y Distribución", section06.SectionID);

            Department department071 = new Department("Soporte al Usuario y Helpdesk", section07.SectionID);
            Department department072 = new Department("Comunicaciones Unificadas y Telefonía IP", section07.SectionID);
            Department department073 = new Department("Gestión de Activos y Red Local", section07.SectionID);

            Department department081 = new Department("Arquitectura y Gestión de Firewalls", section08.SectionID);
            Department department082 = new Department("Monitorización de Amenazas y SOC", section08.SectionID);
            Department department083 = new Department("Análisis Forense y Respuesta a Incidentes", section08.SectionID);

            Department department091 = new Department("Planificación Estratégica", section09.SectionID);
            Department department092 = new Department("Gestión de Riesgos", section09.SectionID);
            Department department093 = new Department("Relaciones Institucionales", section09.SectionID);

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(

                department011, department012, department013,
                department021, department022, department023,
                department031, department032, department033,
                department041, department042, department043,
                department051, department052, department053,
                department061, department062, department063,
                department071, department072, department073,
                department081, department082, department083
            );

            // Seed Users

            Administrator administrator01 = new Administrator("David González", "admin01", department073.DepartmentID);
            Administrator administrator02 = new Administrator("Laura Martínez", "admin02", department063.DepartmentID);
            Administrator administrator03 = new Administrator("Javier Rodríguez", "admin03", department063.DepartmentID);
            Administrator administrator04 = new Administrator("Carmen Sánchez", "admin04", department022.DepartmentID);
            Administrator administrator05 = new Administrator("Roberto López", "admin05", department013.DepartmentID);

            // Administrator
            modelBuilder.Entity<Administrator>().HasData(
                administrator01, administrator02, administrator03, administrator04, administrator05
            );

            Director director = new Director("Elena Morales", "dir123", department093.DepartmentID);

            // Director
            modelBuilder.Entity<Director>().HasData(
                director
            );

            SectionManager sectionManager01 = new SectionManager("Sofía Ramírez", "manager01", department011.DepartmentID);
            SectionManager sectionManager02 = new SectionManager("Alejandro Torres", "manager02", department023.DepartmentID);
            SectionManager sectionManager03 = new SectionManager("Patricia Herrera", "manager03", department031.DepartmentID);
            SectionManager sectionManager04 = new SectionManager("Ricardo Díaz", "manager04", department041.DepartmentID);
            SectionManager sectionManager05 = new SectionManager("Isabel Castro", "manager05", department051.DepartmentID);

            // SectionManagers
            modelBuilder.Entity<SectionManager>().HasData(
                sectionManager01,
                sectionManager02,
                sectionManager03,
                sectionManager04,
                sectionManager05
            );

            Technician technician01 = new Technician("Carlos Méndez", "tech01", department013, 5, "Redes y Comunicaciones");
            Technician technician02 = new Technician("Eduardo Vargas", "tech02", department023, 3, "Servidores y Virtualización");
            Technician technician03 = new Technician("Jorge Silva", "tech03", department033, 7, "Electricidad y Energía");
            Technician technician04 = new Technician("María Ortega", "tech04", department081, 4, "Ciberseguridad");
            Technician technician05 = new Technician("Ana López", "tech05", department042, 6, "Fibra Óptica");

            // Technicians
            modelBuilder.Entity<Technician>().HasData(
                technician01, technician02, technician03, technician04, technician05
            );

            DeviceReceiver receiver01 = new DeviceReceiver("Miguel Ángel Santos", "rec01", department033.DepartmentID);
            DeviceReceiver receiver02 = new DeviceReceiver("Ana García", "rec02", department012.DepartmentID);
            DeviceReceiver receiver03 = new DeviceReceiver("Luis Fernández", "rec03", department023.DepartmentID);
            DeviceReceiver receiver04 = new DeviceReceiver("Marta Jiménez", "rec04", department093.DepartmentID);
            DeviceReceiver receiver05 = new DeviceReceiver("Carlos Ruiz", "rec05", department062.DepartmentID);

            // EquipmentReceiver
            modelBuilder.Entity<DeviceReceiver>().HasData(
                receiver01, receiver02, receiver03, receiver04, receiver05
            );

            Device device01 = new Device("Router de Agregación ASR 9000", DeviceType.ConnectivityAndNetwork, OperationalState.Operational, department032.DepartmentID, dateInEighteenDaysBefore);
            Device device02 = new Device("Servidor de Virtualización HP DL380", DeviceType.ComputingAndIT, OperationalState.UnderMaintenance, department061.DepartmentID, today);
            Device device03 = new Device("Firewall de Próxima Generación PA-5200", DeviceType.ConnectivityAndNetwork, OperationalState.Operational, department083.DepartmentID, dateInEighteenDaysBefore);
            Device device04 = new Device("Sistema UPS Eaton 20kVA", DeviceType.ElectricalInfrastructureAndSupport, OperationalState.Decommissioned, department091.DepartmentID, dateInEighteenDaysBefore);
            Device device05 = new Device("Antena de Radioenlace AirFiber 5XHD", DeviceType.CommunicationsAndTransmission, OperationalState.Operational, department043.DepartmentID, today);
            Device device06 = new Device("Analizador de Espectro Viavi", DeviceType.DiagnosticAndMeasurement, OperationalState.Operational, department052.DepartmentID, dateInEighteenDaysBefore);


            // Seed Devices
            modelBuilder.Entity<Device>().HasData(
                device01, device02, device03, device04, device05, device06
            );

            MaintenanceRecord maintenance01 = new MaintenanceRecord(technician01.UserID, device01.DeviceID, new DateOnly(2020, 12, 30), 0, "Preventivo");
            MaintenanceRecord maintenance02 = new MaintenanceRecord(technician02.UserID, device02.DeviceID, new DateOnly(2022, 5, 13), 100, "Correctivo");
            MaintenanceRecord maintenance03 = new MaintenanceRecord(technician03.UserID, device03.DeviceID, new DateOnly(2022, 10, 10), 20, "Correctivo");
            MaintenanceRecord maintenance04 = new MaintenanceRecord(technician04.UserID, device04.DeviceID, new DateOnly(2021, 5, 11), 10.5, "Preventivo");
            MaintenanceRecord maintenance05 = new MaintenanceRecord(technician05.UserID, device05.DeviceID, new DateOnly(2020, 8, 24), 0, "Correctivo");
            MaintenanceRecord maintenance06 = new MaintenanceRecord(technician03.UserID, device05.DeviceID, new DateOnly(2022, 7, 18), 0, "Correctivo");

            // Seed Mainteinance - Usa DateOnly creados correctamente
            modelBuilder.Entity<MaintenanceRecord>().HasData(
                maintenance01, maintenance02, maintenance03, maintenance04, maintenance05, maintenance06
            );

            DecommissioningRequest request01 = new DecommissioningRequest(technician03.UserID, device03.DeviceID, receiver03.UserID, dateInFiveDaysBefore);
            DecommissioningRequest request02 = new DecommissioningRequest(technician04.UserID, device04.DeviceID, receiver04.UserID, dateInFiveDaysBefore);

            // Seed DecommissioningRequest
            modelBuilder.Entity<DecommissioningRequest>().HasData(
                request01,
                request02
            );

            Decommissioning decommissioning = new Decommissioning(device04.DeviceID, request02.DecommissioningRequestID, receiver04.UserID, department082.DepartmentID, dateInFiveDaysBefore, DecommissioningReason.SeverePhysicalDamage, "Reciclaje");

            // Seed Decommissioning
            modelBuilder.Entity<Decommissioning>().HasData(
                decommissioning
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
