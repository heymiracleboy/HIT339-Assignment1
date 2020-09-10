using Microsoft.EntityFrameworkCore.Migrations;

namespace Ass1.Migrations.ItemDB
{
    public partial class newitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Item",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Item");
        }
    }
}
