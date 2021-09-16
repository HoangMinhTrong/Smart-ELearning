using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_ELearning.Migrations
{
    public partial class addFieldIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_SpecificId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "SpecificId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SpecificId",
                table: "Users",
                column: "SpecificId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_SpecificId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "SpecificId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SpecificId",
                table: "Users",
                column: "SpecificId",
                unique: true,
                filter: "[SpecificId] IS NOT NULL");
        }
    }
}
