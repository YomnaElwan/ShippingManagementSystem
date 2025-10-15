using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dropPermissionsAndModuleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "PermissionsModule");

            migrationBuilder.AlterColumn<int>(
                name: "CourierId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "CourierId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PermissionsModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsModule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionsModuleId = table.Column<int>(type: "int", nullable: false),
                    CanAdd = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanViewDetails = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionsModuleId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RolePermissions_PermissionsModule_PermissionsModuleId",
                        column: x => x.PermissionsModuleId,
                        principalTable: "PermissionsModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionsModuleId",
                table: "RolePermissions",
                column: "PermissionsModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
