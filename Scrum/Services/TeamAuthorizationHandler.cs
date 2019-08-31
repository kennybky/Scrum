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
    public class TeamAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ScrumTeam>
    {

        public TeamAuthorizationHandler(ScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ScrumUser User;
        private ScrumContext _dbContext;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ScrumTeam resource)
        {
            if (context.User.IsInRole(Roles.Admin))
            {
                context.Succeed(requirement);
                return;
            }
            User = await _dbContext.Users.Where(u => u.UserName == context.User.Identity.Name).FirstOrDefaultAsync();

            if (requirement.Name == Operations.View.Name)
            {
                await CanViewTeam(context, requirement, resource);
            }
        }

        private async Task CanViewTeam(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ScrumTeam resource)
        {
          
                var _IsInTeam = await IsInTeam(resource);
                if(_IsInTeam)
                {
                    context.Succeed(requirement);
                }
                
        }

        private async Task<bool> IsInTeam(ScrumTeam resource)
        {
            var UserTeam = await _dbContext.ScrumUserTeams.Where(pt => pt.TeamId == resource.Id).ToListAsync();
            foreach (var user in UserTeam)
            {
                if (user.UserId == User.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
