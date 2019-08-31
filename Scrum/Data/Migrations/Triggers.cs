using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Data.Migrations
{
    public partial class Triggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create or alter Trigger BacklogUpdateDate on dbo.BacklogUpdates
                                    for update 
                                    as
                                    Update BacklogUpdates set UpdateTime = GETUTCDATE()
                                    from inserted
                                    where inserted.Id = BacklogUpdates.Id");

            migrationBuilder.Sql(@"Create or alter Trigger ItemUpdateDate on dbo.ProductBackLogItems
                                    for update 
                                    as
                                    Update ProductBackLogItems set LastUpdated = GETUTCDATE()
                                    from inserted
                                    where inserted.Id = ProductBackLogItems.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER BacklogUpdateDate");
            migrationBuilder.Sql(@"
                DROP TRIGGER ItemUpdateDate");
        }
    }
}
