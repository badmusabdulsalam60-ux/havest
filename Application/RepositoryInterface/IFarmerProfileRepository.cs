using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface IFarmerProfileRepository : IGenericRepository<FarmerProfile>
    {
        Task<FarmerProfile?> GetByUserIdAsync(Guid userId);
       
    }
}
