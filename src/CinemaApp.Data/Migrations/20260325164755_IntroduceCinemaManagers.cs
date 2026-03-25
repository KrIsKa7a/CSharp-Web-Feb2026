#nullable disable

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class IntroduceCinemaManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Cinemas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manager_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("86e9d655-4bec-4685-b42f-40f93efedda2"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("ccb61fcf-9bd8-4008-88e8-dc69c9d24566"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("e2e63228-9ddf-491c-888a-f8077c53430e"),
                column: "ManagerId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Cinemas_ManagerId",
                table: "Cinemas",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserId",
                table: "Manager",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cinemas_Manager_ManagerId",
                table: "Cinemas",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cinemas_Manager_ManagerId",
                table: "Cinemas");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Cinemas_ManagerId",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Cinemas");
        }
    }
}
