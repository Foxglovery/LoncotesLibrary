using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoncotesLibrary.Migrations
{
    public partial class InitialCreateV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterialTypes_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId",
                principalTable: "MaterialTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterialTypes_MaterialTypeId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials");
        }
    }
}
