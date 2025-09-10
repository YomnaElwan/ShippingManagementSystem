using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_AspNetUsers_EmployeeId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Branch_BranchId",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employee",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Employee",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Branch_BranchId",
                table: "Employee",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Branch_BranchId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employee",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_AspNetUsers_EmployeeId",
                table: "Employee",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Branch_BranchId",
                table: "Employee",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
