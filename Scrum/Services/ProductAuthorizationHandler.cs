﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Scrum.Data;
using Scrum.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

/*
 *  Authoprization requirements
 * */
namespace Scrum.Services
{
    public class ProductAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Product>
    {
        private readonly ScrumContext _dbContext;
        private ScrumUser User;
        public ProductAuthorizationHandler(ScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Product resource)
        {
            if (context.User.IsInRole(Roles.Admin) || resource.ProductManager.UserName == context.User.Identity.Name)
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
            if (requirement.Name == Operations.View.Name)
            {
                await CanViewBacklog(context, requirement, resource);
            }
        }

       
        private async Task CanViewBacklog(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Product resource)
        {

            var _IsInProductTeam = await IsInProductTeam(resource);
            if (_IsInProductTeam)
            {
                context.Succeed(requirement);
            }

        }


        private async Task<bool> IsInProductTeam(Product resource)
        {
            var ProductTeam = await _dbContext.ProductTeams.Where(pt => pt.ProductId == resource.Id).ToListAsync();
            var UserTeam = await _dbContext.ScrumUserTeams.Where(ut => ut.UserId == User.Id).ToListAsync();
            foreach (var team in UserTeam)
            {
                var product = ProductTeam.Find(pt => pt.TeamId == team.TeamId);
                if (product != null)
                {
                    return true;
                }
            }
            return false;
        }



    }
     
}
