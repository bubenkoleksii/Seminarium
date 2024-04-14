using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueForJoiningStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schools_JoiningRequest_JoiningRequestId",
                schema: "public",
                table: "Schools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JoiningRequest",
                schema: "public",
                table: "JoiningRequest");

            migrationBuilder.RenameTable(
                name: "JoiningRequest",
                schema: "public",
                newName: "JoiningRequests",
                newSchema: "public");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "public",
                table: "JoiningRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JoiningRequests",
                schema: "public",
                table: "JoiningRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_JoiningRequests_JoiningRequestId",
                schema: "public",
                table: "Schools",
                column: "JoiningRequestId",
                principalSchema: "public",
                principalTable: "JoiningRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schools_JoiningRequests_JoiningRequestId",
                schema: "public",
                table: "Schools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JoiningRequests",
                schema: "public",
                table: "JoiningRequests");

            migrationBuilder.RenameTable(
                name: "JoiningRequests",
                schema: "public",
                newName: "JoiningRequest",
                newSchema: "public");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "public",
                table: "JoiningRequest",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JoiningRequest",
                schema: "public",
                table: "JoiningRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_JoiningRequest_JoiningRequestId",
                schema: "public",
                table: "Schools",
                column: "JoiningRequestId",
                principalSchema: "public",
                principalTable: "JoiningRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
