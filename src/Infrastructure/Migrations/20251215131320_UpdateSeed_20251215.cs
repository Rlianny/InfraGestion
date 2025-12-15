using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeed_20251215 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { -24, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -16, "Marcos Ramírez", true, "$2a$11$eZJoOAWDoV6iLxXJtV3sjeW134dzJnATrW0BUXedvDUpm8D3v1vC6", null, null, 3, null, "mramirez", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -34);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -33);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -32);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -31);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -30);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -24);
        }
    }
}
