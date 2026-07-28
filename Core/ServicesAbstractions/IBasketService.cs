using Shared.Dto.Basket;

namespace ServicesAbstractions
{
    public interface IBasketService
    {
        Task<BasketDto> GetAsync(string id);
        Task<BasketDto> UpdateAsync(BasketDto basket);
        Task<bool> DeleteAsync(string id);
    }
}
