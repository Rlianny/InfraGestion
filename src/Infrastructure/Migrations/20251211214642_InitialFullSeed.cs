using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialFullSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceivingInspectionRequests",
                columns: table => new
                {
                    ReceivingInspectionRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdministratorId = table.Column<int>(type: "INTEGER", nullable: false),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmissionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcceptedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RejectionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    RejectReason = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingInspectionRequests", x => x.ReceivingInspectionRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Rejections",
                columns: table => new
                {
                    RejectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    DecommissioningRequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RejectionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejections", x => x.RejectionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SourceSectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DestinationSectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferId);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    PerformanceRatingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.PerformanceRatingId);
                });

            migrationBuilder.CreateTable(
                name: "DecommissioningRequests",
                columns: table => new
                {
                    DecommissioningRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecommissioningRequests", x => x.DecommissioningRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationalState = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    Specialty = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mainteinances",
                columns: table => new
                {
                    MaintenanceRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cost = table.Column<double>(type: "REAL", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mainteinances", x => x.MaintenanceRecordId);
                    table.ForeignKey(
                        name: "FK_Mainteinances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SectionManagerId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_Sections_Users_SectionManagerId",
                        column: x => x.SectionManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "DecommissioningRequests",
                columns: new[] { "DecommissioningRequestId", "Date", "DeviceId", "DeviceReceiverId", "IsApproved", "Reason", "Status", "TechnicianId", "UserId" },
                values: new object[] { -2, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), -3, -18, false, 1, 0, -15, null });

            migrationBuilder.InsertData(
                table: "Mainteinances",
                columns: new[] { "MaintenanceRecordId", "Cost", "Date", "Description", "DeviceId", "TechnicianId", "Type", "UserId" },
                values: new object[,]
                {
                    { -6, 60.0, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Revisión de calibración del analizador", -6, -12, 0, null },
                    { -5, 150.0, new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste de antena y verificación de potencia", -5, -16, 2, null },
                    { -4, 80.0, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Calibración y reemplazo de baterías UPS", -4, -15, 1, null },
                    { -3, 210.0, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Actualización de firmware y pruebas de firewall", -3, -14, 0, null },
                    { -2, 450.0, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Reemplazo de módulo RAID", -2, -13, 1, null },
                    { -1, 120.0, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Chequeo de enlaces y limpieza de puertos", -1, -12, 0, null }
                });

            migrationBuilder.InsertData(
                table: "ReceivingInspectionRequests",
                columns: new[] { "ReceivingInspectionRequestId", "AcceptedDate", "AdministratorId", "DeviceId", "EmissionDate", "RejectReason", "RejectionDate", "Status", "TechnicianId" },
                values: new object[,]
                {
                    { -3, null, -3, -3, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, -14 },
                    { -2, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), -2, -2, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -13 },
                    { -1, null, -1, -1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, 2, -12 }
                });

            migrationBuilder.InsertData(
                table: "Rejections",
                columns: new[] { "RejectionId", "DecommissioningRequestDate", "DeviceId", "DeviceReceiverId", "RejectionDate", "TechnicianId" },
                values: new object[,]
                {
                    { -2, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), -5, -17, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), -16 },
                    { -1, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), -3, -19, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), -15 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Technician" },
                    { 2, "EquipmentReceiver" },
                    { 3, "SectionManager" },
                    { 4, "Administrator" },
                    { 5, "Director" },
                    { 6, "Logistician" }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionId", "IsDisabled", "Name", "SectionManagerId" },
                values: new object[,]
                {
                    { -8, false, "Seguridad Informática (Ciberseguridad)", null },
                    { -7, false, "Infraestructura Interna (TI Interno)", null },
                    { -6, false, "Taller Central y Logística", null },
                    { -5, false, "División de Servicios en la Nube (Cloud)", null },
                    { -4, false, "Planificación y Despliegue de Red", null },
                    { -3, false, "Soporte Técnico en Campo", null },
                    { -2, false, "Infraestructura de Centro de Datos (Data Center)", null },
                    { -1, false, "Operaciones de Red Corporativa", null }
                });

            migrationBuilder.InsertData(
                table: "Transfers",
                columns: new[] { "TransferId", "Date", "DestinationSectionId", "DeviceId", "DeviceReceiverId", "IsDisabled", "SourceSectionId", "Status" },
                values: new object[,]
                {
                    { -3, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), -5, -5, -19, false, -4, 2 },
                    { -2, new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), -3, -2, -18, false, -6, 4 },
                    { -1, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), -2, -1, -17, false, -1, 3 }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "IsDisabled", "Name", "SectionId" },
                values: new object[,]
                {
                    { -24, false, "Análisis Forense y Respuesta a Incidentes", -8 },
                    { -23, false, "Monitorización de Amenazas y SOC", -8 },
                    { -22, false, "Arquitectura y Gestión de Firewalls", -8 },
                    { -21, false, "Gestión de Activos y Red Local", -7 },
                    { -20, false, "Comunicaciones Unificadas y Telefonía IP", -7 },
                    { -19, false, "Soporte al Usuario y Helpdesk", -7 },
                    { -18, false, "Gestión de Inventario y Distribución", -6 },
                    { -17, false, "Reparación y Refabricación", -6 },
                    { -16, false, "Recepción y Diagnóstico Técnico", -6 },
                    { -15, false, "Operaciones Cloud y Escalabilidad", -5 },
                    { -14, false, "Plataforma como Servicio", -5 },
                    { -13, false, "Infraestructura como Servicio", -5 },
                    { -12, false, "Mediciones y Certificación de Red", -4 },
                    { -11, false, "Despliegue de Fibra Óptica y Acceso", -4 },
                    { -10, false, "Diseño y Ingeniería de Red", -4 },
                    { -9, false, "Soporte a Nodos Remotos", -3 },
                    { -8, false, "Mantenimiento Correctivo y Urgencias", -3 },
                    { -7, false, "Instalaciones y Activaciones", -3 },
                    { -6, false, "Infraestructura Física y Climatización", -2 },
                    { -5, false, "Almacenamiento y Backup", -2 },
                    { -4, false, "Servidores y Virtualización", -2 },
                    { -3, false, "Reparaciones de Red", -1 },
                    { -2, false, "Seguridad Perimetral y Firewalls", -1 },
                    { -1, false, "Conmutación y Enrutamiento Avanzado", -1 },
                    { 1, false, "Almacen General", -1 }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "AcquisitionDate", "DepartmentId", "IsDisabled", "Name", "OperationalState", "Type" },
                values: new object[,]
                {
                    { -6, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -14, false, "Analizador de Espectro Viavi", 2, 4 },
                    { -5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "Antena de Radioenlace AirFiber 5XHD", 2, 3 },
                    { -4, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -24, false, "Sistema UPS Eaton 20kVA", 4, 2 },
                    { -3, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -24, false, "Firewall de Próxima Generación PA-5200", 2, 0 },
                    { -2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -16, false, "Servidor de Virtualización HP DL380", 3, 1 },
                    { -1, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -8, false, "Router de Agregación ASR 9000", 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "DepartmentId", "FullName", "IsActive", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Specialty", "Username", "YearsOfExperience" },
                values: new object[,]
                {
                    { -21, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -17, "Carlos Ruiz", true, "rec05", null, null, 2, null, "cruiz", null },
                    { -20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -24, "Marta Jiménez", true, "rec04", null, null, 2, null, "mjimenez", null },
                    { -19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -6, "Luis Fernández", true, "rec03", null, null, 2, null, "lfernandez", null },
                    { -18, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -2, "Ana García", true, "rec02", null, null, 2, null, "agarcia", null },
                    { -17, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -9, "Miguel Ángel Santos", true, "rec01", null, null, 2, null, "msantos", null },
                    { -16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -11, "Ana López", true, "tech05", null, null, 1, "Fibra Óptica", "alopez", 6 },
                    { -15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -22, "María Ortega", true, "tech04", null, null, 1, "Ciberseguridad", "mortega", 4 },
                    { -14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -9, "Jorge Silva", true, "tech03", null, null, 1, "Electricidad y Energía", "jsilva", 7 },
                    { -13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -6, "Eduardo Vargas", true, "tech02", null, null, 1, "Servidores y Virtualización", "evargas", 3 },
                    { -12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -3, "Carlos Méndez", true, "tech01", null, null, 1, "Redes y Comunicaciones", "cmendez", 5 },
                    { -11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -13, "Isabel Castro", true, "$2a$11$6l3KjoYoKF0fDqIgEDwB4u8gUJ6IKOQgMf1K6n4DoYneSFqPrMRvu", null, null, 3, null, "icastro", null },
                    { -10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -10, "Ricardo Díaz", true, "$2a$11$DuHpt0qFrD1nX4vvR6ypUu59ou5tGK8J.v8k3KhzMCQ/LzqyFOfWi", null, null, 3, null, "rdiaz", null },
                    { -9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -7, "Patricia Herrera", true, "$2a$11$ZeaAzBiPv.Xw6IYn5HsfM.TsEo8Rsh7iwC6xyVRAY6kAibSVuK.IW", null, null, 3, null, "pherrera", null },
                    { -8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -6, "Alejandro Torres", true, "$2a$11$2p2NYJ3apLI342Dtp5EpEub2nxcPFkzsb88ci3oDI25IykC.0Unfm", null, null, 3, null, "atorres", null },
                    { -7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Sofía Ramírez", true, "$2a$11$eZJoOAWDoV6iLxXJtV3sjeW134dzJnATrW0BUXedvDUpm8D3v1vC6", null, null, 3, null, "sramirez", null },
                    { -6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -24, "Elena Morales", true, "$2a$11$frxc8XpGXgGi53fiLcSRoOt3Nq7aE56VMPf.ECnNWXJkxIFPXxrMq", null, null, 5, null, "emorales", null },
                    { -5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -3, "Roberto López", true, "$2a$11$OlHPfN0V9EcZwDZ2NhvzaOT0E6F8/EfWo2wHzJhSFEVEwd7fqBkCa", null, null, 4, null, "rlopez", null },
                    { -4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -5, "Carmen Sánchez", true, "$2a$11$J0ZStMp50aiYljT3vj.yQucG.tqZOI7x43quH5EdmzB6K5z5cIVuq", null, null, 4, null, "csanchez", null },
                    { -3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -18, "Javier Rodríguez", true, "$2a$11$/jwi2T7PHM6fSAyAelnoaOOxPMLU25uXG/3NvTyTK5rMlEoSZm55y", null, null, 4, null, "jrodriguez", null },
                    { -2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -18, "Laura Martínez", true, "$2a$11$2u/fqdnoGBupkujZw8HXeu3tjULZ6e.EqZVEQwLmWGWRvPPpu8gee", null, null, 4, null, "lmartinez", null },
                    { -1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -21, "David González", true, "$2a$11$kkxPo0Sl6gHf46gy7Pe5xeZZa0X2gGboRBG4Rd9gWniOm1PGHZ7he", null, null, 4, null, "dgonzalez", null }
                });

            migrationBuilder.InsertData(
                table: "Assessments",
                columns: new[] { "PerformanceRatingId", "Date", "Description", "Score", "TechnicianId", "UserId" },
                values: new object[,]
                {
                    { -5, new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Trabajo constante en fibra", 4.0, -16, -11 },
                    { -4, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Cumplimiento en ciberseguridad", 4.2000000000000002, -15, -10 },
                    { -3, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Mejorar tiempos de respuesta", 2.5, -14, -9 },
                    { -2, new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Buen manejo de servidores", 3.8999999999999999, -13, -8 },
                    { -1, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Excelente trabajo en red troncal", 4.7999999999999998, -12, -7 }
                });

            migrationBuilder.InsertData(
                table: "DecommissioningRequests",
                columns: new[] { "DecommissioningRequestId", "Date", "DeviceId", "DeviceReceiverId", "IsApproved", "Reason", "Status", "TechnicianId", "UserId" },
                values: new object[,]
                {
                    { -3, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), -5, -17, false, 3, 2, -16, -2 },
                    { -1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), -4, -19, true, 4, 1, -14, -1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_UserId",
                table: "Assessments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_UserId",
                table: "DecommissioningRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SectionId",
                table: "Departments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DepartmentId",
                table: "Devices",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Mainteinances_UserId",
                table: "Mainteinances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionManagerId",
                table: "Sections",
                column: "SectionManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Users_UserId",
                table: "Assessments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DecommissioningRequests_Users_UserId",
                table: "DecommissioningRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Sections_SectionId",
                table: "Departments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Users_SectionManagerId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "DecommissioningRequests");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Mainteinances");

            migrationBuilder.DropTable(
                name: "ReceivingInspectionRequests");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
