
using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementation
{
    public class EfGenericRepository<T>(AppDbContext db)
        : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext Db = db;

        protected readonly DbSet<T> Set = db.Set<T>();

        public async Task AddAsync(T entity)
        {
            await Set.AddAsync(entity);
        }

        public void Update(T entity)
        {
            Set.Update(entity);
        }

        public IQueryable<T> Query()
        {
            return Set.AsQueryable();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await Set.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await Set.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.MarkAsDeleted();

            return true;
        }
    }
}
