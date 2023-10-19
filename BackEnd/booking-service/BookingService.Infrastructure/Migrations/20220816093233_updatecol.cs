using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    public partial class updatecol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_miss_invoice_number",
                table: "ORDER",
                newName: "miss_invoice_number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "miss_invoice_number",
                table: "ORDER",
                newName: "_miss_invoice_number");
        }
    }
}
