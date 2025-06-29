global using Domain.Models;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenricReposatory<TEntity,Tkey>where TEntity:BaseEntity<Tkey>
    {

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetAsync(Tkey id );
        Task<int> CountAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetAsync(ISpecification<TEntity>specification );
        Task<IEnumerable<TEntity>>GetAllAsync(bool trackChanges=false);
        Task<IEnumerable<TEntity>>GetAllAsync(ISpecification<TEntity>specification);
    }
}
