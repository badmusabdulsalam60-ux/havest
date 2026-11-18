using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
