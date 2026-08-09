using AutoMapper;
using Domain.Basket;
using Domain.Contracts;
using Domain.Exceptions;
using Shared.Dto.Basket;
using ServicesAbstractions;

namespace Services
{
    internal class BasketService(IBasketRepository basketReposatory, IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteAsync(string id) => await basketReposatory.DeleteAsync(id);

        public async Task<BasketDto> GetAsync(string id)
        {
            var basket = await basketReposatory.GetAsync(id)
                ?? throw new BusinessException(BusinessErrorCode.BasketNotFound, id);
            return mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await basketReposatory.UpdateAsync(customerBasket)
                ?? throw new BusinessException(BusinessErrorCode.BasketUpdateFailed);
            return mapper.Map<BasketDto>(updatedBasket);
        }
    }
}