using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetById();
        Task<T> GetByName(string name);
    }
}
