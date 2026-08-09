using Domain.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetAsync(string id);
        Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan? timeSpan = null); //ttL time to live in memory
        Task<bool> DeleteAsync(string id);
    }
}
