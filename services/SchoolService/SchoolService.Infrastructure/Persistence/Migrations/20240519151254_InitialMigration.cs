using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
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
                    JoiningRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisterCode = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    GradingSystem = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostalCode = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    OwnershipType = table.Column<int>(type: "integer", nullable: false),
                    StudentsQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    TerritorialCommunity = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AreOccupied = table.Column<bool>(type: "boolean", nullable: false),
                    SiteUrl = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Img = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JoiningRequests",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    RegisterCode = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RequesterEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RequesterPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RequesterFullName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    GradingSystem = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostalCode = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    OwnershipType = table.Column<int>(type: "integer", nullable: false),
                    StudentsQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    TerritorialCommunity = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AreOccupied = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoiningRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoiningRequests_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "public",
                        principalTable: "Schools",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoiningRequests_SchoolId",
                schema: "public",
                table: "JoiningRequests",
                column: "SchoolId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoiningRequests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "public");
        }
    }
}
