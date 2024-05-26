using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddFKForSchoolProfileToSchool : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_SchoolProfiles_SchoolId",
            schema: "public",
            table: "SchoolProfiles",
            column: "SchoolId");

        migrationBuilder.AddForeignKey(
            name: "FK_SchoolProfiles_Schools_SchoolId",
            schema: "public",
            table: "SchoolProfiles",
            column: "SchoolId",
            principalSchema: "public",
            principalTable: "Schools",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_SchoolProfiles_Schools_SchoolId",
            schema: "public",
            table: "SchoolProfiles");

        migrationBuilder.DropIndex(
            name: "IX_SchoolProfiles_SchoolId",
            schema: "public",
            table: "SchoolProfiles");
    }
}
