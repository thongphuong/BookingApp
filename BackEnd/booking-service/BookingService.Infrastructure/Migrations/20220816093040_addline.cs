using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    public partial class addline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "supplier_code",
                table: "ORDER",
                newName: "receiver_name");

            migrationBuilder.RenameColumn(
                name: "emernegy_phone",
                table: "ORDER",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "emernegy_email",
                table: "ORDER",
                newName: "link");

            migrationBuilder.RenameColumn(
                name: "delivery_regisdate",
                table: "ORDER",
                newName: "receiver_time");

            migrationBuilder.AlterColumn<int>(
                name: "invoice_number",
                table: "ORDER",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "delivery_order_quantity",
                table: "ORDER",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "_miss_invoice_number",
                table: "ORDER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "check_in",
                table: "ORDER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "check_out",
                table: "ORDER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "delivery_invoice_date",
                table: "ORDER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "delivery_regis_date",
                table: "ORDER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "delivery_status",
                table: "ORDER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergn_email",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergn_phone",
                table: "ORDER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "receipt_status",
                table: "ORDER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "supplier_reference",
                table: "ORDER",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LINE",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    linecode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    linename = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LINE", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LINE");

            migrationBuilder.DropColumn(
                name: "_miss_invoice_number",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "check_in",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "check_out",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "delivery_invoice_date",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "delivery_regis_date",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "delivery_status",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "emergn_email",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "emergn_phone",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "receipt_status",
                table: "ORDER");

            migrationBuilder.DropColumn(
                name: "supplier_reference",
                table: "ORDER");

            migrationBuilder.RenameColumn(
                name: "receiver_time",
                table: "ORDER",
                newName: "delivery_regisdate");

            migrationBuilder.RenameColumn(
                name: "receiver_name",
                table: "ORDER",
                newName: "supplier_code");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "ORDER",
                newName: "emernegy_phone");

            migrationBuilder.RenameColumn(
                name: "link",
                table: "ORDER",
                newName: "emernegy_email");

            migrationBuilder.AlterColumn<int>(
                name: "invoice_number",
                table: "ORDER",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "delivery_order_quantity",
                table: "ORDER",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
