using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlicoProject.DataAccessLayer.Migrations
{
    public partial class orderproduct_warehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Warehouses",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Warehouses",
                table: "OrderProducts");
        }
    }
}
