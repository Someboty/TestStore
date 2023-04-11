using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    /// <inheritdoc />
    public partial class ProductsChangedToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Baskets_BasketsId",
                table: "BasketProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Products_ProductsId",
                table: "BasketProducts");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "BasketProducts",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "BasketsId",
                table: "BasketProducts",
                newName: "BasketId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProducts_ProductsId",
                table: "BasketProducts",
                newName: "IX_BasketProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProducts_BasketsId",
                table: "BasketProducts",
                newName: "IX_BasketProducts_BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Baskets_BasketId",
                table: "BasketProducts",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Products_ProductId",
                table: "BasketProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Baskets_BasketId",
                table: "BasketProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Products_ProductId",
                table: "BasketProducts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "BasketProducts",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "BasketId",
                table: "BasketProducts",
                newName: "BasketsId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProducts_ProductId",
                table: "BasketProducts",
                newName: "IX_BasketProducts_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProducts_BasketId",
                table: "BasketProducts",
                newName: "IX_BasketProducts_BasketsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Baskets_BasketsId",
                table: "BasketProducts",
                column: "BasketsId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Products_ProductsId",
                table: "BasketProducts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
