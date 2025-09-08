using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_AspNetRoles_RoleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission");

            migrationBuilder.RenameTable(
                name: "RolePermission",
                newName: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "RolePermissions",
                newName: "PermissionsModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionsModuleId");

            migrationBuilder.AddColumn<bool>(
                name: "CanAdd",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDelete",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEdit",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanView",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionsModuleId" });

            migrationBuilder.CreateTable(
                name: "PermissionsModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsModule", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AspNetRoles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_PermissionsModule_PermissionsModuleId",
                table: "RolePermissions",
                column: "PermissionsModuleId",
                principalTable: "PermissionsModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_AspNetRoles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_PermissionsModule_PermissionsModuleId",
                table: "RolePermissions");

            migrationBuilder.DropTable(
                name: "PermissionsModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanAdd",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanDelete",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanEdit",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanView",
                table: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "RolePermission");

            migrationBuilder.RenameColumn(
                name: "PermissionsModuleId",
                table: "RolePermission",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionsModuleId",
                table: "RolePermission",
                newName: "IX_RolePermission_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanAdd = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_AspNetRoles_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
