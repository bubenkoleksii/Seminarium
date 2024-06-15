using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
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
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    StudyPeriodId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    IsCreator = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTeachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseGroupCourse",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroupCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseGroupCourse_CourseGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "public",
                        principalTable: "CourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroupCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "public",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Topic = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Homework = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "public",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTeacherCourse",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTeacherCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTeacherCourse_CourseTeachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "public",
                        principalTable: "CourseTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTeacherCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "public",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LessonItems",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Text = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonItemType = table.Column<int>(type: "integer", nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: true),
                    AllowSubmitAfterDeadline = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    IsGraded = table.Column<bool>(type: "boolean", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonItems_CourseTeachers_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "public",
                        principalTable: "CourseTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonItems_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "public",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracticalLessonItemSubmits",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticalLessonItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Attempt = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TeacherComment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticalLessonItemSubmits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticalLessonItemSubmits_LessonItems_PracticalLessonItemId",
                        column: x => x.PracticalLessonItemId,
                        principalSchema: "public",
                        principalTable: "LessonItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    LessonItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    PracticalLessonItemSubmitId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_LessonItems_LessonItemId",
                        column: x => x.LessonItemId,
                        principalSchema: "public",
                        principalTable: "LessonItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attachments_PracticalLessonItemSubmits_PracticalLessonItemS~",
                        column: x => x.PracticalLessonItemSubmitId,
                        principalSchema: "public",
                        principalTable: "PracticalLessonItemSubmits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_LessonItemId",
                schema: "public",
                table: "Attachments",
                column: "LessonItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PracticalLessonItemSubmitId",
                schema: "public",
                table: "Attachments",
                column: "PracticalLessonItemSubmitId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupCourse_CourseId",
                schema: "public",
                table: "CourseGroupCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupCourse_GroupId",
                schema: "public",
                table: "CourseGroupCourse",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeacherCourse_CourseId",
                schema: "public",
                table: "CourseTeacherCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeacherCourse_TeacherId",
                schema: "public",
                table: "CourseTeacherCourse",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonItems_AuthorId",
                schema: "public",
                table: "LessonItems",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonItems_LessonId",
                schema: "public",
                table: "LessonItems",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId",
                schema: "public",
                table: "Lessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticalLessonItemSubmits_PracticalLessonItemId",
                schema: "public",
                table: "PracticalLessonItemSubmits",
                column: "PracticalLessonItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CourseGroupCourse",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CourseTeacherCourse",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PracticalLessonItemSubmits",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CourseGroups",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LessonItems",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CourseTeachers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "public");
        }
    }
}
