using Shared.Dto.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class BasketsController(IServiceManger serviceManger)
           : ApiController
    {
        //basket by id
        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await serviceManger.BasketService.GetAsync(id);
            return Ok(basket);
        }
        // update basket(Baskedt Dto) => (CREATE ,REMOVE,UPDATE ,ADD)
        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket = await serviceManger.BasketService.UpdateAsync(basketDto);
            return Ok(basket);
        }
        // delete basket
        [HttpDelete("{id}")]
        public async Task<ActionResult<BasketDto>> Delete(string id)
        {
            await serviceManger.BasketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
