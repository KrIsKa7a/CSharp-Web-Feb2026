#nullable disable

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class FixProjectionSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("1d696303-8c43-406c-a007-c142ac1986af"),
                column: "Showtime",
                value: new DateTime(2026, 3, 3, 16, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("4edcb701-2070-4570-8700-c52674e39427"),
                column: "Showtime",
                value: new DateTime(2026, 3, 9, 11, 20, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("9a4c71a6-660a-4932-95ba-ab8081f788ab"),
                column: "Showtime",
                value: new DateTime(2026, 3, 5, 13, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("edbb7f8d-95e9-4d1a-97da-d88e2b99fff1"),
                column: "Showtime",
                value: new DateTime(2026, 3, 8, 19, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("1d696303-8c43-406c-a007-c142ac1986af"),
                column: "Showtime",
                value: new DateTime(2026, 3, 13, 15, 58, 1, 703, DateTimeKind.Local).AddTicks(6854));

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("4edcb701-2070-4570-8700-c52674e39427"),
                column: "Showtime",
                value: new DateTime(2026, 3, 15, 16, 58, 1, 703, DateTimeKind.Local).AddTicks(6925));

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("9a4c71a6-660a-4932-95ba-ab8081f788ab"),
                column: "Showtime",
                value: new DateTime(2026, 3, 13, 15, 58, 1, 703, DateTimeKind.Local).AddTicks(6910));

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("edbb7f8d-95e9-4d1a-97da-d88e2b99fff1"),
                column: "Showtime",
                value: new DateTime(2026, 3, 14, 17, 58, 1, 703, DateTimeKind.Local).AddTicks(6919));
        }
    }
}
