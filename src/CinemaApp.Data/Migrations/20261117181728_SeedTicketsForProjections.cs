#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class SeedTicketsForProjections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Price", "ProjectionId" },
                values: new object[,]
                {
                    { new Guid("165f6f04-f4a6-4af8-b664-b66dc3298fb5"), 12.00m, new Guid("5465ab3a-21ce-4cd5-b35b-c5967f1ce98e") },
                    { new Guid("2fb26c8e-9465-4f1e-88e2-8d370ddfd750"), 11.50m, new Guid("2ff9ec78-3226-4f7d-a8d4-b7744bc40616") },
                    { new Guid("39d7396a-cf8a-47a4-b0a6-6a14c9429ffc"), 12.50m, new Guid("da876235-1f08-4e0f-b177-c078bbdeb0f1") },
                    { new Guid("4a5e1906-aa85-4258-bdf7-e923aa9f7113"), 13.00m, new Guid("b97e76f3-3646-4267-8f59-76a37c8e70dc") },
                    { new Guid("54b10d8f-d10e-4b0a-a459-6dcbe9e4e943"), 11.00m, new Guid("302763cb-a86b-4f4b-a66b-f44fbd949836") },
                    { new Guid("61f9b8ec-f906-4f21-92c3-8ec95f59e086"), 12.75m, new Guid("4e203f0f-fafe-44f9-b26c-f4e8a1f2da76") },
                    { new Guid("71e6f7db-fc70-4f42-927d-96d1b943c5bb"), 13.50m, new Guid("4fc4cc82-f924-4875-8abc-f2d15898bc7d") },
                    { new Guid("841f7d89-2172-4f27-a80d-9bafe2fd89de"), 11.25m, new Guid("5e6a89cc-f9fd-4f49-9877-e21f211c5cd4") },
                    { new Guid("9af8d2c6-ee22-4f56-af8c-945d45d52b60"), 12.25m, new Guid("8051818b-9f0f-417f-8ee3-117d3fbbf154") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("165f6f04-f4a6-4af8-b664-b66dc3298fb5"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("2fb26c8e-9465-4f1e-88e2-8d370ddfd750"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("39d7396a-cf8a-47a4-b0a6-6a14c9429ffc"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("4a5e1906-aa85-4258-bdf7-e923aa9f7113"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("54b10d8f-d10e-4b0a-a459-6dcbe9e4e943"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("61f9b8ec-f906-4f21-92c3-8ec95f59e086"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("71e6f7db-fc70-4f42-927d-96d1b943c5bb"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("841f7d89-2172-4f27-a80d-9bafe2fd89de"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("9af8d2c6-ee22-4f56-af8c-945d45d52b60"));
        }
    }
}
