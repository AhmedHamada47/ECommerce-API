using Domain.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Reposatories
{
    public class BasketReposatory(IConnectionMultiplexer connectionMultiplexer) :
          IBasketReposatory
    {
        private readonly IDatabase database = connectionMultiplexer.GetDatabase();
        public async Task<bool> DeleteAsync(string id) => await database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetAsync(string id)
        {
            //get obj from data base
            //DEserzation
            //Return 
            var basket = await database.StringGetAsync(id);
            if (basket.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket!);

        }
        //user for carete and update 
        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await database.StringSetAsync(basket.Id, JsonBasket, timeToLive ?? TimeSpan.FromDays(30));
            return isCreatedOrUpdated ? await GetAsync(basket.Id) : null;
        }
    }
}
