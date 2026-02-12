using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Schems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_Users", table: "Users");

            migrationBuilder.DropPrimaryKey(name: "PK_Accounts", table: "Accounts");

            migrationBuilder.EnsureSchema(name: "identity");

            migrationBuilder.EnsureSchema(name: "profile");

            migrationBuilder.RenameTable(name: "Users", newName: "users", newSchema: "profile");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "accounts",
                newSchema: "identity"
            );

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Email",
                schema: "identity",
                table: "accounts",
                newName: "IX_accounts_Email"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "profile",
                table: "users",
                column: "UserId"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                schema: "identity",
                table: "accounts",
                column: "AccountId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_users", schema: "profile", table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                schema: "identity",
                table: "accounts"
            );

            migrationBuilder.RenameTable(name: "users", schema: "profile", newName: "Users");

            migrationBuilder.RenameTable(name: "accounts", schema: "identity", newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_Email",
                table: "Accounts",
                newName: "IX_Accounts_Email"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Users", table: "Users", column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountId"
            );
        }
    }
}
