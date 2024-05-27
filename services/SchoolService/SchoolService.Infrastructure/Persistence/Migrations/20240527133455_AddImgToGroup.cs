using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImgToGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                schema: "public",
                table: "Groups",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                schema: "public",
                table: "Groups");
        }
    }
}
