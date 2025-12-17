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
                    EmissionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AnswerDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceReceiverId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: true),
                    FinalDestinationDepartmentID = table.Column<int>(type: "INTEGER", nullable: true),
                    logisticId = table.Column<int>(type: "INTEGER", nullable: true),
                    description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
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
                table: "Mainteinances",
                columns: new[] { "MaintenanceRecordId", "Cost", "Date", "Description", "DeviceId", "TechnicianId", "Type", "UserId" },
                values: new object[,]
                {
                    { -22, 45.0, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verificación de enlace inalámbrico", -27, -16, 0, null },
                    { -21, 160.0, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Problemas en snapshot de backup", -25, -15, 1, null },
                    { -20, 110.0, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Actualización de firmware de switch", -24, -14, 0, null },
                    { -19, 300.0, new DateTime(2025, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Reemplazo de fuente y test de rendimiento", -18, -13, 1, null },
                    { -18, 90.0, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chequeo de sondeo de paquetes", -16, -12, 0, null },
                    { -17, 55.0, new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Calibración de OTDR", -11, -16, 0, null },
                    { -16, 210.0, new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Reparación de controladora SAN", -9, -15, 1, null },
                    { -15, 130.0, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Limpieza y reemplazo de módulos", -7, -14, 0, null },
                    { -14, 75.0, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste de alineación de antena", -5, -13, 0, null },
                    { -13, 200.0, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Reemplazo de disco y verificación RAID", -2, -12, 1, null },
                    { -12, 110.0, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Revisión de reglas y backup de configuración", -13, -13, 0, null },
                    { -11, 140.0, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Medición de potencia y análisis de degradación", -12, -12, 2, null },
                    { -10, 50.0, new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Recalibración y limpieza de conectores", -11, -16, 0, null },
                    { -9, 320.0, new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Reemplazo de fuente redundante y pruebas I/O", -9, -15, 1, null },
                    { -8, 180.0, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Actualización de parches del SO y verificación de discos", -8, -14, 0, null },
                    { -7, 95.0, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Inspección de ventiladores y revisión de VLANs", -7, -13, 0, null },
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
                    { -30, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), -5, -30, new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -16 },
                    { -29, null, -4, -29, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, -15 },
                    { -28, null, -3, -28, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -14 },
                    { -27, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), -2, -27, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -13 },
                    { -26, null, -1, -26, new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -12 },
                    { -25, null, -5, -25, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, -16 },
                    { -24, new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), -4, -24, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 },
                    { -23, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), -3, -23, new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -14 },
                    { -22, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), -2, -22, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -13 },
                    { -21, new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), -1, -21, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -12 },
                    { -20, null, -5, -20, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, -16 },
                    { -19, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), -4, -19, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 },
                    { -18, null, -3, -18, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -14 },
                    { -17, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), -2, -17, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -13 },
                    { -16, null, -1, -16, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, -12 },
                    { -15, null, -5, -15, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -16 },
                    { -14, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), -4, -14, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 },
                    { -13, null, -3, -13, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -14 },
                    { -12, null, -2, -12, new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, -13 },
                    { -11, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), -1, -11, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -12 },
                    { -10, new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), -5, -10, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -16 },
                    { -9, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), -4, -9, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 },
                    { -8, null, -3, -8, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, -14 },
                    { -7, null, -2, -7, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -13 },
                    { -6, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), -1, -6, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -12 },
                    { -5, null, -5, -5, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -16 },
                    { -4, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), -4, -4, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 },
                    { -3, null, -3, -3, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, -14 },
                    { -2, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), -2, -2, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -13 },
                    { -1, null, -1, -1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -12 }
                });

            migrationBuilder.InsertData(
                table: "Rejections",
                columns: new[] { "RejectionId", "DecommissioningRequestDate", "DeviceId", "DeviceReceiverId", "RejectionDate", "TechnicianId" },
                values: new object[,]
                {
                    { -2, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Utc), -22, -18, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), -12 },
                    { -1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), -4, -19, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), -14 }
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
                    { -7, new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), -6, -14, -18, false, -8, 2 },
                    { -6, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), -5, -10, -19, false, -6, 4 },
                    { -5, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), -6, -8, -21, false, -2, 3 },
                    { -4, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), -7, -7, -20, false, -1, 4 },
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
                    { -30, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "UPS Modular 30kVA", 1, 2 },
                    { -29, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -23, false, "Servidor de Logs SIEM", 1, 1 },
                    { -28, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -22, false, "Firewall Legacy ASA 5516", 0, 0 },
                    { -27, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -9, false, "Radio Punto-a-Punto Ubiquiti", 1, 3 },
                    { -26, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "Equipo de Pruebas Fluke Networks", 0, 4 },
                    { -25, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -5, false, "Servidor Backup Veeam Proxy", 1, 1 },
                    { -24, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -1, false, "Switch Distribución Nexus 3000", 1, 0 },
                    { -23, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "UPS Line-Interactive 3kVA", 4, 2 },
                    { -22, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -3, false, "Router Legacy ISR 2900", 4, 0 },
                    { -21, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -4, false, "Servidor Legacy Xeon E5", 4, 1 },
                    { -20, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "Medidor de Potencia Óptica", 1, 4 },
                    { -19, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -11, false, "CPE GPON Huawei", 1, 3 },
                    { -18, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -4, false, "Servidor de Aplicaciones VMHost", 0, 1 },
                    { -17, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -21, false, "Switch Acceso Aruba 2930F", 1, 0 },
                    { -16, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -24, false, "Analizador de Protocolos Wireshark Probe", 1, 4 },
                    { -15, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "UPS APC Smart-UPS 10kVA", 0, 2 },
                    { -14, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -23, false, "Servidor de Monitoreo Zabbix", 4, 1 },
                    { -13, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -22, false, "Firewall FortiGate 200F", 0, 0 },
                    { -12, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -11, false, "Radioenlace Cambium PTP", 1, 3 },
                    { -11, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "OTDR Fibra Óptica EXFO", 1, 4 },
                    { -10, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "Sistema de Climatización CRAC", 4, 2 },
                    { -9, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -5, false, "Storage SAN NetApp AFF", 1, 1 },
                    { -8, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -4, false, "Servidor de Base de Datos Dell R740", 5, 1 },
                    { -7, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -1, false, "Switch Core Catalyst 9500", 0, 0 },
                    { -6, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -14, false, "Analizador de Espectro Viavi", 1, 4 },
                    { -5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "Antena de Radioenlace AirFiber 5XHD", 0, 3 },
                    { -4, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -24, false, "Sistema UPS Eaton 20kVA", 4, 2 },
                    { -3, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -24, false, "Firewall de Próxima Generación PA-5200", 1, 0 },
                    { -2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -16, false, "Servidor de Virtualización HP DL380", 1, 1 },
                    { -1, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -8, false, "Router de Agregación ASR 9000", 5, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "DepartmentId", "FullName", "IsActive", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Specialty", "Username", "YearsOfExperience" },
                values: new object[,]
                {
                    { -34, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -24, "Tomás Quezada", true, "$2a$11$ZeaAzBiPv.Xw6IYn5HsfM.TsEo8Rsh7iwC6xyVRAY6kAibSVuK.IW", null, null, 6, null, "tquezada", null },
                    { -33, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -6, "Sonia Olivera", true, "$2a$11$2p2NYJ3apLI342Dtp5EpEub2nxcPFkzsb88ci3oDI25IykC.0Unfm", null, null, 6, null, "solivera", null },
                    { -32, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -21, "Rafael Domínguez", true, "$2a$11$eZJoOAWDoV6iLxXJtV3sjeW134dzJnATrW0BUXedvDUpm8D3v1vC6", null, null, 6, null, "rdominguez", null },
                    { -31, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -18, "María García", true, "$2a$11$6l3KjoYoKF0fDqIgEDwB4u8gUJ6IKOQgMf1K6n4DoYneSFqPrMRvu", null, null, 6, null, "mgarcia", null },
                    { -30, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -17, "José López", true, "$2a$11$DuHpt0qFrD1nX4vvR6ypUu59ou5tGK8J.v8k3KhzMCQ/LzqyFOfWi", null, null, 6, null, "jlopez", null },
                    { -26, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -22, "Pablo Navarro", true, "$2a$11$ZeaAzBiPv.Xw6IYn5HsfM.TsEo8Rsh7iwC6xyVRAY6kAibSVuK.IW", null, null, 3, null, "pnavarro", null },
                    { -25, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -19, "Gabriela Fernández", true, "$2a$11$2p2NYJ3apLI342Dtp5EpEub2nxcPFkzsb88ci3oDI25IykC.0Unfm", null, null, 3, null, "gfernandez", null },
                    { -24, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -16, "Marcos Ramírez", true, "$2a$11$eZJoOAWDoV6iLxXJtV3sjeW134dzJnATrW0BUXedvDUpm8D3v1vC6", null, null, 3, null, "mramirez", null },
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
                    { -10, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Buen trabajo con tareas preventivas", 4.0999999999999996, -12, -11 },
                    { -9, new DateTime(2025, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Rendimiento estable", 3.7999999999999998, -16, -10 },
                    { -8, new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Excelente gestión de riesgos", 4.7000000000000002, -15, -9 },
                    { -7, new DateTime(2025, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Necesita mejorar documentación", 3.3999999999999999, -14, -8 },
                    { -6, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Buen soporte en incidentes", 4.2999999999999998, -13, -7 },
                    { -5, new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Trabajo constante en fibra", 4.0, -16, -11 },
                    { -4, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Cumplimiento en ciberseguridad", 4.2000000000000002, -15, -10 },
                    { -3, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Mejorar tiempos de respuesta", 2.5, -14, -9 },
                    { -2, new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Buen manejo de servidores", 3.8999999999999999, -13, -8 },
                    { -1, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Excelente trabajo en red troncal", 4.7999999999999998, -12, -7 }
                });

            migrationBuilder.InsertData(
                table: "DecommissioningRequests",
                columns: new[] { "DecommissioningRequestId", "AnswerDate", "DeviceId", "DeviceReceiverId", "EmissionDate", "FinalDestinationDepartmentID", "IsApproved", "Reason", "Status", "TechnicianId", "UserId", "description", "logisticId" },
                values: new object[,]
                {
                    { -27, new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), -30, null, new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 4, 2, -15, null, "Rechazado: el daño es superficial; se autoriza reparación y retorno a operación.", -3 },
                    { -26, null, -29, null, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 0, -14, null, "", null },
                    { -25, null, -28, null, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, 0, -13, null, "", null },
                    { -24, null, -27, null, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 0, -12, null, "", null },
                    { -23, new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Utc), -26, null, new DateTime(2025, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 2, -16, null, "Rechazado: el equipo de pruebas sigue vigente; se reubica a otro departamento.", -2 },
                    { -22, null, -25, null, new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 0, -15, null, "", null },
                    { -21, null, -24, null, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 0, -14, null, "", null },
                    { -20, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), -23, -19, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), -18, true, 4, 1, -13, null, "Aprobado: UPS con daño irreparable en módulos de potencia.", -1 },
                    { -19, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), -22, -18, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Utc), -18, true, 6, 1, -12, null, "Aprobado: reemplazo por router actualizado; equipo anterior pasa a baja.", -5 },
                    { -18, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), -21, -17, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), -18, true, 1, 1, -16, null, "Aprobado: servidor legacy fuera de soporte (EOL). Baja y traslado a almacén.", -4 },
                    { -17, null, -20, null, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, 0, -15, null, "", null },
                    { -16, null, -19, null, new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 0, -14, null, "", null },
                    { -15, new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Utc), -18, null, new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 2, -13, null, "Rechazado: la estimación de reparación se considera aceptable frente al reemplazo.", -3 },
                    { -14, null, -17, null, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 0, -12, null, "", null },
                    { -13, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), -16, null, new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 2, -16, null, "Rechazado: el diagnóstico indica fallas corregibles; se programa reparación.", -2 },
                    { -12, null, -15, null, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 0, -15, null, "", null },
                    { -11, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), -14, -20, new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), -18, true, 6, 1, -14, null, "Aprobado: se reemplaza por plataforma nueva; se da de baja el servidor de monitoreo.", -1 },
                    { -10, null, -13, null, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, 0, -13, null, "", null },
                    { -9, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), -12, null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 2, -12, null, "Rechazado: aún cumple requerimientos mínimos; se mantiene en inventario operativo.", -34 },
                    { -8, null, -11, null, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 0, -16, null, "", null },
                    { -7, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), -10, -21, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), -18, true, 4, 1, -15, null, "Aprobado: daño estructural en unidad CRAC. Baja autorizada y retiro por logística.", -33 },
                    { -6, null, -9, null, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, 0, -14, null, "", null },
                    { -5, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), -8, null, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 2, -13, null, "Rechazado: el equipo puede repararse; se deriva a mantenimiento correctivo.", -32 },
                    { -4, null, -7, null, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, 0, -12, null, "", null },
                    { -3, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), -5, null, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 2, -16, null, "Rechazado: se requiere reevaluación, no se justifica baja con la evidencia presentada.", -31 },
                    { -2, null, -3, null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, 0, -15, null, "", null },
                    { -1, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), -4, -19, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), -18, true, 4, 1, -14, null, "Daño físico severo: equipo fuera de servicio. Se autoriza baja y traslado a almacén general.", -30 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_UserId",
                table: "Assessments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_DeviceId",
                table: "DecommissioningRequests",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_DeviceReceiverId",
                table: "DecommissioningRequests",
                column: "DeviceReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_FinalDestinationDepartmentID",
                table: "DecommissioningRequests",
                column: "FinalDestinationDepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_logisticId",
                table: "DecommissioningRequests",
                column: "logisticId");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_TechnicianId_DeviceId_EmissionDate",
                table: "DecommissioningRequests",
                columns: new[] { "TechnicianId", "DeviceId", "EmissionDate" },
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
                name: "FK_DecommissioningRequests_Departments_FinalDestinationDepartmentID",
                table: "DecommissioningRequests",
                column: "FinalDestinationDepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
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
                name: "FK_DecommissioningRequests_Users_logisticId",
                table: "DecommissioningRequests",
                column: "logisticId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

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
                name: "Mainteinances");

            migrationBuilder.DropTable(
                name: "ReceivingInspectionRequests");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Devices");

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
