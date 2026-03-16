#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedCinemaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("86e9d655-4bec-4685-b42f-40f93efedda2"), false, "Downtown", "Grand Cinema" },
                    { new Guid("ccb61fcf-9bd8-4008-88e8-dc69c9d24566"), false, "Uptown", "Movie Palace" },
                    { new Guid("e2e63228-9ddf-491c-888a-f8077c53430e"), false, "Suburb", "CinemaX" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("86e9d655-4bec-4685-b42f-40f93efedda2"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("ccb61fcf-9bd8-4008-88e8-dc69c9d24566"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("e2e63228-9ddf-491c-888a-f8077c53430e"));
        }
    }
}
