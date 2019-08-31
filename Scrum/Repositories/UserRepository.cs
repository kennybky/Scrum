using Microsoft.AspNetCore.Identity;
using Scrum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Repositories
{
    public class UserRepository : IRepository<ScrumUser>
    {
        private readonly ScrumContext _dbContext;
        private readonly UserManager<ScrumUser> _userManager;

        public UserRepository(ScrumContext dbContext, UserManager<ScrumUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Task<ScrumUser> GetById()
        {
            throw new NotImplementedException();
        }

        public async Task<ScrumUser> GetByName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }
    }
}
