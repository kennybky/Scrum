using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.Migrations
{
    public partial class UpdateScheduleCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BackLogTaskSchedules_BackLogTaskId",
                table: "BackLogTaskSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogUpdates_UpdateTime",
                table: "BacklogUpdates",
                column: "UpdateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BacklogUpdates_UpdateTime",
                table: "BacklogUpdates");

            migrationBuilder.CreateIndex(
                name: "IX_BackLogTaskSchedules_BackLogTaskId",
                table: "BackLogTaskSchedules",
                column: "BackLogTaskId",
                unique: true);
        }
    }
}
