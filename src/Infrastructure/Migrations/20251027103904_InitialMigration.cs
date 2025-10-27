using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    TechnicianID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    PerformanceRatingID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => new { x.UserID, x.TechnicianID, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "DecommissioningRequests",
                columns: table => new
                {
                    TechnicianID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DecommissioningRequestID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecommissioningRequests", x => new { x.TechnicianID, x.DeviceID, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "Decommissionings",
                columns: table => new
                {
                    DecommissioningRequestID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DecommissioningID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false),
                    DecommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reason = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalDestination = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiverDepartmentID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decommissionings", x => x.DecommissioningRequestID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SectionID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationalState = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceID);
                });

            migrationBuilder.CreateTable(
                name: "Mainteinances",
                columns: table => new
                {
                    TechnicianID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Cost = table.Column<double>(type: "REAL", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    MaintenanceRecordID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mainteinances", x => new { x.TechnicianID, x.DeviceID, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "ReceivingInspectionRequests",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false),
                    AdministratorID = table.Column<int>(type: "INTEGER", nullable: false),
                    TechnicianID = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceivingInspectionRequestID = table.Column<int>(type: "INTEGER", nullable: false),
                    EmissionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcceptedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RejectionDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingInspectionRequests", x => new { x.DeviceID, x.AdministratorID, x.TechnicianID });
                });

            migrationBuilder.CreateTable(
                name: "Rejections",
                columns: table => new
                {
                    DeviceReceiverID = table.Column<int>(type: "INTEGER", nullable: false),
                    TechnicianID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false),
                    RejectionID = table.Column<int>(type: "INTEGER", nullable: false),
                    DecommissioningRequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RejectionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejections", x => new { x.DeviceReceiverID, x.TechnicianID, x.DeviceID });
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionID);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SourceSectionID = table.Column<int>(type: "INTEGER", nullable: false),
                    DestinationSectionID = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    SectionID = table.Column<int>(type: "INTEGER", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    Specialty = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.InsertData(
                table: "DecommissioningRequests",
                columns: new[] { "Date", "DeviceID", "TechnicianID", "DecommissioningRequestID", "DeviceReceiverID" },
                values: new object[,]
                {
                    { new DateTime(2025, 10, 22, 0, 0, 0, 0, DateTimeKind.Local), -4, -15, -2, -20 },
                    { new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Local), -3, -14, -1, -19 }
                });

            migrationBuilder.InsertData(
                table: "Decommissionings",
                columns: new[] { "DecommissioningRequestID", "DecommissioningDate", "DecommissioningID", "DeviceID", "DeviceReceiverID", "FinalDestination", "Reason", "ReceiverDepartmentID" },
                values: new object[] { -2, new DateTime(2025, 10, 22, 0, 0, 0, 0, DateTimeKind.Local), -1, -4, -20, "Reciclaje", 4, -23 });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentID", "Name", "SectionID" },
                values: new object[,]
                {
                    { -24, "Análisis Forense y Respuesta a Incidentes", -8 },
                    { -23, "Monitorización de Amenazas y SOC", -8 },
                    { -22, "Arquitectura y Gestión de Firewalls", -8 },
                    { -21, "Gestión de Activos y Red Local", -7 },
                    { -20, "Comunicaciones Unificadas y Telefonía IP", -7 },
                    { -19, "Soporte al Usuario y Helpdesk", -7 },
                    { -18, "Gestión de Inventario y Distribución", -6 },
                    { -17, "Reparación y Refabricación", -6 },
                    { -16, "Recepción y Diagnóstico Técnico", -6 },
                    { -15, "Operaciones Cloud y Escalabilidad", -5 },
                    { -14, "Plataforma como Servicio", -5 },
                    { -13, "Infraestructura como Servicio", -5 },
                    { -12, "Mediciones y Certificación de Red", -4 },
                    { -11, "Despliegue de Fibra Óptica y Acceso", -4 },
                    { -10, "Diseño y Ingeniería de Red", -4 },
                    { -9, "Soporte a Nodos Remotos", -3 },
                    { -8, "Mantenimiento Correctivo y Urgencias", -3 },
                    { -7, "Instalaciones y Activaciones", -3 },
                    { -6, "Infraestructura Física y Climatización", -2 },
                    { -5, "Almacenamiento y Backup", -2 },
                    { -4, "Servidores y Virtualización", -2 },
                    { -3, "Reparaciones de Red", -1 },
                    { -2, "Seguridad Perimetral y Firewalls", -1 },
                    { -1, "Conmutación y Enrutamiento Avanzado", -1 }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceID", "AcquisitionDate", "DepartmentID", "Name", "OperationalState", "Type" },
                values: new object[,]
                {
                    { -6, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Local), -14, "Analizador de Espectro Viavi", 0, 4 },
                    { -5, new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), -12, "Antena de Radioenlace AirFiber 5XHD", 0, 3 },
                    { -4, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Local), -24, "Sistema UPS Eaton 20kVA", 2, 2 },
                    { -3, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Local), -24, "Firewall de Próxima Generación PA-5200", 0, 0 },
                    { -2, new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), -16, "Servidor de Virtualización HP DL380", 1, 1 },
                    { -1, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Local), -8, "Router de Agregación ASR 9000", 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "Mainteinances",
                columns: new[] { "Date", "DeviceID", "TechnicianID", "Cost", "MaintenanceRecordID", "Type" },
                values: new object[,]
                {
                    { new DateOnly(2020, 8, 24), -5, -16, 0.0, -5, "Correctivo" },
                    { new DateOnly(2021, 5, 11), -4, -15, 10.5, -4, "Preventivo" },
                    { new DateOnly(2022, 7, 18), -5, -14, 0.0, -6, "Correctivo" },
                    { new DateOnly(2022, 10, 10), -3, -14, 20.0, -3, "Correctivo" },
                    { new DateOnly(2022, 5, 13), -2, -13, 100.0, -2, "Correctivo" },
                    { new DateOnly(2020, 12, 30), -1, -12, 0.0, -1, "Preventivo" }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionID", "Name" },
                values: new object[,]
                {
                    { -8, "Seguridad Informática (Ciberseguridad)" },
                    { -7, "Infraestructura Interna (TI Interno)" },
                    { -6, "Taller Central y Logística" },
                    { -5, "División de Servicios en la Nube (Cloud)" },
                    { -4, "Planificación y Despliegue de Red" },
                    { -3, "Soporte Técnico en Campo" },
                    { -2, "Infraestructura de Centro de Datos (Data Center)" },
                    { -1, "Operaciones de Red Corporativa" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash" },
                values: new object[,]
                {
                    { -21, -17, "DeviceReceiver", "Carlos Ruiz", "rec05" },
                    { -20, -24, "DeviceReceiver", "Marta Jiménez", "rec04" },
                    { -19, -6, "DeviceReceiver", "Luis Fernández", "rec03" },
                    { -18, -2, "DeviceReceiver", "Ana García", "rec02" },
                    { -17, -9, "DeviceReceiver", "Miguel Ángel Santos", "rec01" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash", "Specialty", "YearsOfExperience" },
                values: new object[,]
                {
                    { -16, -11, "Technician", "Ana López", "tech05", "Fibra Óptica", 6 },
                    { -15, -22, "Technician", "María Ortega", "tech04", "Ciberseguridad", 4 },
                    { -14, -9, "Technician", "Jorge Silva", "tech03", "Electricidad y Energía", 7 },
                    { -13, -6, "Technician", "Eduardo Vargas", "tech02", "Servidores y Virtualización", 3 },
                    { -12, -3, "Technician", "Carlos Méndez", "tech01", "Redes y Comunicaciones", 5 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash", "SectionID" },
                values: new object[,]
                {
                    { -11, -13, "SectionManager", "Isabel Castro", "manager05", -5 },
                    { -10, -10, "SectionManager", "Ricardo Díaz", "manager04", -4 },
                    { -9, -7, "SectionManager", "Patricia Herrera", "manager03", -3 },
                    { -8, -6, "SectionManager", "Alejandro Torres", "manager02", -2 },
                    { -7, -1, "SectionManager", "Sofía Ramírez", "manager01", -1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash" },
                values: new object[,]
                {
                    { -6, -24, "Director", "Elena Morales", "dir123" },
                    { -5, -3, "Administrator", "Roberto López", "admin05" },
                    { -4, -5, "Administrator", "Carmen Sánchez", "admin04" },
                    { -3, -18, "Administrator", "Javier Rodríguez", "admin03" },
                    { -2, -18, "Administrator", "Laura Martínez", "admin02" },
                    { -1, -21, "Administrator", "David González", "admin01" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "DecommissioningRequests");

            migrationBuilder.DropTable(
                name: "Decommissionings");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Mainteinances");

            migrationBuilder.DropTable(
                name: "ReceivingInspectionRequests");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
