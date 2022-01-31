using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Entities.Migrations
{
    public partial class DataTypesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "ProductImages");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductImages");

            migrationBuilder.AlterColumn<int>(
                name: "UnitPrice",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "ProductImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
