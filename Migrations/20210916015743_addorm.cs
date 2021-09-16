using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_ELearning.Migrations
{
    public partial class addorm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Submits_TestId",
                table: "Submits",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submits_Testes_TestId",
                table: "Submits",
                column: "TestId",
                principalTable: "Testes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submits_Testes_TestId",
                table: "Submits");

            migrationBuilder.DropIndex(
                name: "IX_Submits_TestId",
                table: "Submits");
        }
    }
}
