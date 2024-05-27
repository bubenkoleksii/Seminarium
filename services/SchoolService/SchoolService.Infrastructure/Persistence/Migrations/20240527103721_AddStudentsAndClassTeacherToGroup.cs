using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentsAndClassTeacherToGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolProfiles_Groups_GroupId",
                schema: "public",
                table: "SchoolProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassTeacherGroupId",
                schema: "public",
                table: "SchoolProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClassTeacherId",
                schema: "public",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ClassTeacherId",
                schema: "public",
                table: "Groups",
                column: "ClassTeacherId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_SchoolProfiles_ClassTeacherId",
                schema: "public",
                table: "Groups",
                column: "ClassTeacherId",
                principalSchema: "public",
                principalTable: "SchoolProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolProfiles_Groups_GroupId",
                schema: "public",
                table: "SchoolProfiles",
                column: "GroupId",
                principalSchema: "public",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_SchoolProfiles_ClassTeacherId",
                schema: "public",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolProfiles_Groups_GroupId",
                schema: "public",
                table: "SchoolProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ClassTeacherId",
                schema: "public",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ClassTeacherGroupId",
                schema: "public",
                table: "SchoolProfiles");

            migrationBuilder.DropColumn(
                name: "ClassTeacherId",
                schema: "public",
                table: "Groups");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolProfiles_Groups_GroupId",
                schema: "public",
                table: "SchoolProfiles",
                column: "GroupId",
                principalSchema: "public",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
