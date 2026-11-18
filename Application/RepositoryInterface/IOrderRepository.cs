using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetByIdWithDetailsAsync(Guid id);
        IQueryable<Order> ForFarmer(Guid farmerProfileId);
        IQueryable<Order> ForBuyer(Guid buyerProfileId);
    }
}
