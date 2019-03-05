using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.Migrations
{
    public partial class CustomizeScrumDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ScrumUsers_ProductManagerId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumUserTeam_ScrumTeams_TeamId",
                table: "ScrumUserTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumUserTeam_ScrumUsers_UserId",
                table: "ScrumUserTeam");

            migrationBuilder.DropTable(
                name: "ProductBacklog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumUserTeam",
                table: "ScrumUserTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "ScrumUserTeam",
                newName: "ScrumUserTeams");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumUserTeam_TeamId",
                table: "ScrumUserTeams",
                newName: "IX_ScrumUserTeams_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProductManagerId",
                table: "Products",
                newName: "IX_Products_ProductManagerId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductPriority",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductStatus",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumUserTeams",
                table: "ScrumUserTeams",
                columns: new[] { "UserId", "TeamId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductBackLogItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Priority = table.Column<int>(nullable: false, defaultValue: 0),
                    Status = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBackLogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBackLogItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBackLogItems_ScrumTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "ScrumTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductTeams",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTeams", x => new { x.ProductId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_ProductTeams_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTeams_ScrumTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "ScrumTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BacklogTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BacklogTasks_ProductBackLogItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ProductBackLogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BacklogUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductBacklogId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    UpdatePersonId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BacklogUpdates_ProductBackLogItems_ProductBacklogId",
                        column: x => x.ProductBacklogId,
                        principalTable: "ProductBackLogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BacklogUpdates_ScrumUsers_UpdatePersonId",
                        column: x => x.UpdatePersonId,
                        principalTable: "ScrumUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackLogTaskSchedules",
                columns: table => new
                {
                    BackLogTaskId = table.Column<int>(nullable: false),
                    Day = table.Column<DateTime>(nullable: false),
                    Hours = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackLogTaskSchedules", x => new { x.BackLogTaskId, x.Day });
                    table.ForeignKey(
                        name: "FK_BackLogTaskSchedules_BacklogTasks_BackLogTaskId",
                        column: x => x.BackLogTaskId,
                        principalTable: "BacklogTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BacklogTasks_ItemId",
                table: "BacklogTasks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BackLogTaskSchedules_BackLogTaskId",
                table: "BackLogTaskSchedules",
                column: "BackLogTaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BacklogUpdates_ProductBacklogId",
                table: "BacklogUpdates",
                column: "ProductBacklogId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogUpdates_UpdatePersonId",
                table: "BacklogUpdates",
                column: "UpdatePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBackLogItems_ProductId",
                table: "ProductBackLogItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBackLogItems_TeamId",
                table: "ProductBackLogItems",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTeams_TeamId",
                table: "ProductTeams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ScrumUsers_ProductManagerId",
                table: "Products",
                column: "ProductManagerId",
                principalTable: "ScrumUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumUserTeams_ScrumTeams_TeamId",
                table: "ScrumUserTeams",
                column: "TeamId",
                principalTable: "ScrumTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumUserTeams_ScrumUsers_UserId",
                table: "ScrumUserTeams",
                column: "UserId",
                principalTable: "ScrumUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ScrumUsers_ProductManagerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumUserTeams_ScrumTeams_TeamId",
                table: "ScrumUserTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumUserTeams_ScrumUsers_UserId",
                table: "ScrumUserTeams");

            migrationBuilder.DropTable(
                name: "BackLogTaskSchedules");

            migrationBuilder.DropTable(
                name: "BacklogUpdates");

            migrationBuilder.DropTable(
                name: "ProductTeams");

            migrationBuilder.DropTable(
                name: "BacklogTasks");

            migrationBuilder.DropTable(
                name: "ProductBackLogItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumUserTeams",
                table: "ScrumUserTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPriority",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductStatus",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ScrumUserTeams",
                newName: "ScrumUserTeam");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumUserTeams_TeamId",
                table: "ScrumUserTeam",
                newName: "IX_ScrumUserTeam_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductManagerId",
                table: "Product",
                newName: "IX_Product_ProductManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumUserTeam",
                table: "ScrumUserTeam",
                columns: new[] { "UserId", "TeamId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductBacklog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklog_ProductId",
                table: "ProductBacklog",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklog_TeamId",
                table: "ProductBacklog",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ScrumUsers_ProductManagerId",
                table: "Product",
                column: "ProductManagerId",
                principalTable: "ScrumUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumUserTeam_ScrumTeams_TeamId",
                table: "ScrumUserTeam",
                column: "TeamId",
                principalTable: "ScrumTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumUserTeam_ScrumUsers_UserId",
                table: "ScrumUserTeam",
                column: "UserId",
                principalTable: "ScrumUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
