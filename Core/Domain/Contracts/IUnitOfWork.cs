using Domain.Models;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGenricReposatory<TEntity, TKey> GetReposatory<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>;
    }
}
