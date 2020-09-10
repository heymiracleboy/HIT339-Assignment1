using Microsoft.EntityFrameworkCore.Migrations;

namespace Ass1.Migrations.ItemDB
{
    public partial class nameofitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "NameOfItem",
                table: "Carts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOfItem",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
