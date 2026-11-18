
using Domain.Shared;

namespace Application.RepositoryInterface
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<T>> GetAll();

        Task AddAsync(T entity);

        void Update(T entity);

        Task<bool> DeleteAsync(Guid id);

        IQueryable<T> Query();
    }
}
