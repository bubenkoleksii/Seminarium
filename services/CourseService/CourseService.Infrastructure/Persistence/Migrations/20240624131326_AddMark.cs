using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Mark",
                schema: "public",
                table: "PracticalLessonItemSubmits",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                schema: "public",
                table: "PracticalLessonItemSubmits");
        }
    }
}
