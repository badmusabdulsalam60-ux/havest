using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class RoleRepository(AppDbContext db) : EfGenericRepository<Role>(db), IRoleRepository
    {
        public Task<Role?> GetByNameAsync(string name) => Set.FirstOrDefaultAsync(x => x.Name == name && !x.IsDeleted);
    }
}
