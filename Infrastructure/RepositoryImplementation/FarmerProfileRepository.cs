using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class FarmerProfileRepository(AppDbContext db) : EfGenericRepository<FarmerProfile>(db), IFarmerProfileRepository
    {
        public Task<FarmerProfile?> GetByUserIdAsync(Guid userId) =>
            Set.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
       
    }
}
