using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
        public interface IProduceListingRepository : IGenericRepository<ProduceListing>
        {
            Task<ProduceListing?> GetByIdWithDetailsAsync(Guid id);
            IQueryable<ProduceListing> SearchAvailable(string? search);
        }
    
}
