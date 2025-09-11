using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "GovernorateId",
                table: "Merchant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Merchant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Merchant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Merchant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_Branch_BranchId",
                table: "Merchant",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_City_CityId",
                table: "Merchant",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_Governorate_GovernorateId",
                table: "Merchant",
                column: "GovernorateId",
                principalTable: "Governorate",
                principalColumn: "Id");
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Merchant");

            migrationBuilder.AlterColumn<int>(
                name: "GovernorateId",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_Branch_BranchId",
                table: "Merchant",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_City_CityId",
                table: "Merchant",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_Governorate_GovernorateId",
                table: "Merchant",
                column: "GovernorateId",
                principalTable: "Governorate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
