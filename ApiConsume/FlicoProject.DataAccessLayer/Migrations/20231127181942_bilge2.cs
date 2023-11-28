using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlicoProject.DataAccessLayer.Migrations
{
    public partial class bilge2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Closets_AirportID",
                table: "Closets",
                column: "AirportID");

            migrationBuilder.AddForeignKey(
                name: "FK_Closets_Airports_AirportID",
                table: "Closets",
                column: "AirportID",
                principalTable: "Airports",
                principalColumn: "AirportID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Closets_Airports_AirportID",
                table: "Closets");

            migrationBuilder.DropIndex(
                name: "IX_Closets_AirportID",
                table: "Closets");
        }
    }
}
