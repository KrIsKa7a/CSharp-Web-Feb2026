#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedProjections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projections",
                columns: new[] { "Id", "AvailableTickets", "CinemaId", "IsDeleted", "MovieId", "Showtime", "TicketPrice" },
                values: new object[,]
                {
                    { new Guid("1d696303-8c43-406c-a007-c142ac1986af"), 100, new Guid("86e9d655-4bec-4685-b42f-40f93efedda2"), false, new Guid("ae50a5ab-9642-466f-b528-3cc61071bb4c"), new DateTime(2026, 3, 13, 14, 59, 53, 306, DateTimeKind.Local).AddTicks(3482), 10.00m },
                    { new Guid("4edcb701-2070-4570-8700-c52674e39427"), 120, new Guid("e2e63228-9ddf-491c-888a-f8077c53430e"), false, new Guid("68fb84b9-ef2a-402f-b4fc-595006f5c275"), new DateTime(2026, 3, 15, 15, 59, 53, 306, DateTimeKind.Local).AddTicks(3562), 15.00m },
                    { new Guid("9a4c71a6-660a-4932-95ba-ab8081f788ab"), 100, new Guid("86e9d655-4bec-4685-b42f-40f93efedda2"), false, new Guid("777634e2-3bb6-4748-8e91-7a10b70c78ac"), new DateTime(2026, 3, 13, 14, 59, 53, 306, DateTimeKind.Local).AddTicks(3547), 10.00m },
                    { new Guid("edbb7f8d-95e9-4d1a-97da-d88e2b99fff1"), 80, new Guid("ccb61fcf-9bd8-4008-88e8-dc69c9d24566"), false, new Guid("ae50a5ab-9642-466f-b528-3cc61071bb4c"), new DateTime(2026, 3, 14, 16, 59, 53, 306, DateTimeKind.Local).AddTicks(3555), 12.50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("1d696303-8c43-406c-a007-c142ac1986af"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("4edcb701-2070-4570-8700-c52674e39427"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("9a4c71a6-660a-4932-95ba-ab8081f788ab"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("edbb7f8d-95e9-4d1a-97da-d88e2b99fff1"));
        }
    }
}
