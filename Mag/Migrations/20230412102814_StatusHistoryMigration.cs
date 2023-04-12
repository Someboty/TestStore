using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    /// <inheritdoc />
    public partial class StatusHistoryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusHistory_Orders_OrderId",
                table: "StatusHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusHistory",
                table: "StatusHistory");

            migrationBuilder.RenameTable(
                name: "StatusHistory",
                newName: "StatusHistories");

            migrationBuilder.RenameIndex(
                name: "IX_StatusHistory_OrderId",
                table: "StatusHistories",
                newName: "IX_StatusHistories_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusHistories",
                table: "StatusHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusHistories_Orders_OrderId",
                table: "StatusHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusHistories_Orders_OrderId",
                table: "StatusHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusHistories",
                table: "StatusHistories");

            migrationBuilder.RenameTable(
                name: "StatusHistories",
                newName: "StatusHistory");

            migrationBuilder.RenameIndex(
                name: "IX_StatusHistories_OrderId",
                table: "StatusHistory",
                newName: "IX_StatusHistory_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusHistory",
                table: "StatusHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusHistory_Orders_OrderId",
                table: "StatusHistory",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
