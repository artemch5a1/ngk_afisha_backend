using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class snakeCaseIdentityService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_groups_specialties_SpecialtyId",
                schema: "profile",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_departments_DepartmentId",
                schema: "profile",
                table: "posts");

            migrationBuilder.DropForeignKey(
                name: "FK_publishers_posts_PostId",
                schema: "profile",
                table: "publishers");

            migrationBuilder.DropForeignKey(
                name: "FK_publishers_users_PublisherId",
                schema: "profile",
                table: "publishers");

            migrationBuilder.DropForeignKey(
                name: "FK_students_groups_GroupId",
                schema: "profile",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_users_StudentId",
                schema: "profile",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "Surname",
                schema: "profile",
                table: "users",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Patronymic",
                schema: "profile",
                table: "users",
                newName: "patronymic");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "profile",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                schema: "profile",
                table: "users",
                newName: "birth_date");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "profile",
                table: "users",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "profile",
                table: "students",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "profile",
                table: "students",
                newName: "student_id");

            migrationBuilder.RenameIndex(
                name: "IX_students_GroupId",
                schema: "profile",
                table: "students",
                newName: "IX_students_group_id");

            migrationBuilder.RenameColumn(
                name: "SpecialtyTitle",
                schema: "profile",
                table: "specialties",
                newName: "specialty_title");

            migrationBuilder.RenameColumn(
                name: "SpecialtyId",
                schema: "profile",
                table: "specialties",
                newName: "specialty_id");

            migrationBuilder.RenameColumn(
                name: "PostId",
                schema: "profile",
                table: "publishers",
                newName: "post_id");

            migrationBuilder.RenameColumn(
                name: "PublisherId",
                schema: "profile",
                table: "publishers",
                newName: "publisher_id");

            migrationBuilder.RenameIndex(
                name: "IX_publishers_PostId",
                schema: "profile",
                table: "publishers",
                newName: "IX_publishers_post_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "profile",
                table: "posts",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                schema: "profile",
                table: "posts",
                newName: "department_id");

            migrationBuilder.RenameColumn(
                name: "PostId",
                schema: "profile",
                table: "posts",
                newName: "post_id");

            migrationBuilder.RenameIndex(
                name: "IX_posts_DepartmentId",
                schema: "profile",
                table: "posts",
                newName: "IX_posts_department_id");

            migrationBuilder.RenameColumn(
                name: "Course",
                schema: "profile",
                table: "groups",
                newName: "course");

            migrationBuilder.RenameColumn(
                name: "SpecialtyId",
                schema: "profile",
                table: "groups",
                newName: "specialty_id");

            migrationBuilder.RenameColumn(
                name: "NumberGroup",
                schema: "profile",
                table: "groups",
                newName: "number_group");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "profile",
                table: "groups",
                newName: "group_id");

            migrationBuilder.RenameIndex(
                name: "IX_groups_SpecialtyId",
                schema: "profile",
                table: "groups",
                newName: "IX_groups_specialty_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "profile",
                table: "departments",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                schema: "profile",
                table: "departments",
                newName: "department_id");

            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "identity",
                table: "accounts",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "identity",
                table: "accounts",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                schema: "identity",
                table: "accounts",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "identity",
                table: "accounts",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "identity",
                table: "accounts",
                newName: "account_id");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_Email",
                schema: "identity",
                table: "accounts",
                newName: "IX_accounts_email");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_specialties_specialty_id",
                schema: "profile",
                table: "groups",
                column: "specialty_id",
                principalSchema: "profile",
                principalTable: "specialties",
                principalColumn: "specialty_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_departments_department_id",
                schema: "profile",
                table: "posts",
                column: "department_id",
                principalSchema: "profile",
                principalTable: "departments",
                principalColumn: "department_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_publishers_posts_post_id",
                schema: "profile",
                table: "publishers",
                column: "post_id",
                principalSchema: "profile",
                principalTable: "posts",
                principalColumn: "post_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_publishers_users_publisher_id",
                schema: "profile",
                table: "publishers",
                column: "publisher_id",
                principalSchema: "profile",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_groups_group_id",
                schema: "profile",
                table: "students",
                column: "group_id",
                principalSchema: "profile",
                principalTable: "groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_users_student_id",
                schema: "profile",
                table: "students",
                column: "student_id",
                principalSchema: "profile",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_groups_specialties_specialty_id",
                schema: "profile",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_departments_department_id",
                schema: "profile",
                table: "posts");

            migrationBuilder.DropForeignKey(
                name: "FK_publishers_posts_post_id",
                schema: "profile",
                table: "publishers");

            migrationBuilder.DropForeignKey(
                name: "FK_publishers_users_publisher_id",
                schema: "profile",
                table: "publishers");

            migrationBuilder.DropForeignKey(
                name: "FK_students_groups_group_id",
                schema: "profile",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_users_student_id",
                schema: "profile",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "surname",
                schema: "profile",
                table: "users",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "patronymic",
                schema: "profile",
                table: "users",
                newName: "Patronymic");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "profile",
                table: "users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                schema: "profile",
                table: "users",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "profile",
                table: "users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "group_id",
                schema: "profile",
                table: "students",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "student_id",
                schema: "profile",
                table: "students",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_students_group_id",
                schema: "profile",
                table: "students",
                newName: "IX_students_GroupId");

            migrationBuilder.RenameColumn(
                name: "specialty_title",
                schema: "profile",
                table: "specialties",
                newName: "SpecialtyTitle");

            migrationBuilder.RenameColumn(
                name: "specialty_id",
                schema: "profile",
                table: "specialties",
                newName: "SpecialtyId");

            migrationBuilder.RenameColumn(
                name: "post_id",
                schema: "profile",
                table: "publishers",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "publisher_id",
                schema: "profile",
                table: "publishers",
                newName: "PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_publishers_post_id",
                schema: "profile",
                table: "publishers",
                newName: "IX_publishers_PostId");

            migrationBuilder.RenameColumn(
                name: "title",
                schema: "profile",
                table: "posts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "department_id",
                schema: "profile",
                table: "posts",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "post_id",
                schema: "profile",
                table: "posts",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_posts_department_id",
                schema: "profile",
                table: "posts",
                newName: "IX_posts_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "course",
                schema: "profile",
                table: "groups",
                newName: "Course");

            migrationBuilder.RenameColumn(
                name: "specialty_id",
                schema: "profile",
                table: "groups",
                newName: "SpecialtyId");

            migrationBuilder.RenameColumn(
                name: "number_group",
                schema: "profile",
                table: "groups",
                newName: "NumberGroup");

            migrationBuilder.RenameColumn(
                name: "group_id",
                schema: "profile",
                table: "groups",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_groups_specialty_id",
                schema: "profile",
                table: "groups",
                newName: "IX_groups_SpecialtyId");

            migrationBuilder.RenameColumn(
                name: "title",
                schema: "profile",
                table: "departments",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "department_id",
                schema: "profile",
                table: "departments",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "role",
                schema: "identity",
                table: "accounts",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "identity",
                table: "accounts",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                schema: "identity",
                table: "accounts",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "created_date",
                schema: "identity",
                table: "accounts",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "account_id",
                schema: "identity",
                table: "accounts",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_email",
                schema: "identity",
                table: "accounts",
                newName: "IX_accounts_Email");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_specialties_SpecialtyId",
                schema: "profile",
                table: "groups",
                column: "SpecialtyId",
                principalSchema: "profile",
                principalTable: "specialties",
                principalColumn: "SpecialtyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_departments_DepartmentId",
                schema: "profile",
                table: "posts",
                column: "DepartmentId",
                principalSchema: "profile",
                principalTable: "departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_publishers_posts_PostId",
                schema: "profile",
                table: "publishers",
                column: "PostId",
                principalSchema: "profile",
                principalTable: "posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_publishers_users_PublisherId",
                schema: "profile",
                table: "publishers",
                column: "PublisherId",
                principalSchema: "profile",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_groups_GroupId",
                schema: "profile",
                table: "students",
                column: "GroupId",
                principalSchema: "profile",
                principalTable: "groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_users_StudentId",
                schema: "profile",
                table: "students",
                column: "StudentId",
                principalSchema: "profile",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
