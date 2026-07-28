using Microsoft.AspNetCore.Mvc;
using Shared.Dto.Basket;
using ServicesAbstractions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await serviceManager.BasketService.GetAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket = await serviceManager.BasketService.UpdateAsync(basketDto);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await serviceManager.BasketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
