using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class ProduceListingRepository(AppDbContext db) : EfGenericRepository<ProduceListing>(db), IProduceListingRepository
    {
        public Task<ProduceListing?> GetByIdWithDetailsAsync(Guid id) =>
            Set.Include(x => x.Crop).Include(x => x.FarmerProfile).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public IQueryable<ProduceListing> SearchAvailable(string? search)
        {
            var query = Set.AsNoTracking().Include(x => x.Crop).Include(x => x.FarmerProfile)
                .Where(x => !x.IsDeleted && x.Status !=Domain.Enum.ListingStatus.Cancelled
                    && x.Status !=Domain.Enum.ListingStatus.SoldOut && x.TotalQuantity - x.ReservedQuantity > 0);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Title.Contains(search) || x.Crop.Name.Contains(search));
            return query;
        }
    }
}
