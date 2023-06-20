using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackProject.Migrations
{
    public partial class AddFooter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FooterId",
                table: "Touches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FooterId",
                table: "Links",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FooterId",
                table: "Informations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Touches_FooterId",
                table: "Touches",
                column: "FooterId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_FooterId",
                table: "Links",
                column: "FooterId");

            migrationBuilder.CreateIndex(
                name: "IX_Informations_FooterId",
                table: "Informations",
                column: "FooterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Informations_Footers_FooterId",
                table: "Informations",
                column: "FooterId",
                principalTable: "Footers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Footers_FooterId",
                table: "Links",
                column: "FooterId",
                principalTable: "Footers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Touches_Footers_FooterId",
                table: "Touches",
                column: "FooterId",
                principalTable: "Footers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Informations_Footers_FooterId",
                table: "Informations");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Footers_FooterId",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Touches_Footers_FooterId",
                table: "Touches");

            migrationBuilder.DropIndex(
                name: "IX_Touches_FooterId",
                table: "Touches");

            migrationBuilder.DropIndex(
                name: "IX_Links_FooterId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Informations_FooterId",
                table: "Informations");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "Touches");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "Informations");
        }
    }
}
