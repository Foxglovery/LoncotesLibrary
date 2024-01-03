using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoncotesLibrary.Migrations
{
    public partial class InitialCreateV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Checkouts_MaterialId",
                table: "Checkouts");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_MaterialId",
                table: "Checkouts",
                column: "MaterialId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Checkouts_MaterialId",
                table: "Checkouts");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_MaterialId",
                table: "Checkouts",
                column: "MaterialId");
        }
    }
}
