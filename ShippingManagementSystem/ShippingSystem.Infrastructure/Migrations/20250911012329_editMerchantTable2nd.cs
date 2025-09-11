using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editMerchantTable2nd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SpecialPackUpCost",
                table: "Merchant",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Merchant_BranchId",
                table: "Merchant",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchant_CityId",
                table: "Merchant",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchant_GovernorateId",
                table: "Merchant",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_Branch_BranchId",
                table: "Merchant",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_City_CityId",
                table: "Merchant",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_Governorate_GovernorateId",
                table: "Merchant",
                column: "GovernorateId",
                principalTable: "Governorate",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchant_Branch_BranchId",
                table: "Merchant");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchant_City_CityId",
                table: "Merchant");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchant_Governorate_GovernorateId",
                table: "Merchant");

            migrationBuilder.DropIndex(
                name: "IX_Merchant_BranchId",
                table: "Merchant");

            migrationBuilder.DropIndex(
                name: "IX_Merchant_CityId",
                table: "Merchant");

            migrationBuilder.DropIndex(
                name: "IX_Merchant_GovernorateId",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "SpecialPackUpCost",
                table: "Merchant");
        }
    }
}
