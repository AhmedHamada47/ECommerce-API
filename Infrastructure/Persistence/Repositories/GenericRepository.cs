using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext context)
        : IGenericRepository<TEntity, TKey>
            where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity) => context.Set<TEntity>().Add(entity);
        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
        {
            return trackChanges
                ? await context.Set<TEntity>().ToListAsync()
                : await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TKey id) => await context.Set<TEntity>().FindAsync(id);

        public Task<TEntity?> GetAsync(ISpecification<TEntity> specification)
            => SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specification)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
            => await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specification).ToListAsync();

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
            => await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specification).CountAsync();
    }
}
