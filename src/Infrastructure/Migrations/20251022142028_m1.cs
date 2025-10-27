using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    SectionID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_Departments_Sections_SectionID",
                        column: x => x.SectionID,
                        principalTable: "Sections",
                        principalColumn: "SectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationalState = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentID);
                    table.ForeignKey(
                        name: "FK_Equipments_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    SectionID = table.Column<Guid>(type: "TEXT", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    Specialty = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Sections_SectionID",
                        column: x => x.SectionID,
                        principalTable: "Sections",
                        principalColumn: "SectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false),
                    TechnicianID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => new { x.UserID, x.TechnicianID, x.Date });
                    table.ForeignKey(
                        name: "FK_Assessments_Users_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assessments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecommissioningRequests",
                columns: table => new
                {
                    TechnicianID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EquipmentReceiverID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecommissioningRequests", x => new { x.TechnicianID, x.EquipmentID, x.Date });
                    table.ForeignKey(
                        name: "FK_DecommissioningRequests_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecommissioningRequests_Users_EquipmentReceiverID",
                        column: x => x.EquipmentReceiverID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecommissioningRequests_Users_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decommissionings",
                columns: table => new
                {
                    DecommissioningRequestID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentReceiverID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DecommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: false),
                    FinalDestination = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decommissionings", x => x.DecommissioningRequestID);
                    table.ForeignKey(
                        name: "FK_Decommissionings_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decommissionings_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decommissionings_Users_EquipmentReceiverID",
                        column: x => x.EquipmentReceiverID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mainteinances",
                columns: table => new
                {
                    TechnicianID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Cost = table.Column<double>(type: "REAL", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mainteinances", x => new { x.TechnicianID, x.EquipmentID, x.Date });
                    table.ForeignKey(
                        name: "FK_Mainteinances_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mainteinances_Users_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingInspectionRequests",
                columns: table => new
                {
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdministratorID = table.Column<Guid>(type: "TEXT", nullable: false),
                    TechnicianID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmissionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcceptedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RejectionDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingInspectionRequests", x => new { x.EquipmentID, x.AdministratorID, x.TechnicianID });
                    table.ForeignKey(
                        name: "FK_ReceivingInspectionRequests_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivingInspectionRequests_Users_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivingInspectionRequests_Users_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rejections",
                columns: table => new
                {
                    EquipmentReceiverID = table.Column<Guid>(type: "TEXT", nullable: false),
                    TechnicianID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DecommissioningRequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RejectionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejections", x => new { x.EquipmentReceiverID, x.TechnicianID, x.EquipmentID });
                    table.ForeignKey(
                        name: "FK_Rejections_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rejections_Users_EquipmentReceiverID",
                        column: x => x.EquipmentReceiverID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rejections_Users_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateTime = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SourceSectionID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DestinySectionID = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentReceiverID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferID);
                    table.ForeignKey(
                        name: "FK_Transfers_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Sections_DestinySectionID",
                        column: x => x.DestinySectionID,
                        principalTable: "Sections",
                        principalColumn: "SectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Sections_SourceSectionID",
                        column: x => x.SourceSectionID,
                        principalTable: "Sections",
                        principalColumn: "SectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_EquipmentReceiverID",
                        column: x => x.EquipmentReceiverID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "SectionID", "Name" },
                values: new object[,]
                {
                    { new Guid("2a48f950-9a2d-42e4-b324-d510c101247a"), "Desarrollo de Software" },
                    { new Guid("3f9edad3-41fd-47df-b0af-0f7cc9c28de7"), "Infraestructura" },
                    { new Guid("53a63a6f-eecc-4a53-83e1-66d8a972cb52"), "Línea de Ensamblaje" },
                    { new Guid("64e12757-0e8d-4b2f-98be-234c37d44553"), "Control de Calidad" },
                    { new Guid("75f23a48-a4b2-4cc9-8c09-f05778d559fd"), "Mantenimiento Eléctrico" },
                    { new Guid("86a34c3a-5bb1-4dad-9c0f-77d1469820de"), "Mantenimiento Mecánico" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentID", "SectionID" },
                values: new object[,]
                {
                    { new Guid("1c0c1962-b336-42bf-a9e0-7b1098da51c4"), new Guid("75f23a48-a4b2-4cc9-8c09-f05778d559fd") },
                    { new Guid("5abcde20-13fa-416f-85b9-ad1a00ca5959"), new Guid("64e12757-0e8d-4b2f-98be-234c37d44553") },
                    { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("2a48f950-9a2d-42e4-b324-d510c101247a") },
                    { new Guid("b4c493f9-6b62-48f4-b293-28e30b3d77a8"), new Guid("53a63a6f-eecc-4a53-83e1-66d8a972cb52") }
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "EquipmentID", "AcquisitionDate", "DepartmentID", "Name", "OperationalState", "Type" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Local), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "Servidor Principal", 0, 1 },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Local), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "Computadora Desarrollo", 0, 1 },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 10, 22, 0, 0, 0, 0, DateTimeKind.Local), new Guid("b4c493f9-6b62-48f4-b293-28e30b3d77a8"), "Máquina Ensamblaje", 0, 0 },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Local), new Guid("5abcde20-13fa-416f-85b9-ad1a00ca5959"), "Sistema de Comunicación", 0, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "Administrator", "Administrador Principal", "AdminHash123" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "Director", "Director General", "DirHash123" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash", "SectionID" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "SectionManager", "Gerente Desarrollo", "GerHash123", new Guid("2a48f950-9a2d-42e4-b324-d510c101247a") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("b4c493f9-6b62-48f4-b293-28e30b3d77a8"), "SectionManager", "Gerente Producción", "GerHash456", new Guid("53a63a6f-eecc-4a53-83e1-66d8a972cb52") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash", "Specialty", "YearsOfExperience" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "Technician", "Técnico Informática", "TecHash123", "Redes", 5 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("1c0c1962-b336-42bf-a9e0-7b1098da51c4"), "Technician", "Técnico Eléctrico", "TecHash456", "Electricidad Industrial", 8 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DepartmentID", "Discriminator", "FullName", "PasswordHash" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("5abcde20-13fa-416f-85b9-ad1a00ca5959"), "EquipmentReceiver", "Receptor Equipos", "RecHash123" });

            migrationBuilder.InsertData(
                table: "Assessments",
                columns: new[] { "Date", "TechnicianID", "UserID", "Score" },
                values: new object[] { new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-2222-2222-2222-222222222222"), 4.5 });

            migrationBuilder.InsertData(
                table: "DecommissioningRequests",
                columns: new[] { "Date", "EquipmentID", "TechnicianID", "EquipmentReceiverID" },
                values: new object[] { new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Local), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777") });

            migrationBuilder.InsertData(
                table: "Decommissionings",
                columns: new[] { "DecommissioningRequestID", "DecommissioningDate", "DepartmentID", "EquipmentID", "EquipmentReceiverID", "FinalDestination", "Reason", "RequestDate" },
                values: new object[] { new Guid("abcdef01-abcd-abcd-abcd-abcdef012345"), new DateTime(2025, 10, 17, 0, 0, 0, 0, DateTimeKind.Local), new Guid("b4c493f9-6b62-48f4-b293-28e30b3d77a8"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("77777777-7777-7777-7777-777777777777"), "Donación a institución educativa", "Equipo obsoleto", new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "Mainteinances",
                columns: new[] { "Date", "EquipmentID", "TechnicianID", "Cost", "Type" },
                values: new object[,]
                {
                    { new DateOnly(2025, 11, 6), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("55555555-5555-5555-5555-555555555555"), 500.0, "Preventivo" },
                    { new DateOnly(2025, 10, 25), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("66666666-6666-6666-6666-666666666666"), 1200.0, "Correctivo" }
                });

            migrationBuilder.InsertData(
                table: "ReceivingInspectionRequests",
                columns: new[] { "AdministratorID", "EquipmentID", "TechnicianID", "AcceptedDate", "EmissionDate", "RejectionDate" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), null });

            migrationBuilder.InsertData(
                table: "Rejections",
                columns: new[] { "EquipmentID", "EquipmentReceiverID", "TechnicianID", "DecommissioningRequestDate", "RejectionDate" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("77777777-7777-7777-7777-777777777777"), new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_TechnicianID",
                table: "Assessments",
                column: "TechnicianID");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_EquipmentID",
                table: "DecommissioningRequests",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_DecommissioningRequests_EquipmentReceiverID",
                table: "DecommissioningRequests",
                column: "EquipmentReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Decommissionings_DepartmentID",
                table: "Decommissionings",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Decommissionings_EquipmentID",
                table: "Decommissionings",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Decommissionings_EquipmentReceiverID",
                table: "Decommissionings",
                column: "EquipmentReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SectionID",
                table: "Departments",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_DepartmentID",
                table: "Equipments",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Mainteinances_EquipmentID",
                table: "Mainteinances",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingInspectionRequests_AdministratorID",
                table: "ReceivingInspectionRequests",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingInspectionRequests_TechnicianID",
                table: "ReceivingInspectionRequests",
                column: "TechnicianID");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_EquipmentID",
                table: "Rejections",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_TechnicianID",
                table: "Rejections",
                column: "TechnicianID");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DestinySectionID",
                table: "Transfers",
                column: "DestinySectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_EquipmentID",
                table: "Transfers",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_EquipmentReceiverID",
                table: "Transfers",
                column: "EquipmentReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SourceSectionID",
                table: "Transfers",
                column: "SourceSectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentID",
                table: "Users",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SectionID",
                table: "Users",
                column: "SectionID");
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
                name: "Mainteinances");

            migrationBuilder.DropTable(
                name: "ReceivingInspectionRequests");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
