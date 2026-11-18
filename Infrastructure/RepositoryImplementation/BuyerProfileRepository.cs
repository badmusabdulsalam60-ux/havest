using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class BuyerProfileRepository(AppDbContext db) : EfGenericRepository<BuyerProfile>(db), IBuyerProfileRepository
    {
        public Task<BuyerProfile?> GetByUserIdAsync(Guid userId) =>
            Set.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
      
    }
}
