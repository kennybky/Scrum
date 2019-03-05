using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.Migrations
{
    public partial class AddNamesForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ScrumUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ScrumUsers",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ScrumUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ScrumUsers");
        }
    }
}
