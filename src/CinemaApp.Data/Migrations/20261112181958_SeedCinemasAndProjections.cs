#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedCinemasAndProjections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("5ad7fbe7-f76f-4e98-826f-b85de6169939"), false, "Varna", "Sea View Cinema" },
                    { new Guid("76fa26f7-f48f-4397-bf96-744a74128234"), false, "Plovdiv", "Movie Palace" },
                    { new Guid("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), false, "Sofia", "Cine Grand Mall" }
                });

            migrationBuilder.InsertData(
                table: "Projections",
                columns: new[] { "Id", "AvailableTickets", "CinemaId", "IsDeleted", "MovieId", "Showtime" },
                values: new object[,]
                {
                    { new Guid("2ff9ec78-3226-4f7d-a8d4-b7744bc40616"), 100, new Guid("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), false, new Guid("e673a37c-8ffc-46bf-a66d-18b8078d7c94"), new DateTime(2026, 11, 15, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("302763cb-a86b-4f4b-a66b-f44fbd949836"), 90, new Guid("76fa26f7-f48f-4397-bf96-744a74128234"), false, new Guid("e673a37c-8ffc-46bf-a66d-18b8078d7c94"), new DateTime(2026, 11, 15, 19, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4e203f0f-fafe-44f9-b26c-f4e8a1f2da76"), 125, new Guid("76fa26f7-f48f-4397-bf96-744a74128234"), false, new Guid("2f22c0dd-f7b9-46c3-a753-0d076dafb489"), new DateTime(2026, 11, 16, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4fc4cc82-f924-4875-8abc-f2d15898bc7d"), 140, new Guid("5ad7fbe7-f76f-4e98-826f-b85de6169939"), false, new Guid("4571bf2f-dbb3-446c-a92a-07cb77f47ed0"), new DateTime(2026, 11, 14, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5465ab3a-21ce-4cd5-b35b-c5967f1ce98e"), 120, new Guid("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), false, new Guid("4571bf2f-dbb3-446c-a92a-07cb77f47ed0"), new DateTime(2026, 11, 14, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5e6a89cc-f9fd-4f49-9877-e21f211c5cd4"), 95, new Guid("5ad7fbe7-f76f-4e98-826f-b85de6169939"), false, new Guid("e673a37c-8ffc-46bf-a66d-18b8078d7c94"), new DateTime(2026, 11, 15, 21, 15, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8051818b-9f0f-417f-8ee3-117d3fbbf154"), 115, new Guid("5ad7fbe7-f76f-4e98-826f-b85de6169939"), false, new Guid("2f22c0dd-f7b9-46c3-a753-0d076dafb489"), new DateTime(2026, 11, 16, 19, 45, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b97e76f3-3646-4267-8f59-76a37c8e70dc"), 130, new Guid("76fa26f7-f48f-4397-bf96-744a74128234"), false, new Guid("4571bf2f-dbb3-446c-a92a-07cb77f47ed0"), new DateTime(2026, 11, 14, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("da876235-1f08-4e0f-b177-c078bbdeb0f1"), 110, new Guid("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), false, new Guid("2f22c0dd-f7b9-46c3-a753-0d076dafb489"), new DateTime(2026, 11, 16, 18, 15, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("2ff9ec78-3226-4f7d-a8d4-b7744bc40616"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("302763cb-a86b-4f4b-a66b-f44fbd949836"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("4e203f0f-fafe-44f9-b26c-f4e8a1f2da76"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("4fc4cc82-f924-4875-8abc-f2d15898bc7d"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("5465ab3a-21ce-4cd5-b35b-c5967f1ce98e"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("5e6a89cc-f9fd-4f49-9877-e21f211c5cd4"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("8051818b-9f0f-417f-8ee3-117d3fbbf154"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("b97e76f3-3646-4267-8f59-76a37c8e70dc"));

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "Id",
                keyValue: new Guid("da876235-1f08-4e0f-b177-c078bbdeb0f1"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("5ad7fbe7-f76f-4e98-826f-b85de6169939"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("76fa26f7-f48f-4397-bf96-744a74128234"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"));
        }
    }
}
