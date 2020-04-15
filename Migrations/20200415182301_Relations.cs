using Microsoft.EntityFrameworkCore.Migrations;

namespace TutorialApi.Migrations
{
    public partial class Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "TodoItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_PersonId",
                table: "TodoItems",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_People_PersonId",
                table: "TodoItems",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_People_PersonId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_PersonId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "TodoItems");
        }
    }
}
