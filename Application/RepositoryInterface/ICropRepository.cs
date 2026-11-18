using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface ICropRepository : IGenericRepository<Crop>
    {
       
        Task<bool> NameExistsAsync(string name);
    }
}
