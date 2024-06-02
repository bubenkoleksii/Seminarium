using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOnDeleteBehaviorForParentChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ChildId",
                schema: "public",
                table: "ParentChild");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ParentId",
                schema: "public",
                table: "ParentChild");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ChildId",
                schema: "public",
                table: "ParentChild",
                column: "ChildId",
                principalSchema: "public",
                principalTable: "SchoolProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ParentId",
                schema: "public",
                table: "ParentChild",
                column: "ParentId",
                principalSchema: "public",
                principalTable: "SchoolProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ChildId",
                schema: "public",
                table: "ParentChild");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ParentId",
                schema: "public",
                table: "ParentChild");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ChildId",
                schema: "public",
                table: "ParentChild",
                column: "ChildId",
                principalSchema: "public",
                principalTable: "SchoolProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentChild_SchoolProfiles_ParentId",
                schema: "public",
                table: "ParentChild",
                column: "ParentId",
                principalSchema: "public",
                principalTable: "SchoolProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
