using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.Migrations
{
    public partial class AddNickNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nicknames",
                table: "ScrumUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nicknames",
                table: "ScrumUsers");
        }
    }
}
