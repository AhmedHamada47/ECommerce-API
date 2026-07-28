using Domain.Models;

namespace Domain.Contracts
{
    public interface IGenricReposatory<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetAsync(TKey id);
        Task<int> CountAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetAsync(ISpecification<TEntity> specification);
        Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification);
    }
}
