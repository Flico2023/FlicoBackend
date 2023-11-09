using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlicoProject.DataAccessLayer.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_ProductID",
                table: "StockDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_WarehouseID",
                table: "StockDetails",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_Products_ProductID",
                table: "StockDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_Warehouses_WarehouseID",
                table: "StockDetails",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "WarehouseID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockDetails_Products_ProductID",
                table: "StockDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockDetails_Warehouses_WarehouseID",
                table: "StockDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockDetails_ProductID",
                table: "StockDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockDetails_WarehouseID",
                table: "StockDetails");
        }
    }
}
