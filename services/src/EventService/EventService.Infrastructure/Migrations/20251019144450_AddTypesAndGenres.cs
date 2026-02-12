using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTypesAndGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "events",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "events",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.CreateTable(
                name: "event_types",
                columns: table => new
                {
                    TypeId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Title = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_types", x => x.TypeId);
                }
            );

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    GenreId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Title = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.GenreId);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_events_GenreId",
                table: "events",
                column: "GenreId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_events_TypeId",
                table: "events",
                column: "TypeId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_events_event_types_TypeId",
                table: "events",
                column: "TypeId",
                principalTable: "event_types",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_events_genres_GenreId",
                table: "events",
                column: "GenreId",
                principalTable: "genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_events_event_types_TypeId", table: "events");

            migrationBuilder.DropForeignKey(name: "FK_events_genres_GenreId", table: "events");

            migrationBuilder.DropTable(name: "event_types");

            migrationBuilder.DropTable(name: "genres");

            migrationBuilder.DropIndex(name: "IX_events_GenreId", table: "events");

            migrationBuilder.DropIndex(name: "IX_events_TypeId", table: "events");

            migrationBuilder.DropColumn(name: "GenreId", table: "events");

            migrationBuilder.DropColumn(name: "TypeId", table: "events");
        }
    }
}
