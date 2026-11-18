using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class UserRepository(AppDbContext db) : EfGenericRepository<User>(db), IUserRepository
    {
        public Task<User?> GetByEmailAsync(string email) =>
            Set.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email.ToLower() && !x.IsDeleted);
        public Task<bool> EmailOrUsernameExistsAsync(string email, string username) =>
            Set.AnyAsync(x => (x.Email == email.ToLower() || x.UserName == username) && !x.IsDeleted);
    }
}
