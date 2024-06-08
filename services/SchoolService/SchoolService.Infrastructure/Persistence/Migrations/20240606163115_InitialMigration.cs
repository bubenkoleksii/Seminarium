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
                    RegisterCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    GradingSystem = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OwnershipType = table.Column<int>(type: "integer", nullable: false),
                    StudentsQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    TerritorialCommunity = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AreOccupied = table.Column<bool>(type: "boolean", nullable: false),
                    SiteUrl = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Img = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    JoiningRequestId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    RegisterCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RequesterEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RequesterPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RequesterFullName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    GradingSystem = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OwnershipType = table.Column<int>(type: "integer", nullable: false),
                    StudentsQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    TerritorialCommunity = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AreOccupied = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    StudyPeriodNumber = table.Column<byte>(type: "smallint", nullable: false),
                    Img = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "SchoolProfiles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Img = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Details = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Data = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClassTeacherGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolProfiles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "public",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolProfiles_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "public",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentChild",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChildId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentChild", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentChild_SchoolProfiles_ChildId",
                        column: x => x.ChildId,
                        principalSchema: "public",
                        principalTable: "SchoolProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ParentChild_SchoolProfiles_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "SchoolProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ClassTeacherId",
                schema: "public",
                table: "Groups",
                column: "ClassTeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SchoolId",
                schema: "public",
                table: "Groups",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_JoiningRequests_SchoolId",
                schema: "public",
                table: "JoiningRequests",
                column: "SchoolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentChild_ChildId",
                schema: "public",
                table: "ParentChild",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChild_ParentId",
                schema: "public",
                table: "ParentChild",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProfiles_GroupId",
                schema: "public",
                table: "SchoolProfiles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProfiles_SchoolId",
                schema: "public",
                table: "SchoolProfiles",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_SchoolProfiles_ClassTeacherId",
                schema: "public",
                table: "Groups",
                column: "ClassTeacherId",
                principalSchema: "public",
                principalTable: "SchoolProfiles",
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

            migrationBuilder.DropTable(
                name: "JoiningRequests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ParentChild",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SchoolProfiles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "public");
        }
    }
}
