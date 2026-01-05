using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class groups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "specialties",
                schema: "profile",
                columns: table => new
                {
                    SpecialtyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpecialtyTitle = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialties", x => x.SpecialtyId);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "profile",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Course = table.Column<int>(type: "integer", nullable: false),
                    NumberGroup = table.Column<int>(type: "integer", nullable: false),
                    SpecialtyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_groups_specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "profile",
                        principalTable: "specialties",
                        principalColumn: "SpecialtyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_groups_SpecialtyId",
                schema: "profile",
                table: "groups",
                column: "SpecialtyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "groups",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "specialties",
                schema: "profile");
        }
    }
}
