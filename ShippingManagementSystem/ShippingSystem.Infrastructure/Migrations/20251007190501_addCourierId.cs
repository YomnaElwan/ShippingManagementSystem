using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCourierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "Order",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CourierId",
                table: "Order",
                column: "CourierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CourierId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Order");
        }
    }
}
