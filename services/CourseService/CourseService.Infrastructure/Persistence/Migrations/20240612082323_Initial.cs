using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseService.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "CourseGroups",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseGroups", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Courses",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Courses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CourseTeachers",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseTeachers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CourseCourseGroup",
            schema: "public",
            columns: table => new
            {
                CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                GroupsId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseCourseGroup", x => new { x.CoursesId, x.GroupsId });
                table.ForeignKey(
                    name: "FK_CourseCourseGroup_CourseGroups_GroupsId",
                    column: x => x.GroupsId,
                    principalSchema: "public",
                    principalTable: "CourseGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourseCourseGroup_Courses_CoursesId",
                    column: x => x.CoursesId,
                    principalSchema: "public",
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CourseCourseTeacher",
            schema: "public",
            columns: table => new
            {
                CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                TeachersId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseCourseTeacher", x => new { x.CoursesId, x.TeachersId });
                table.ForeignKey(
                    name: "FK_CourseCourseTeacher_CourseTeachers_TeachersId",
                    column: x => x.TeachersId,
                    principalSchema: "public",
                    principalTable: "CourseTeachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourseCourseTeacher_Courses_CoursesId",
                    column: x => x.CoursesId,
                    principalSchema: "public",
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CourseCourseGroup_GroupsId",
            schema: "public",
            table: "CourseCourseGroup",
            column: "GroupsId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseCourseTeacher_TeachersId",
            schema: "public",
            table: "CourseCourseTeacher",
            column: "TeachersId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CourseCourseGroup",
            schema: "public");

        migrationBuilder.DropTable(
            name: "CourseCourseTeacher",
            schema: "public");

        migrationBuilder.DropTable(
            name: "CourseGroups",
            schema: "public");

        migrationBuilder.DropTable(
            name: "CourseTeachers",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Courses",
            schema: "public");
    }
}
