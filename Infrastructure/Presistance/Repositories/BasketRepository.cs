using System.Text.Json;
using Domain.Basket;
using Domain.Contracts;
using StackExchange.Redis;

namespace Presistance.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connectionMultiplexer) :
          IBasketReposatory
    {
        private readonly IDatabase database = connectionMultiplexer.GetDatabase();

        public async Task<bool> DeleteAsync(string id) => await database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetAsync(string id)
        {
            var basket = await database.StringGetAsync(id);
            if (basket.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await database.StringSetAsync(basket.Id, jsonBasket, timeToLive ?? TimeSpan.FromDays(30));
            return isCreatedOrUpdated ? await GetAsync(basket.Id) : null;
        }
    }
}
