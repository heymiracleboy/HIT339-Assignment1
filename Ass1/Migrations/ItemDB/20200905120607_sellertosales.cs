using Microsoft.EntityFrameworkCore.Migrations;

namespace Ass1.Migrations.ItemDB
{
    public partial class sellertosales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Carts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
