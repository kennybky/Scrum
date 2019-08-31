using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Scrum.Services
{
    public class BacklogItemAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ProductBacklogItem>
    {
        private readonly ScrumContext _dbContext;
        private ScrumUser User;
        private ProductBacklogItem BacklogItem;
        public BacklogItemAuthorizationHandler(ScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ProductBacklogItem resource)
        {
            BacklogItem = await _dbContext.ProductBackLogItems.Include(prop=> prop.Team)
                .Include(prop => prop.Product).ThenInclude(p => p.ProductManager).Where(i => i.Id == resource.Id).FirstOrDefaultAsync();

            if (context.User.IsInRole(Roles.Admin) || resource.Product.ProductManager.UserName == context.User.Identity.Name)
            {
                context.Succeed(requirement);
                return;
            }
            User = await _dbContext.Users.Where(u => u.UserName == context.User.Identity.Name).FirstOrDefaultAsync();
            if (User == null)
            {
                context.Fail();
                return;
            }
            if (requirement.Name == Operations.Update.Name)
            {
                await CanUpdateBacklogTask(context, requirement);
            } else if (requirement.Name == Operations.View.Name)
            {
                await CanViewBacklogTask(context, requirement);
            }
        }

        private async Task CanUpdateBacklogTask(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var _IsBacklogItemTeamScrumMaster = await IsBacklogItemTeamScrumMaster();
            if (_IsBacklogItemTeamScrumMaster && context.User.IsInRole(Roles.Scrum_Master)) // Is the backlog teAm scrum master
            {
                context.Succeed(requirement);
            }
        }

        private async Task CanViewBacklogTask(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var _IsInBacklogItemTeam = await IsInBacklogItemTeam();
            if (_IsInBacklogItemTeam) // Is in the backlog teAm
            {
                context.Succeed(requirement);
            }
        }

        private async Task<bool> IsInBacklogItemTeam()
        {
            var Team = BacklogItem.Team;
            var UserTeam = await _dbContext.ScrumUserTeams.Where(ut => ut.TeamId == Team.Id).ToListAsync(); // Get users in team
            foreach (var team in UserTeam)
            {
               if (team.UserId == User.Id) // Check if user is in team
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> IsBacklogItemTeamScrumMaster()
        {
            var Team = await _dbContext.ScrumTeams.Include(t => t.ScrumMaster).Where(t => t.Id == BacklogItem.Team.Id).FirstOrDefaultAsync();
           
            if (Team.ScrumMaster.Id == User.Id)
            {
                return true;
            }
            return false;
        }
    }
}
