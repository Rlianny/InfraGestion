using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandSeedDataAfterIsApprovedNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Assessments",
                columns: new[] { "PerformanceRatingId", "Date", "Description", "Score", "TechnicianId", "UserId" },
                values: new object[,]
                {
                    { -10, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Buen trabajo con tareas preventivas", 4.0999999999999996, -12, -11 },
                    { -9, new DateTime(2025, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Rendimiento estable", 3.7999999999999998, -16, -10 },
                    { -8, new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Excelente gestión de riesgos", 4.7000000000000002, -15, -9 },
                    { -7, new DateTime(2025, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Necesita mejorar documentación", 3.3999999999999999, -14, -8 },
                    { -6, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Buen soporte en incidentes", 4.2999999999999998, -13, -7 }
                });

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -26,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -25,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -24,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -22,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -21,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -20,
                column: "FinalDestinationDepartmentID",
                value: -18);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -19,
                column: "FinalDestinationDepartmentID",
                value: -18);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -18,
                column: "FinalDestinationDepartmentID",
                value: -18);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -17,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -16,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -14,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -12,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -11,
                column: "FinalDestinationDepartmentID",
                value: -18);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -10,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -8,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -7,
                column: "FinalDestinationDepartmentID",
                value: -18);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -6,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -4,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -2,
                column: "IsApproved",
                value: null);

            migrationBuilder.UpdateData(
                table: "DecommissioningRequests",
                keyColumn: "DecommissioningRequestId",
                keyValue: -1,
                column: "FinalDestinationDepartmentID",
                value: -18);

            migrationBuilder.InsertData(
                table: "Mainteinances",
                columns: new[] { "MaintenanceRecordId", "Cost", "Date", "Description", "DeviceId", "TechnicianId", "Type", "UserId" },
                values: new object[,]
                {
                    { -12, 110.0, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Revisión de reglas y backup de configuración", -13, -13, 0, null },
                    { -11, 140.0, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Medición de potencia y análisis de degradación", -12, -12, 2, null },
                    { -10, 50.0, new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Recalibración y limpieza de conectores", -11, -16, 0, null },
                    { -9, 320.0, new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Reemplazo de fuente redundante y pruebas I/O", -9, -15, 1, null },
                    { -8, 180.0, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Actualización de parches del SO y verificación de discos", -8, -14, 0, null },
                    { -7, 95.0, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Inspección de ventiladores y revisión de VLANs", -7, -13, 0, null }
                });

            migrationBuilder.UpdateData(
                table: "ReceivingInspectionRequests",
                keyColumn: "ReceivingInspectionRequestId",
                keyValue: -3,
                columns: new[] { "EmissionDate", "RejectionDate" },
                values: new object[] { new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "ReceivingInspectionRequests",
                keyColumn: "ReceivingInspectionRequestId",
                keyValue: -2,
                columns: new[] { "AcceptedDate", "EmissionDate" },
                values: new object[] { new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "ReceivingInspectionRequests",
                keyColumn: "ReceivingInspectionRequestId",
                keyValue: -1,
                column: "RejectReason",
                value: 6);

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
                    { -23, null, -3, -23, new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -14 },
                    { -22, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), -2, -22, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -13 },
                    { -21, null, -1, -21, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -12 },
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
                    { -10, null, -5, -10, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -16 },
                    { -9, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), -4, -9, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 },
                    { -8, null, -3, -8, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, -14 },
                    { -7, null, -2, -7, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -13 },
                    { -6, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), -1, -6, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -12 },
                    { -5, null, -5, -5, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 2, -16 },
                    { -4, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), -4, -4, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, 0, -15 }
                });

            migrationBuilder.UpdateData(
                table: "Rejections",
                keyColumn: "RejectionId",
                keyValue: -2,
                columns: new[] { "DecommissioningRequestDate", "DeviceId", "DeviceReceiverId", "RejectionDate", "TechnicianId" },
                values: new object[] { new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Utc), -22, -18, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), -12 });

            migrationBuilder.UpdateData(
                table: "Rejections",
                keyColumn: "RejectionId",
                keyValue: -1,
                columns: new[] { "DecommissioningRequestDate", "DeviceId", "RejectionDate", "TechnicianId" },
                values: new object[] { new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), -4, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), -14 });

            migrationBuilder.InsertData(
                table: "Transfers",
                columns: new[] { "TransferId", "Date", "DestinationSectionId", "DeviceId", "DeviceReceiverId", "IsDisabled", "SourceSectionId", "Status" },
                values: new object[,]
                {
                    { -7, new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), -6, -14, -18, false, -8, 2 },
                    { -6, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), -5, -10, -19, false, -6, 4 },
                    { -5, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), -6, -8, -21, false, -2, 3 },
                    { -4, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), -7, -7, -20, false, -1, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
