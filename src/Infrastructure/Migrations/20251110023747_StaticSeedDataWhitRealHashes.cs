using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StaticSeedDataWhitRealHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -11,
                column: "PasswordHash",
                value: "$2a$11$6l3KjoYoKF0fDqIgEDwB4u8gUJ6IKOQgMf1K6n4DoYneSFqPrMRvu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -10,
                column: "PasswordHash",
                value: "$2a$11$DuHpt0qFrD1nX4vvR6ypUu59ou5tGK8J.v8k3KhzMCQ/LzqyFOfWi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -9,
                column: "PasswordHash",
                value: "$2a$11$ZeaAzBiPv.Xw6IYn5HsfM.TsEo8Rsh7iwC6xyVRAY6kAibSVuK.IW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -8,
                column: "PasswordHash",
                value: "$2a$11$2p2NYJ3apLI342Dtp5EpEub2nxcPFkzsb88ci3oDI25IykC.0Unfm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -7,
                column: "PasswordHash",
                value: "$2a$11$eZJoOAWDoV6iLxXJtV3sjeW134dzJnATrW0BUXedvDUpm8D3v1vC6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -6,
                column: "PasswordHash",
                value: "$2a$11$frxc8XpGXgGi53fiLcSRoOt3Nq7aE56VMPf.ECnNWXJkxIFPXxrMq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -5,
                column: "PasswordHash",
                value: "$2a$11$OlHPfN0V9EcZwDZ2NhvzaOT0E6F8/EfWo2wHzJhSFEVEwd7fqBkCa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -4,
                column: "PasswordHash",
                value: "$2a$11$J0ZStMp50aiYljT3vj.yQucG.tqZOI7x43quH5EdmzB6K5z5cIVuq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -3,
                column: "PasswordHash",
                value: "$2a$11$/jwi2T7PHM6fSAyAelnoaOOxPMLU25uXG/3NvTyTK5rMlEoSZm55y");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -2,
                column: "PasswordHash",
                value: "$2a$11$2u/fqdnoGBupkujZw8HXeu3tjULZ6e.EqZVEQwLmWGWRvPPpu8gee");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -1,
                column: "PasswordHash",
                value: "$2a$11$kkxPo0Sl6gHf46gy7Pe5xeZZa0X2gGboRBG4Rd9gWniOm1PGHZ7he");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -11,
                column: "PasswordHash",
                value: "$2a$11$5xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5V");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -10,
                column: "PasswordHash",
                value: "$2a$11$6xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5W");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -9,
                column: "PasswordHash",
                value: "$2a$11$7xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5X");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -8,
                column: "PasswordHash",
                value: "$2a$11$8xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Y");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -7,
                column: "PasswordHash",
                value: "$2a$11$9xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -6,
                column: "PasswordHash",
                value: "$2a$11$9yH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5P");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -5,
                column: "PasswordHash",
                value: "$2a$11$0xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Q");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -4,
                column: "PasswordHash",
                value: "$2a$11$1xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5R");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -3,
                column: "PasswordHash",
                value: "$2a$11$2xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5S");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -2,
                column: "PasswordHash",
                value: "$2a$11$3xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5T");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: -1,
                column: "PasswordHash",
                value: "$2a$11$4xH3VJKZQxW5YpJ5Z5Z5ZeF5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5U");
        }
    }
}
