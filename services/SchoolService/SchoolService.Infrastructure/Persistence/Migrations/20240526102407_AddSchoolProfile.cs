using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddSchoolProfile : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SchoolProfiles",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Img = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                Details = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                Data = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SchoolProfiles", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SchoolProfiles",
            schema: "public");
    }
}
