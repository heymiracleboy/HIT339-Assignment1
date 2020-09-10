using Microsoft.EntityFrameworkCore.Migrations;

namespace Ass1.Migrations.ItemDB
{
    public partial class sellertosalesperson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Sales");

            migrationBuilder.AddColumn<string>(
                name: "SellerPerson",
                table: "Sales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerPerson",
                table: "Sales");

            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
