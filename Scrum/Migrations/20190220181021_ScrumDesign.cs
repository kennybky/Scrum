using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.Migrations
{
    public partial class ScrumDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductManagerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ScrumUsers_ProductManagerId",
                        column: x => x.ProductManagerId,
                        principalTable: "ScrumUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScrumTeams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScrumMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrumTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScrumTeams_ScrumUsers_ScrumMasterId",
                        column: x => x.ScrumMasterId,
                        principalTable: "ScrumUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductBacklog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBacklog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBacklog_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBacklog_ScrumTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "ScrumTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScrumUserTeam",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrumUserTeam", x => new { x.UserId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_ScrumUserTeam_ScrumTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "ScrumTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScrumUserTeam_ScrumUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ScrumUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductManagerId",
                table: "Product",
                column: "ProductManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklog_ProductId",
                table: "ProductBacklog",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklog_TeamId",
                table: "ProductBacklog",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTeams_ScrumMasterId",
                table: "ScrumTeams",
                column: "ScrumMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumUserTeam_TeamId",
                table: "ScrumUserTeam",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBacklog");

            migrationBuilder.DropTable(
                name: "ScrumUserTeam");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ScrumTeams");
        }
    }
}
