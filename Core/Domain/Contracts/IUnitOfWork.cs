using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;

    public  interface IUnitOfWork
    {
    Task<int>SaveChangesAsync();
    IGenricReposatory<TEntity,Tkey>GetReposatory<TEntity, Tkey>()
        where TEntity : BaseEntity<Tkey>;
   
}

