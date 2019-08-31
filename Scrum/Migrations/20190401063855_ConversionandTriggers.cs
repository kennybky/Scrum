using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.Migrations
{
    public partial class ConversionandTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductStatus",
                table: "Products",
                nullable: false,
                defaultValue: "CONCEPTUALIZED",
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ProductPriority",
                table: "Products",
                nullable: false,
                defaultValue: "NONE",
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProductBackLogItems",
                nullable: false,
                defaultValue: "CREATED",
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "ProductBackLogItems",
                nullable: false,
                defaultValue: "NONE",
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "BacklogUpdates",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductStatus",
                table: "Products",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldDefaultValue: "CONCEPTUALIZED");

            migrationBuilder.AlterColumn<int>(
                name: "ProductPriority",
                table: "Products",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldDefaultValue: "NONE");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ProductBackLogItems",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldDefaultValue: "CREATED");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "ProductBackLogItems",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldDefaultValue: "NONE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "BacklogUpdates",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");
        }
    }
}
