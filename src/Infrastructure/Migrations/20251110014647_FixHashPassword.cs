using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixHashPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -11,
                column: "PasswordHash",
                value: "$2a$11$9yGz2Dy9M7iIxKEJ9QNRo.IzO97/nx0oK83MVJd/T.9Es0OeqBjL.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -10,
                column: "PasswordHash",
                value: "$2a$11$jPY00tcem4ugF/UDZOMooOqE7oBPNx/r80ZNPZQLBgwcOHM1OPlyG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -9,
                column: "PasswordHash",
                value: "$2a$11$pDQkWvtI00rydgA./C0rPOZwGRKMhHqUE9YYpfg2MSi7ScaDSrx3.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -8,
                column: "PasswordHash",
                value: "$2a$11$fBopIqf3w6xRJ16TmS.fa.9fNAN0zInWPubGjrwxOXk7kMqUCN21q");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -7,
                column: "PasswordHash",
                value: "$2a$11$b8APwYP5.soYiC9oJugDo.I7zisWsHK7R9WQT/yRbnXIRgp9skWDe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -11,
                column: "PasswordHash",
                value: "manager05");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -10,
                column: "PasswordHash",
                value: "manager04");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -9,
                column: "PasswordHash",
                value: "manager03");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -8,
                column: "PasswordHash",
                value: "manager02");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -7,
                column: "PasswordHash",
                value: "manager01");
        }
    }
}
