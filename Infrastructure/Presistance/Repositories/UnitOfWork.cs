using Domain.Contracts;
using Domain.Models;
using Presistance.Data;

namespace Presistance.Repositories
{
    public class UnitOfWork(StoreDbContext context)
        : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];

        public IGenricReposatory<TEntity, TKey> GetReposatory<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
                return (IGenricReposatory<TEntity, TKey>)_repositories[type];

            var repo = new GenericRepository<TEntity, TKey>(context);
            _repositories[type] = repo;
            return repo;
        }

        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
    }
}
