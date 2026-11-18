using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class CropRepository(AppDbContext db) : EfGenericRepository<Crop>(db), ICropRepository
    {
        public Task<bool> NameExistsAsync(string name) => Set.AnyAsync(x => x.Name.ToLower() == name.ToLower() && !x.IsDeleted);
    }
}
