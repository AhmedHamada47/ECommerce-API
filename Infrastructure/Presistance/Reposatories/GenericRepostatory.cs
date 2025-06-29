using Presistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Reposatories
{
    public class GenericRepostatory<TEntity, Tkey>(StoreDbContetx context)
        : IGenricReposatory<TEntity, Tkey>
            where TEntity : BaseEntity<Tkey>
    {
        public void Add(TEntity entity) => context.Set<TEntity>().Add(entity);
        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
        {
            return trackChanges ?
                await context.Set<TEntity>().ToListAsync() :
                await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }



        public async Task<TEntity?> GetAsync(Tkey id) => await context.Set<TEntity>().FindAsync(id);

        public Task<TEntity?> GetAsync(ISpecification<TEntity> specification)
            => SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specification)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
            => await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specification).ToListAsync();

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await SpecificationsEvaluator.CreateQuery(context.Set<TEntity>(), specification).CountAsync();
        }
    }
}