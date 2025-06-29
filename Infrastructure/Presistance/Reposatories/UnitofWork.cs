using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Reposatories
{
    public class UnitofWork(StoreDbContetx context)
        : IUnitOfWork
    {
        private readonly Dictionary<string,object> _reposatories = [];
        public IGenricReposatory<TEntity, Tkey> GetReposatory<TEntity, Tkey>() 
            where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (_reposatories.ContainsKey(type))
                return (IGenricReposatory<TEntity, Tkey>)_reposatories[type];
           else 
            {var repo= new GenericRepostatory<TEntity, Tkey>(context);
            _reposatories[type]= repo;
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();

    }
}