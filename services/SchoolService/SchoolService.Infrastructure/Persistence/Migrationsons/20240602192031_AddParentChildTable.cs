using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrationsons
{
    /// <inheritdoc />
    public partial class AddParentChildTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                schema: "public",
                table: "SchoolProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParentChild",
                schema: "public",
                columns: table => new
                {
                    ChildId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentChild", x => new { x.ChildId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_ParentChild_SchoolProfiles_ChildId",
                        column: x => x.ChildId,
                        principalSchema: "public",
                        principalTable: "SchoolProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentChild_SchoolProfiles_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "SchoolProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentChild_ParentId",
                schema: "public",
                table: "ParentChild",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentChild",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "public",
                table: "SchoolProfiles");
        }
    }
}
