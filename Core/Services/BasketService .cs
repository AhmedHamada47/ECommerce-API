using Domain.Basket;
using Domain.Contracts;
using Domain.Exeptions;
using Shared.Dto.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class BasketService(IBasketReposatory basketReposatory, IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteAsync(string id) => await basketReposatory.DeleteAsync(id);

        public async Task<BasketDto> GetAsync(string id)
        {
            var basket = await basketReposatory.GetAsync(id) ??
                throw new BasketNotFoundExeption(id);
            return mapper.Map<BasketDto>(basket);

        }

        public async Task<BasketDto> UpdateAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            var updateBasket = await basketReposatory.UpdateAsync(customerBasket)
                ?? throw new Exception("Cant Update Basket Now!!!");
            return mapper.Map<BasketDto>(updateBasket);

        }
    }
}
