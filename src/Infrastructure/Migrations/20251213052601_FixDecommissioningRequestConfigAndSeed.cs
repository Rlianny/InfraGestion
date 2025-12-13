using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDecommissioningRequestConfigAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DecommissioningRequests",
                newName: "EmissionDate");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "DecommissioningRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnswerDate",
                table: "DecommissioningRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalDestinationDepartmentID",
                table: "DecommissioningRequests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "DecommissioningRequests",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "logisticId",
                table: "DecommissioningRequests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -3,
                columns: new[] { "AnswerDate", "DeviceReceiverId", "FinalDestinationDepartmentID", "UserId", "description", "logisticId" },
                values: new object[] { new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Rechazado: se requiere reevaluación, no se justifica baja con la evidencia presentada.", -2 });

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -2,
                columns: new[] { "AnswerDate", "DeviceReceiverId", "FinalDestinationDepartmentID", "description", "logisticId" },
                values: new object[] { null, null, null, "", null });

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -1,
                columns: new[] { "AnswerDate", "FinalDestinationDepartmentID", "UserId", "description", "logisticId" },
                values: new object[] { new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, "Daño físico severo: equipo fuera de servicio. Se autoriza baja y traslado a almacén general.", -1 });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "AcquisitionDate", "DepartmentId", "IsDisabled", "Name", "OperationalState", "Type" },
                values: new object[,]
                {
                    { -30, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "UPS Modular 30kVA", 2, 2 },
                    { -29, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -23, false, "Servidor de Logs SIEM", 2, 1 },
                    { -28, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -22, false, "Firewall Legacy ASA 5516", 2, 0 },
                    { -27, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -9, false, "Radio Punto-a-Punto Ubiquiti", 2, 3 },
                    { -26, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "Equipo de Pruebas Fluke Networks", 2, 4 },
                    { -25, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -5, false, "Servidor Backup Veeam Proxy", 3, 1 },
                    { -24, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -1, false, "Switch Distribución Nexus 3000", 2, 0 },
                    { -23, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "UPS Line-Interactive 3kVA", 4, 2 },
                    { -22, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -3, false, "Router Legacy ISR 2900", 4, 0 },
                    { -21, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -4, false, "Servidor Legacy Xeon E5", 4, 1 },
                    { -20, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "Medidor de Potencia Óptica", 2, 4 },
                    { -19, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -11, false, "CPE GPON Huawei", 2, 3 },
                    { -18, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -4, false, "Servidor de Aplicaciones VMHost", 2, 1 },
                    { -17, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -21, false, "Switch Acceso Aruba 2930F", 2, 0 },
                    { -16, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -24, false, "Analizador de Protocolos Wireshark Probe", 2, 4 },
                    { -15, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "UPS APC Smart-UPS 10kVA", 2, 2 },
                    { -14, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -23, false, "Servidor de Monitoreo Zabbix", 4, 1 },
                    { -13, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -22, false, "Firewall FortiGate 200F", 2, 0 },
                    { -12, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), -11, false, "Radioenlace Cambium PTP", 2, 3 },
                    { -11, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -12, false, "OTDR Fibra Óptica EXFO", 2, 4 },
                    { -10, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -6, false, "Sistema de Climatización CRAC", 4, 2 },
                    { -9, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -5, false, "Storage SAN NetApp AFF", 3, 1 },
                    { -8, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -4, false, "Servidor de Base de Datos Dell R740", 2, 1 },
                    { -7, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), -1, false, "Switch Core Catalyst 9500", 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "DecommissioningRequests",
                columns: new[] { "DecommissioningRequestId", "AnswerDate", "DeviceId", "DeviceReceiverId", "EmissionDate", "FinalDestinationDepartmentID", "IsApproved", "Reason", "Status", "TechnicianId", "UserId", "description", "logisticId" },
                values: new object[,]
                {
                    { -27, new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), -30, null, new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 4, 2, -15, null, "Rechazado: el daño es superficial; se autoriza reparación y retorno a operación.", -3 },
                    { -26, null, -29, null, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 0, -14, null, "", null },
                    { -25, null, -28, null, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 0, -13, null, "", null },
                    { -24, null, -27, null, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 0, -12, null, "", null },
                    { -23, new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Utc), -26, null, new DateTime(2025, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 2, -16, null, "Rechazado: el equipo de pruebas sigue vigente; se reubica a otro departamento.", -2 },
                    { -22, null, -25, null, new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 0, -15, null, "", null },
                    { -21, null, -24, null, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 0, -14, null, "", null },
                    { -20, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), -23, -19, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 4, 1, -13, null, "Aprobado: UPS con daño irreparable en módulos de potencia.", -1 },
                    { -19, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), -22, -18, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 6, 1, -12, null, "Aprobado: reemplazo por router actualizado; equipo anterior pasa a baja.", -5 },
                    { -18, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), -21, -17, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 1, 1, -16, null, "Aprobado: servidor legacy fuera de soporte (EOL). Baja y traslado a almacén.", -4 },
                    { -17, null, -20, null, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 0, -15, null, "", null },
                    { -16, null, -19, null, new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 0, -14, null, "", null },
                    { -15, new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Utc), -18, null, new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 2, -13, null, "Rechazado: la estimación de reparación se considera aceptable frente al reemplazo.", -3 },
                    { -14, null, -17, null, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 0, -12, null, "", null },
                    { -13, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), -16, null, new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 2, -16, null, "Rechazado: el diagnóstico indica fallas corregibles; se programa reparación.", -2 },
                    { -12, null, -15, null, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 0, -15, null, "", null },
                    { -11, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), -14, -20, new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 6, 1, -14, null, "Aprobado: se reemplaza por plataforma nueva; se da de baja el servidor de monitoreo.", -1 },
                    { -10, null, -13, null, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 0, -13, null, "", null },
                    { -9, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), -12, null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 2, -12, null, "Rechazado: aún cumple requerimientos mínimos; se mantiene en inventario operativo.", -5 },
                    { -8, null, -11, null, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 0, -16, null, "", null },
                    { -7, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), -10, -21, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 4, 1, -15, null, "Aprobado: daño estructural en unidad CRAC. Baja autorizada y retiro por logística.", -4 },
                    { -6, null, -9, null, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 0, -14, null, "", null },
                    { -5, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), -8, null, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 0, 2, -13, null, "Rechazado: el equipo puede repararse; se deriva a mantenimiento correctivo.", -3 },
                    { -4, null, -7, null, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 0, -12, null, "", null }
                });

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
                name: "FK_DecommissioningRequests_Users_logisticId",
                table: "DecommissioningRequests",
                column: "logisticId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DecommissioningRequests_Departments_FinalDestinationDepartmentID",
                table: "DecommissioningRequests");

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
                name: "FK_DecommissioningRequests_Users_logisticId",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_DeviceId",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_DeviceReceiverId",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_FinalDestinationDepartmentID",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_logisticId",
                table: "DecommissioningRequests");

            migrationBuilder.DropIndex(
                name: "IX_DecommissioningRequests_TechnicianId_DeviceId_EmissionDate",
                table: "DecommissioningRequests");

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -30);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -29);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -28);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: -7);

            migrationBuilder.DropColumn(
                name: "AnswerDate",
                table: "DecommissioningRequests");

            migrationBuilder.DropColumn(
                name: "FinalDestinationDepartmentID",
                table: "DecommissioningRequests");

            migrationBuilder.DropColumn(
                name: "description",
                table: "DecommissioningRequests");

            migrationBuilder.DropColumn(
                name: "logisticId",
                table: "DecommissioningRequests");

            migrationBuilder.RenameColumn(
                name: "EmissionDate",
                table: "DecommissioningRequests",
                newName: "Date");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "DecommissioningRequests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -3,
                columns: new[] { "DeviceReceiverId", "UserId" },
                values: new object[] { -17, -2 });

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -2,
                column: "DeviceReceiverId",
                value: -18);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -1,
                column: "UserId",
                value: -1);
        }
    }
}
