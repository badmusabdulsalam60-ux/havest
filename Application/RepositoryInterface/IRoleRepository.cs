using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
    }
}
