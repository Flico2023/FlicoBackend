using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlicoProject.DataAccessLayer.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockDetailsID",
                table: "Carts",
                newName: "ProductID");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Carts",
                newName: "StockDetailsID");
        }
    }
}
