using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Decommissioning",
                columns: table => new
                {
                    DecommissioningId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    DecommissioningRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    DecommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reason = table.Column<int>(type: "INTEGER", maxLength: 500, nullable: false),
                    FinalDestination = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ReceiverDepartmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decommissioning", x => x.DecommissioningId);
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
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
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
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    OperationalState = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    MaintenanceRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cost = table.Column<double>(type: "REAL", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenances", x => x.MaintenanceRecordId);
                    table.ForeignKey(
                        name: "FK_Maintenances_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_Users_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PerformanceRatings",
                columns: table => new
                {
                    PerformanceRatingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TechnicianId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<double>(type: "REAL", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceRatings", x => x.PerformanceRatingId);
                    table.ForeignKey(
                        name: "FK_PerformanceRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    RejectReason = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingInspectionRequests", x => x.ReceivingInspectionRequestId);
                    table.ForeignKey(
                        name: "FK_ReceivingInspectionRequests_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingInspectionRequests_Users_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingInspectionRequests_Users_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
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
                    table.ForeignKey(
                        name: "FK_Rejections_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rejections_Users_DeviceReceiverId",
                        column: x => x.DeviceReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rejections_Users_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SectionManagerId = table.Column<int>(type: "INTEGER", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SourceSectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DestinationSectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferId);
                    table.ForeignKey(
                        name: "FK_Transfers_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Sections_DestinationSectionId",
                        column: x => x.DestinationSectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Sections_SourceSectionId",
                        column: x => x.SourceSectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_DeviceReceiverId",
                        column: x => x.DeviceReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
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
                    { 5, "Director" }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionId", "Name", "SectionManagerId" },
                values: new object[,]
                {
                    { -8, "Seguridad Informática (Ciberseguridad)", null },
                    { -7, "Infraestructura Interna (TI Interno)", null },
                    { -6, "Taller Central y Logística", null },
                    { -5, "División de Servicios en la Nube (Cloud)", null },
                    { -4, "Planificación y Despliegue de Red", null },
                    { -3, "Soporte Técnico en Campo", null },
                    { -2, "Infraestructura de Centro de Datos (Data Center)", null },
                    { -1, "Operaciones de Red Corporativa", null }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "Name", "SectionId" },
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
                    { -1, "Conmutación y Enrutamiento Avanzado", -1 },
                    { 1, "Mocking Deparment", -1 }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "AcquisitionDate", "DepartmentId", "Name", "OperationalState", "Type" },
                values: new object[,]
                {
                    { -6, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -14, "Analizador de Espectro Viavi", "Operational", "DiagnosticAndMeasurement" },
                    { -5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -12, "Antena de Radioenlace AirFiber 5XHD", "Operational", "CommunicationsAndTransmission" },
                    { -4, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -24, "Sistema UPS Eaton 20kVA", "Decommissioned", "ElectricalInfrastructureAndSupport" },
                    { -3, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -24, "Firewall de Próxima Generación PA-5200", "Operational", "ConnectivityAndNetwork" },
                    { -2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -16, "Servidor de Virtualización HP DL380", "UnderMaintenance", "ComputingAndIT" },
                    { -1, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -8, "Router de Agregación ASR 9000", "Operational", "ConnectivityAndNetwork" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Decommissioning_DeviceId",
                table: "Decommissioning",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Decommissioning_DeviceReceiverId",
                table: "Decommissioning",
                column: "DeviceReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Decommissioning_ReceiverDepartmentId",
                table: "Decommissioning",
                column: "ReceiverDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_DeviceId",
                table: "DecommissioningRequests",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_DeviceReceiverId",
                table: "DecommissioningRequests",
                column: "DeviceReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_TechnicianId_DeviceId_Date",
                table: "DecommissioningRequests",
                columns: new[] { "TechnicianId", "DeviceId", "Date" },
                unique: true);

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
                name: "IX_Maintenances_DeviceId",
                table: "Maintenances",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_TechnicianId_DeviceId_Date",
                table: "Maintenances",
                columns: new[] { "TechnicianId", "DeviceId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_UserId",
                table: "Maintenances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceRatings_UserId_TechnicianId_Date",
                table: "PerformanceRatings",
                columns: new[] { "UserId", "TechnicianId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingInspectionRequests_AdministratorId",
                table: "ReceivingInspectionRequests",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingInspectionRequests_DeviceId_AdministratorId_TechnicianId_EmissionDate",
                table: "ReceivingInspectionRequests",
                columns: new[] { "DeviceId", "AdministratorId", "TechnicianId", "EmissionDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingInspectionRequests_TechnicianId",
                table: "ReceivingInspectionRequests",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_DeviceId",
                table: "Rejections",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_DeviceReceiverId_TechnicianId_DeviceId_DecommissioningRequestDate",
                table: "Rejections",
                columns: new[] { "DeviceReceiverId", "TechnicianId", "DeviceId", "DecommissioningRequestDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_TechnicianId",
                table: "Rejections",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionManagerId",
                table: "Sections",
                column: "SectionManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DestinationSectionId",
                table: "Transfers",
                column: "DestinationSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DeviceId",
                table: "Transfers",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DeviceReceiverId",
                table: "Transfers",
                column: "DeviceReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SourceSectionId",
                table: "Transfers",
                column: "SourceSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Decommissioning_Departments_ReceiverDepartmentId",
                table: "Decommissioning",
                column: "ReceiverDepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Decommissioning_Devices_DeviceId",
                table: "Decommissioning",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Decommissioning_Users_DeviceReceiverId",
                table: "Decommissioning",
                column: "DeviceReceiverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DecommissioningRequests_Devices_DeviceId",
                table: "DecommissioningRequests",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DecommissioningRequests_Users_DeviceReceiverId",
                table: "DecommissioningRequests",
                column: "DeviceReceiverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DecommissioningRequests_Users_TechnicianId",
                table: "DecommissioningRequests",
                column: "TechnicianId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

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
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Decommissioning");

            migrationBuilder.DropTable(
                name: "DecommissioningRequests");

            migrationBuilder.DropTable(
                name: "Maintenances");

            migrationBuilder.DropTable(
                name: "PerformanceRatings");

            migrationBuilder.DropTable(
                name: "ReceivingInspectionRequests");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
