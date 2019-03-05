using Scrum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        public Task<Product> GetById()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
