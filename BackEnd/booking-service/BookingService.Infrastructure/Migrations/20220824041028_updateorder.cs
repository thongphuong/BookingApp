using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    public partial class updateorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "department_code",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "department_name",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "line_code",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "line_name",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "registime_from",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "registime_to",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "department_code",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "department_name",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "line_code",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "line_name",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "registime_from",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "registime_to",
                table: "ORDER");
        }
    }
}
