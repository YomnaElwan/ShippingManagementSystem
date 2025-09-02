using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifyDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeightSettingsId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_WeightSettingsId",
                table: "Order",
                column: "WeightSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_WeightSettings_WeightSettingsId",
                table: "Order",
                column: "WeightSettingsId",
                principalTable: "WeightSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_WeightSettings_WeightSettingsId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_WeightSettingsId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "WeightSettingsId",
                table: "Order");
        }
    }
}
