using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editMerchantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Merchant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Merchant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
