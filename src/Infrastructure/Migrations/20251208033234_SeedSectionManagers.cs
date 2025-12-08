using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSectionManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -1,
                column: "SectionManagerId",
                value: -7);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -2,
                column: "SectionManagerId",
                value: -8);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -3,
                column: "SectionManagerId",
                value: -9);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -4,
                column: "SectionManagerId",
                value: -10);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -5,
                column: "SectionManagerId",
                value: -11);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -6,
                column: "SectionManagerId",
                value: -7);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -7,
                column: "SectionManagerId",
                value: -8);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -8,
                column: "SectionManagerId",
                value: -9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -1,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -2,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -3,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -4,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -5,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -6,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -7,
                column: "SectionManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sections",
                keyColumn: "SectionId",
                keyValue: -8,
                column: "SectionManagerId",
                value: null);
        }
    }
}
