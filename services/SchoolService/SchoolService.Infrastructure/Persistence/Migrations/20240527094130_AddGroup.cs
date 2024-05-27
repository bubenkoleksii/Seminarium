using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                schema: "public",
                table: "SchoolProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    StudyPeriodNumber = table.Column<byte>(type: "smallint", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "public",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProfiles_GroupId",
                schema: "public",
                table: "SchoolProfiles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SchoolId",
                schema: "public",
                table: "Groups",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolProfiles_Groups_GroupId",
                schema: "public",
                table: "SchoolProfiles",
                column: "GroupId",
                principalSchema: "public",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolProfiles_Groups_GroupId",
                schema: "public",
                table: "SchoolProfiles");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_SchoolProfiles_GroupId",
                schema: "public",
                table: "SchoolProfiles");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "public",
                table: "SchoolProfiles");
        }
    }
}
