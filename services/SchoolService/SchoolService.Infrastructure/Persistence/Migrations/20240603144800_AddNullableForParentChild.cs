using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableForParentChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentChild",
                schema: "public",
                table: "ParentChild");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                schema: "public",
                table: "ParentChild",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildId",
                schema: "public",
                table: "ParentChild",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChild_ChildId",
                schema: "public",
                table: "ParentChild",
                column: "ChildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParentChild_ChildId",
                schema: "public",
                table: "ParentChild");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                schema: "public",
                table: "ParentChild",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildId",
                schema: "public",
                table: "ParentChild",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentChild",
                schema: "public",
                table: "ParentChild",
                columns: new[] { "ChildId", "ParentId" });
        }
    }
}
