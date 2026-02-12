using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class publisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                schema: "profile",
                columns: table => new
                {
                    DepartmentId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Title = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.DepartmentId);
                }
            );

            migrationBuilder.CreateTable(
                name: "posts",
                schema: "profile",
                columns: table => new
                {
                    PostId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Title = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_posts_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "profile",
                        principalTable: "departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "publishers",
                schema: "profile",
                columns: table => new
                {
                    PublisherId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishers", x => x.PublisherId);
                    table.ForeignKey(
                        name: "FK_publishers_posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "profile",
                        principalTable: "posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_publishers_users_PublisherId",
                        column: x => x.PublisherId,
                        principalSchema: "profile",
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_posts_DepartmentId",
                schema: "profile",
                table: "posts",
                column: "DepartmentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_publishers_PostId",
                schema: "profile",
                table: "publishers",
                column: "PostId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "publishers", schema: "profile");

            migrationBuilder.DropTable(name: "posts", schema: "profile");

            migrationBuilder.DropTable(name: "departments", schema: "profile");
        }
    }
}
