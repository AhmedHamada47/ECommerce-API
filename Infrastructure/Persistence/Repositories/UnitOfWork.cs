using Domain.Contracts;
using Domain.Models;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext context)
        : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
                return (IGenericRepository<TEntity, TKey>)_repositories[type];

            var repo = new GenericRepository<TEntity, TKey>(context);
            _repositories[type] = repo;
            return repo;
        }

        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
    }
}
