using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Schools",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisterCode = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    GradingSystem = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostalCode = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    OwnershipType = table.Column<int>(type: "integer", nullable: false),
                    StudentsQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    TerritorialCommunity = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    AreOccupied = table.Column<bool>(type: "boolean", nullable: false),
                    SiteUrl = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Img = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schools_Email",
                schema: "public",
                table: "Schools",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_Img",
                schema: "public",
                table: "Schools",
                column: "Img",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_RegisterCode",
                schema: "public",
                table: "Schools",
                column: "RegisterCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schools",
                schema: "public");
        }
    }
}
