using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_ELearning.Migrations
{
    public partial class addorm3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submits_Testes_TestId",
                table: "Submits");

            migrationBuilder.AddForeignKey(
                name: "FK_Submits_Testes_TestId",
                table: "Submits",
                column: "TestId",
                principalTable: "Testes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submits_Testes_TestId",
                table: "Submits");

            migrationBuilder.AddForeignKey(
                name: "FK_Submits_Testes_TestId",
                table: "Submits",
                column: "TestId",
                principalTable: "Testes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
