using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataIsDisabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decommissioning_Departments_ReceiverDepartmentId",
                table: "Decommissioning");

            migrationBuilder.DropForeignKey(
                name: "FK_Decommissioning_Devices_DeviceId",
                table: "Decommissioning");

            migrationBuilder.DropForeignKey(
                name: "FK_Decommissioning_Users_DeviceReceiverId",
                table: "Decommissioning");

            migrationBuilder.DropForeignKey(
                name: "FK_DecommissioningRequests_Devices_DeviceId",
                table: "DecommissioningRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DecommissioningRequests_Users_DeviceReceiverId",
                table: "DecommissioningRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DecommissioningRequests_Users_TechnicianId",
                table: "DecommissioningRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Sections_SectionId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Devices_DeviceId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Users_TechnicianId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Users_UserId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_PerformanceRatings_Users_UserId",
                table: "PerformanceRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingInspectionRequests_Devices_DeviceId",
                table: "ReceivingInspectionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingInspectionRequests_Users_AdministratorId",
                table: "ReceivingInspectionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingInspectionRequests_Users_TechnicianId",
                table: "ReceivingInspectionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Rejections_Devices_DeviceId",
                table: "Rejections");

            migrationBuilder.DropForeignKey(
                name: "FK_Rejections_Users_DeviceReceiverId",
                table: "Rejections");

            migrationBuilder.DropForeignKey(
                name: "FK_Rejections_Users_TechnicianId",
                table: "Rejections");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_DeviceId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Sections_DestinationSectionId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Sections_SourceSectionId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_DeviceReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_DestinationSectionId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_DeviceId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_DeviceReceiverId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SourceSectionId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Rejections_DeviceId",
                table: "Rejections");

            migrationBuilder.DropIndex(
                name: "IX_Rejections_DeviceReceiverId_TechnicianId_DeviceId_DecommissioningRequestDate",
                table: "Rejections");

            migrationBuilder.DropIndex(
                name: "IX_Rejections_TechnicianId",
                table: "Rejections");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingInspectionRequests_AdministratorId",
                table: "ReceivingInspectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingInspectionRequests_DeviceId_AdministratorId_TechnicianId_EmissionDate",
                table: "ReceivingInspectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingInspectionRequests_TechnicianId",
                table: "ReceivingInspectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_DeviceId",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_DeviceReceiverId",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_TechnicianId_DeviceId_Date",
                table: "DecommissioningRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerformanceRatings",
                table: "PerformanceRatings");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceRatings_UserId_TechnicianId_Date",
                table: "PerformanceRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maintenances",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_DeviceId",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_TechnicianId_DeviceId_Date",
                table: "Maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decommissioning",
                table: "Decommissioning");

            migrationBuilder.DropIndex(
                name: "IX_Decommissioning_DeviceId",
                table: "Decommissioning");

            migrationBuilder.DropIndex(
                name: "IX_Decommissioning_DeviceReceiverId",
                table: "Decommissioning");

            migrationBuilder.DropIndex(
                name: "IX_Decommissioning_ReceiverDepartmentId",
                table: "Decommissioning");

            migrationBuilder.RenameTable(
                name: "PerformanceRatings",
                newName: "Assessments");

            migrationBuilder.RenameTable(
                name: "Maintenances",
                newName: "Mainteinances");

            migrationBuilder.RenameTable(
                name: "Decommissioning",
                newName: "Decommissionings");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_UserId",
                table: "Mainteinances",
                newName: "IX_Mainteinances_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Sections",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "OperationalState",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDisabled",
                table: "Departments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "FinalDestination",
                table: "Decommissionings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments",
                column: "PerformanceRatingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mainteinances",
                table: "Mainteinances",
                column: "MaintenanceRecordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decommissionings",
                table: "Decommissionings",
                column: "DecommissioningId");

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -6,
                columns: new[] { "IsDisabled", "OperationalState", "Type" },
                values: new object[] { false, 2, 4 });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -5,
                columns: new[] { "IsDisabled", "OperationalState", "Type" },
                values: new object[] { false, 2, 3 });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -4,
                columns: new[] { "IsDisabled", "OperationalState", "Type" },
                values: new object[] { false, 4, 2 });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -3,
                columns: new[] { "IsDisabled", "OperationalState", "Type" },
                values: new object[] { false, 2, 0 });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -2,
                columns: new[] { "IsDisabled", "OperationalState", "Type" },
                values: new object[] { false, 3, 1 });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -1,
                columns: new[] { "IsDisabled", "OperationalState", "Type" },
                values: new object[] { false, 2, 0 });

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -8,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -7,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -6,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -5,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -4,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -3,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -2,
                column: "IsDisabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -1,
                column: "IsDisabled",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_UserId",
                table: "Assessments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Users_UserId",
                table: "Assessments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Sections_SectionId",
                table: "Departments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mainteinances_Users_UserId",
                table: "Mainteinances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Users_UserId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Sections_SectionId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Mainteinances_Users_UserId",
                table: "Mainteinances");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mainteinances",
                table: "Mainteinances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decommissionings",
                table: "Decommissionings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_UserId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "Mainteinances",
                newName: "Maintenances");

            migrationBuilder.RenameTable(
                name: "Decommissionings",
                newName: "Decommissioning");

            migrationBuilder.RenameTable(
                name: "Assessments",
                newName: "PerformanceRatings");

            migrationBuilder.RenameIndex(
                name: "IX_Mainteinances_UserId",
                table: "Maintenances",
                newName: "IX_Maintenances_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Devices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "OperationalState",
                table: "Devices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDisabled",
                table: "Departments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "FinalDestination",
                table: "Decommissioning",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maintenances",
                table: "Maintenances",
                column: "MaintenanceRecordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decommissioning",
                table: "Decommissioning",
                column: "DecommissioningId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerformanceRatings",
                table: "PerformanceRatings",
                column: "PerformanceRatingId");

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -6,
                columns: new[] { "OperationalState", "Type" },
                values: new object[] { "Operational", "DiagnosticAndMeasurement" });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -5,
                columns: new[] { "OperationalState", "Type" },
                values: new object[] { "Operational", "CommunicationsAndTransmission" });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -4,
                columns: new[] { "OperationalState", "Type" },
                values: new object[] { "Decommissioned", "ElectricalInfrastructureAndSupport" });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -3,
                columns: new[] { "OperationalState", "Type" },
                values: new object[] { "Operational", "ConnectivityAndNetwork" });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -2,
                columns: new[] { "OperationalState", "Type" },
                values: new object[] { "UnderMaintenance", "ComputingAndIT" });

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -1,
                columns: new[] { "OperationalState", "Type" },
                values: new object[] { "Operational", "ConnectivityAndNetwork" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

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
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

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
                name: "IX_Maintenances_DeviceId",
                table: "Maintenances",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_TechnicianId_DeviceId_Date",
                table: "Maintenances",
                columns: new[] { "TechnicianId", "DeviceId", "Date" },
                unique: true);

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
                name: "IX_PerformanceRatings_UserId_TechnicianId_Date",
                table: "PerformanceRatings",
                columns: new[] { "UserId", "TechnicianId", "Date" },
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
                name: "FK_Departments_Sections_SectionId",
                table: "Departments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Devices_DeviceId",
                table: "Maintenances",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Users_TechnicianId",
                table: "Maintenances",
                column: "TechnicianId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Users_UserId",
                table: "Maintenances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PerformanceRatings_Users_UserId",
                table: "PerformanceRatings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingInspectionRequests_Devices_DeviceId",
                table: "ReceivingInspectionRequests",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingInspectionRequests_Users_AdministratorId",
                table: "ReceivingInspectionRequests",
                column: "AdministratorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingInspectionRequests_Users_TechnicianId",
                table: "ReceivingInspectionRequests",
                column: "TechnicianId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rejections_Devices_DeviceId",
                table: "Rejections",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rejections_Users_DeviceReceiverId",
                table: "Rejections",
                column: "DeviceReceiverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rejections_Users_TechnicianId",
                table: "Rejections",
                column: "TechnicianId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_DeviceId",
                table: "Transfers",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Sections_DestinationSectionId",
                table: "Transfers",
                column: "DestinationSectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Sections_SourceSectionId",
                table: "Transfers",
                column: "SourceSectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_DeviceReceiverId",
                table: "Transfers",
                column: "DeviceReceiverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
