#nullable disable

namespace CinemaApp.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddTicketsAndUserTicketsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CinemaMovie_Cinema_CinemaId",
                table: "CinemaMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_CinemaMovie_Movies_MovieId",
                table: "CinemaMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CinemaMovie",
                table: "CinemaMovie");

            migrationBuilder.DropIndex(
                name: "IX_CinemaMovie_CinemaId",
                table: "CinemaMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cinema",
                table: "Cinema");

            migrationBuilder.RenameTable(
                name: "CinemaMovie",
                newName: "Projections");

            migrationBuilder.RenameTable(
                name: "Cinema",
                newName: "Cinemas");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Projections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projections",
                table: "Projections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cinemas",
                table: "Cinemas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(9,4)", nullable: false),
                    ProjectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Projections_ProjectionId",
                        column: x => x.ProjectionId,
                        principalTable: "Projections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersTickets",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTickets", x => new { x.UserId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_UsersTickets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersTickets_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projections_CinemaId_MovieId",
                table: "Projections",
                columns: new[] { "CinemaId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projections_MovieId",
                table: "Projections",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectionId",
                table: "Tickets",
                column: "ProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTickets_TicketId",
                table: "UsersTickets",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projections_Cinemas_CinemaId",
                table: "Projections",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projections_Movies_MovieId",
                table: "Projections",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projections_Cinemas_CinemaId",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FK_Projections_Movies_MovieId",
                table: "Projections");

            migrationBuilder.DropTable(
                name: "UsersTickets");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projections",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_CinemaId_MovieId",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_MovieId",
                table: "Projections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cinemas",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Projections");

            migrationBuilder.RenameTable(
                name: "Projections",
                newName: "CinemaMovie");

            migrationBuilder.RenameTable(
                name: "Cinemas",
                newName: "Cinema");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CinemaMovie",
                table: "CinemaMovie",
                columns: new[] { "MovieId", "CinemaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cinema",
                table: "Cinema",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaMovie_CinemaId",
                table: "CinemaMovie",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaMovie_Cinema_CinemaId",
                table: "CinemaMovie",
                column: "CinemaId",
                principalTable: "Cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaMovie_Movies_MovieId",
                table: "CinemaMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
