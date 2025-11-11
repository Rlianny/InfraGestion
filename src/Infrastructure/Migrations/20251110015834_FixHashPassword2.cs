using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixHashPassword2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -11,
                column: "PasswordHash",
                value: "$2a$11$xPwJ8Csu8Z1hb/MzH6EeI.IeqSWeFW6gnl85v6OiVMB04MhnQl7aG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -10,
                column: "PasswordHash",
                value: "$2a$11$sxGMNt6nW5c0kJdAHGMkDerJLn3/znxfyuZzH17N6sHZeS.VndITm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -9,
                column: "PasswordHash",
                value: "$2a$11$6zsXyXyGNFkbhZtyzIENYOhgfD48P.l8YBuL89ZQ7rQ1AY6AOq.3u");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -8,
                column: "PasswordHash",
                value: "$2a$11$tPCO2JWjkAR5Bqd.FK/YOOf2iOYZB5OQ4Ug4n8XEXJ.ACkwR0mChy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -7,
                column: "PasswordHash",
                value: "$2a$11$a2GP.F5/bPOrkM1Xtz26M.4k.UvfF/XLeZvqLBuPcEFh8luqYekte");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -6,
                column: "PasswordHash",
                value: "$2a$11$gpvkvTKTeinzgaNlieeqw.ZnfxXiNiRLonR2/sVcvD21qtu4AixQi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -5,
                column: "PasswordHash",
                value: "$2a$11$WvTa1nBqSMzLF1ueDLTJP.DZbo19jXihhhbXq3dUySmCJhd8/scvO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -4,
                column: "PasswordHash",
                value: "$2a$11$/L58RuuQpLT6Lt9172DQpOJAhGI2Z.c1awD7cAp8MCNuicXv4gUE2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -3,
                column: "PasswordHash",
                value: "$2a$11$lgCrPGrD2dFZGMG1G4j82OcsGMOprt9fGheruxZNinvl2upvgCfF.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -2,
                column: "PasswordHash",
                value: "$2a$11$Cs8/uunjf/212kXsJv/qs.R5P55oSoN2kgN17VjJYCD7/hIVt3nX2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -1,
                column: "PasswordHash",
                value: "$2a$11$vZGA9Lx4CzEUPCby5lFRPe9We334kkotTWj/y5XOvPPGYbwkrTkyi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -6,
                column: "PasswordHash",
                value: "dir123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -5,
                column: "PasswordHash",
                value: "admin05");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -4,
                column: "PasswordHash",
                value: "admin04");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -3,
                column: "PasswordHash",
                value: "admin03");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -2,
                column: "PasswordHash",
                value: "admin02");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -1,
                column: "PasswordHash",
                value: "admin01");
        }
    }
}
