using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    public partial class retuntdto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Line_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Store_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reference_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnDTO");
        }
    }
}
