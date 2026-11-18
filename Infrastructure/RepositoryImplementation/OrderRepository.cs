using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RepositoryImplementation
{
    public class OrderRepository(AppDbContext db) : EfGenericRepository<Order>(db), IOrderRepository
    {
        public Task<Order?> GetByIdWithDetailsAsync(Guid id) =>
            Set.Include(x => x.Reservation).ThenInclude(x => x.ProduceListing).Include(x => x.Reservation.BuyerProfile)
                .Include(x => x.Reservation.ProduceListing.FarmerProfile)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public IQueryable<Order> ForFarmer(Guid id) =>
            Set.AsNoTracking().Include(x => x.Reservation).ThenInclude(x => x.ProduceListing)
                .Include(x => x.Reservation.BuyerProfile)
                .Where(x => !x.IsDeleted && x.Reservation.ProduceListing.FarmerProfileId == id);
        public IQueryable<Order> ForBuyer(Guid id) =>
            Set.AsNoTracking().Include(x => x.Reservation).ThenInclude(x => x.ProduceListing)
                .Where(x => !x.IsDeleted && x.Reservation.BuyerProfileId == id);
    }
}
