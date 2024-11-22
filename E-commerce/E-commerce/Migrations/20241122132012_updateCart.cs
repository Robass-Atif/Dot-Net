using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Migrations
{
    public partial class updateCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Carts_customer_id",
                table: "Carts",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_product_id",
                table: "Carts",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_customer_id",
                table: "Carts",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_product_id",
                table: "Carts",
                column: "product_id",
                principalTable: "Products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_customer_id",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_product_id",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_customer_id",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_product_id",
                table: "Carts");
        }
    }
}
