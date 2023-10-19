using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    public partial class Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORDER",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    store_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    supplier_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    supplier_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    line_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    department_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delivery_status = table.Column<int>(type: "int", nullable: true),
                    receipt_status = table.Column<int>(type: "int", nullable: true),
                    delivery_order_quantity = table.Column<int>(type: "int", nullable: false),
                    vehicle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    license_plate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    driver_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cmnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emergn_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emergn_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    delivery_item_quantity = table.Column<int>(type: "int", nullable: true),
                    delivery_regis_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    registime_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_number = table.Column<int>(type: "int", nullable: false),
                    miss_invoice_number = table.Column<int>(type: "int", nullable: true),
                    delivery_invoice_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    receiver_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    receiver_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    check_in = table.Column<DateTime>(type: "datetime2", nullable: true),
                    check_out = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    qr_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_by = table.Column<int>(type: "int", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    reference_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_DELIVERY",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_by = table.Column<int>(type: "int", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    reference_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_DELIVERY", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_RECEIPT",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_by = table.Column<int>(type: "int", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    reference_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_RECEIPT", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDER");

            migrationBuilder.DropTable(
                name: "ORDER_DELIVERY");

            migrationBuilder.DropTable(
                name: "ORDER_RECEIPT");
        }
    }
}
