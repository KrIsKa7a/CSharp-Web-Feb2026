using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetupFullTextSearchOnMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON Movies(Title) KEY INDEX PK_Movies;",
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "DROP FULLTEXT INDEX ON Movies;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog;",
                suppressTransaction: true);
        }
    }
}
