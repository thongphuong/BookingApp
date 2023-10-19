using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    public partial class addstoreorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "store_code",
                table: "ORDER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "store_name",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "store_code",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "store_name",
                table: "ORDER");
        }
    }
}
