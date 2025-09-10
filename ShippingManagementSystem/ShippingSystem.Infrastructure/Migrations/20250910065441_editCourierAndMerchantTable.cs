using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editCourierAndMerchantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courier_AspNetUsers_CourierId",
                table: "Courier");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchant_AspNetUsers_MerchantId",
                table: "Merchant");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Merchant_MerchantId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchant",
                table: "Merchant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courier",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "MerchantId",
                table: "Merchant",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CourierId",
                table: "Courier",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "MerchantId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Courier",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Courier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchant",
                table: "Merchant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courier",
                table: "Courier",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Merchant_UserId",
                table: "Merchant",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courier_UserId",
                table: "Courier",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courier_AspNetUsers_UserId",
                table: "Courier",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_AspNetUsers_UserId",
                table: "Merchant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Merchant_MerchantId",
                table: "Order",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courier_AspNetUsers_UserId",
                table: "Courier");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchant_AspNetUsers_UserId",
                table: "Merchant");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Merchant_MerchantId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchant",
                table: "Merchant");

            migrationBuilder.DropIndex(
                name: "IX_Merchant_UserId",
                table: "Merchant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courier",
                table: "Courier");

            migrationBuilder.DropIndex(
                name: "IX_Courier_UserId",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Courier");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Merchant",
                newName: "MerchantId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Courier",
                newName: "CourierId");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchant",
                table: "Merchant",
                column: "MerchantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courier",
                table: "Courier",
                column: "CourierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courier_AspNetUsers_CourierId",
                table: "Courier",
                column: "CourierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_AspNetUsers_MerchantId",
                table: "Merchant",
                column: "MerchantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Merchant_MerchantId",
                table: "Order",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "MerchantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
