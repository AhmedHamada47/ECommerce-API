using Shared.Dto.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IBasketService
    {
        Task<BasketDto> GetAsync(string id);
        Task<BasketDto> UpdateAsync(BasketDto basket);
        Task<bool> DeleteAsync(string id);


    }
}
