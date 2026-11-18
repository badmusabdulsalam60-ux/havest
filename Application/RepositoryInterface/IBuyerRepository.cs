using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface IBuyerProfileRepository : IGenericRepository<BuyerProfile>
    {
        Task<BuyerProfile?> GetByUserIdAsync(Guid userId);
        
    }
}
